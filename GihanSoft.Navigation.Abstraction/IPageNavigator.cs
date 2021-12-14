using System.ComponentModel;
using System.Windows.Input;

namespace GihanSoft.Navigation.Abstraction;

/// <summary>
/// Page navigator. main service interface to use in logic (ViewModel, etc.).
/// </summary>
public interface IPageNavigator : INotifyPropertyChanged, IDisposable
{
    /// <summary>
    /// Fires before navigation. Can be used to cancel navigation.
    /// </summary>
    event EventHandler<NavigatingEventArgs>? Navigating;

    /// <summary>
    /// Fires after navigation.
    /// </summary>
    event EventHandler<NavigatedEventArgs>? Navigated;

    /// <summary>
    /// Gets a value indicating whether is there any page to navigate back.
    /// </summary>
    bool CanNavBack { get; }

    /// <summary>
    /// Gets a value indicating whether is there any page to navigate forward.
    /// </summary>
    bool CanNavForward { get; }

    /// <summary>
    /// Gets back history pages stack.
    /// </summary>
    IReadOnlyList<IPage> BackStack { get; }

    /// <summary>
    /// Gets forward history pages stack.
    /// </summary>
    IReadOnlyList<IPage> ForwardStack { get; }

    /// <summary>
    /// Gets current page.
    /// </summary>
    IPage? CurrentPage { get; }

    /// <summary>
    /// Gets navigate back command.
    /// </summary>
    ICommand CmdNavBack { get; }

    /// <summary>
    /// Gets navigate back until command.
    /// </summary>
    ICommand CmdNavBackUntil { get; }

    /// <summary>
    /// Gets navigate forward command.
    /// </summary>
    ICommand CmdNavForward { get; }

    /// <summary>
    /// Gets navigate forward until command.
    /// </summary>
    ICommand CmdNavForwardUntil { get; }

    /// <summary>
    /// Gets navigate to command.
    /// </summary>
    ICommand CmdNavTo { get; }

    /// <summary>
    /// Navigate back.
    /// </summary>
    void NavBack();

    /// <summary>
    /// Navigate back until input page.
    /// </summary>
    /// <param name="page">input page to navigate back to.</param>
    void NavBackUntil(IPage page);

    /// <summary>
    /// Navigate forward.
    /// </summary>
    void NavForward();

    /// <summary>
    /// Navigate forward until input page.
    /// </summary>
    /// <param name="page">input page to navigate forward to.</param>
    void NavForwardUntil(IPage page);

    /// <summary>
    /// get page from services and navigate to it.
    /// </summary>
    /// <typeparam name="TPage">type of page to create and navigating to.</typeparam>
    void NavTo<TPage>()
        where TPage : IPage;

    /// <summary>
    /// get page from services and navigate to it.
    /// </summary>
    /// <param name="pageType">type of page to create and navigating to.</param>
    void NavTo(Type pageType);
}