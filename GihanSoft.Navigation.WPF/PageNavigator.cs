using System.Collections.Immutable;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

using GihanSoft.Navigation.Abstraction;

using Microsoft.Extensions.DependencyInjection;

namespace GihanSoft.Navigation.WPF;

/// <summary>
/// Page navigator.
/// </summary>
public class PageNavigator : DispatcherObject, IPageNavigator
{
    private readonly IServiceProvider serviceProvider;

    private readonly Stack<Page> backStack;
    private readonly Stack<Page> forwardStack;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageNavigator"/> class.
    /// </summary>
    /// <param name="serviceProvider">service provider for building pages.</param>
    public PageNavigator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        backStack = new Stack<Page>();
        forwardStack = new Stack<Page>();

        CmdNavBack = new ActionCommand(NavBack, () => CanNavBack);
        CmdNavBackUntil = new ActionCommand<IPage>(NavBackUntil, _ => CanNavBack);
        CmdNavForward = new ActionCommand(NavForward, () => CanNavForward);
        CmdNavForwardUntil = new ActionCommand<IPage>(NavForwardUntil, _ => CanNavForward);
        CmdNavTo = new ActionCommand<Type>(NavTo, _ => true);
    }

    /// <inheritdoc/>
    public event EventHandler<NavigatingEventArgs>? Navigating;

    /// <inheritdoc/>
    public event EventHandler<NavigatedEventArgs>? Navigated;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public ICommand CmdNavBack { get; }

    /// <inheritdoc/>
    public ICommand CmdNavBackUntil { get; }

    /// <inheritdoc/>
    public ICommand CmdNavForward { get; }

    /// <inheritdoc/>
    public ICommand CmdNavForwardUntil { get; }

    /// <inheritdoc/>
    public ICommand CmdNavTo { get; }

    /// <inheritdoc/>
    public bool CanNavBack => backStack.Count > 0;

    /// <inheritdoc/>
    public bool CanNavForward => forwardStack.Count > 0;

    /// <inheritdoc/>
    public IReadOnlyList<IPage> BackStack => backStack.ToImmutableArray();

    /// <inheritdoc/>
    public IReadOnlyList<IPage> ForwardStack => forwardStack.ToImmutableArray();

    /// <inheritdoc/>
    public IPage? CurrentPage { get; private set; }

    /// <inheritdoc/>
    public void NavBack()
    {
        NavBackUntil(backStack.Peek());
    }

    /// <inheritdoc/>
    public void NavBackUntil(IPage page)
    {
        if (disposedValue)
        {
            throw new ObjectDisposedException(nameof(PageNavigator));
        }

        if (page is not Page pg)
        {
            throw new ArgumentException("page is not 'Page'", nameof(page));
        }

        if (!CanNavBack || !backStack.Contains(page))
        {
            throw new NavigationException();
        }

        TryNavBackUntil(pg);
    }

    /// <inheritdoc/>
    public void NavForward()
    {
        NavForwardUntil(forwardStack.Peek());
    }

    /// <inheritdoc/>
    public void NavForwardUntil(IPage page)
    {
        if (disposedValue)
        {
            throw new ObjectDisposedException(nameof(PageNavigator));
        }

        if (page is not Page pg)
        {
            throw new ArgumentException("page is not 'Page'", nameof(page));
        }

        if (!CanNavForward || !forwardStack.Contains(pg))
        {
            throw new NavigationException();
        }

        TryNavForwardUntil(pg);
    }

    /// <inheritdoc/>
    public void NavTo<TPage>()
        where TPage : IPage
    {
        NavTo(typeof(TPage));
    }

    /// <inheritdoc/>
    public void NavTo(Type pageType)
    {
        if (disposedValue)
        {
            throw new ObjectDisposedException(nameof(PageNavigator));
        }

        if (pageType is null)
        {
            throw new ArgumentNullException(nameof(pageType));
        }

        IPage page = Dispatcher.Invoke(() =>
            (IPage)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, pageType));

        if (page is not Page pg)
        {
            throw new ArgumentException("invalid type", nameof(pageType));
        }

        if (!TryNavTo(pg))
        {
            page.Dispose();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Real Dispose Function.
    /// </summary>
    /// <param name="disposing">true if managed dispose.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                while (forwardStack.Count > 0)
                {
                    forwardStack.Pop()?.Dispose();
                }

                CurrentPage?.Dispose();
                while (backStack.Count > 0)
                {
                    backStack.Pop()?.Dispose();
                }
            }

            disposedValue = true;
        }
    }

    private void TryNavBackUntil(Page page)
    {
        NavigatingEventArgs navigatingEventArgs = new(CurrentPage!, page);
        Navigating?.Invoke(this, navigatingEventArgs);
        if (navigatingEventArgs.Cancel)
        {
            return;
        }

        while (CurrentPage != page)
        {
            forwardStack.Push((CurrentPage as Page)!);
            CurrentPage = backStack.Pop();
        }

        OnPropertyChanged();
        CurrentPage.Refresh();
        Navigated?.Invoke(this, navigatingEventArgs);
    }

    private void TryNavForwardUntil(Page page)
    {
        NavigatingEventArgs navigatingEventArgs = new(CurrentPage!, page);
        Navigating?.Invoke(this, navigatingEventArgs);
        if (navigatingEventArgs.Cancel)
        {
            return;
        }

        while (CurrentPage != page)
        {
            backStack.Push((CurrentPage as Page)!);
            CurrentPage = forwardStack.Pop();
        }

        OnPropertyChanged();
        CurrentPage.Refresh();
        Navigated?.Invoke(this, navigatingEventArgs);
    }

    private bool TryNavTo(Page page)
    {
        NavigatingEventArgs? navigatingEventArgs = null;
        if (CurrentPage is not null)
        {
            navigatingEventArgs = new(CurrentPage, page);
            Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }
        }

        while (forwardStack.Count > 0)
        {
            forwardStack.Pop()?.Dispose();
        }

        if (CurrentPage is not null)
        {
            var pg = CurrentPage as Page;
            backStack.Push(pg!);
        }

        CurrentPage = page;
        OnPropertyChanged();

        page.Refresh();
        if (navigatingEventArgs is not null)
        {
            Navigated?.Invoke(this, navigatingEventArgs);
        }

        return true;
    }

    private void OnPropertyChanged()
    {
        PropertyChanged?.Invoke(this, new(nameof(CurrentPage)));
        PropertyChanged?.Invoke(this, new(nameof(BackStack)));
        PropertyChanged?.Invoke(this, new(nameof(CanNavBack)));
        PropertyChanged?.Invoke(this, new(nameof(ForwardStack)));
        PropertyChanged?.Invoke(this, new(nameof(CanNavForward)));

        (CmdNavBack as ActionCommand)?.OnCanExecuteChanged();
        (CmdNavBackUntil as ActionCommand)?.OnCanExecuteChanged();
        (CmdNavForward as ActionCommand)?.OnCanExecuteChanged();
        (CmdNavForwardUntil as ActionCommand)?.OnCanExecuteChanged();
    }
}