// -----------------------------------------------------------------------
// <copyright file="PageNavigator.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation.WPF
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Threading;

    using GihanSoft.Navigation.Abstraction;
    using GihanSoft.Navigation.Abstraction.Events;
    using GihanSoft.Navigation.Abstraction.Events.Args;

    using Microsoft.Extensions.DependencyInjection;

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
            this.backStack = new Stack<Page>();
            this.forwardStack = new Stack<Page>();

            this.GoBackCommand = new ActionCommand(
                () => _ = this.GoBackAsync().ConfigureAwait(false),
                () => this.CanGoBack);

            this.GoForwardCommand = new ActionCommand(
                () => _ = this.GoForwardAsync().ConfigureAwait(false),
                () => this.CanGoForward);
        }

        /// <summary>
        /// Fires before navigation. Can be used to cancel navigation.
        /// </summary>
        public event EventHandlerAsync<IPageNavigator, NavigatingEventArgs>? Navigating;

        /// <summary>
        /// Fires after navigation.
        /// </summary>
        public event EventHandlerAsync<IPageNavigator, NavigatedEventArgs>? Navigated;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets Go back command.
        /// </summary>
        public ICommand GoBackCommand { get; }

        /// <summary>
        /// Gets Go forward command.
        /// </summary>
        public ICommand GoForwardCommand { get; }

        /// <summary>
        /// Gets a value indicating whether is there any page to go back.
        /// </summary>
        public bool CanGoBack => this.backStack.Count > 0;

        /// <summary>
        /// Gets a value indicating whether is there any page to go forward.
        /// </summary>
        public bool CanGoForward => this.forwardStack.Count > 0;

        /// <summary>
        /// Gets back history pages stack.
        /// </summary>
        public IReadOnlyList<IPage> BackStack => this.backStack.ToImmutableArray();

        /// <summary>
        /// Gets forward history pages stack.
        /// </summary>
        public IReadOnlyList<IPage> ForwardStack => this.forwardStack.ToImmutableArray();

        /// <summary>
        /// Gets current page.
        /// </summary>
        public IPage? CurrentPage { get; private set; }

        /// <summary>
        /// Create and navigate to a new page.
        /// </summary>
        /// <typeparam name="TPage">type of page to create and navigating to.</typeparam>
        /// <returns>true on successful navigation.</returns>
        public Task<bool> GoToAsync<TPage>()
            where TPage : IPage
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(PageNavigator));
            }

            IPage page = this.Dispatcher.Invoke(() => this.serviceProvider.GetRequiredService<TPage>());
            if (page is not Page pg)
            {
                throw new NavigationException();
            }

            return this.GoToInternalAsync(pg);
        }

        /// <summary>
        /// Create and navigate to a new page.
        /// </summary>
        /// <param name="pageType">type of page to create and navigating to.</param>
        /// <returns>true on successful navigation.</returns>
        public Task<bool> GoToAsync(Type pageType)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(PageNavigator));
            }

            if (pageType is null)
            {
                throw new ArgumentNullException(nameof(pageType));
            }

            if (!pageType.IsSubclassOf(typeof(Page)))
            {
                throw new ArgumentException("invalid type", nameof(pageType));
            }

            Page page = this.Dispatcher.Invoke(() =>
                (Page)ActivatorUtilities.GetServiceOrCreateInstance(this.serviceProvider, pageType));

            return this.GoToInternalAsync(page);
        }

        /// <summary>
        /// go back.
        /// </summary>
        /// <returns>true on successful going back.</returns>
        public Task<bool> GoBackAsync()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(PageNavigator));
            }

            if (!this.CanGoBack)
            {
                throw new NavigationException();
            }

            return this.GoBackInternalAsync();
        }

        /// <summary>
        /// go forward.
        /// </summary>
        /// <returns>true on successful going forward.</returns>
        public Task<bool> GoForwardAsync()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(PageNavigator));
            }

            if (!this.CanGoForward)
            {
                throw new NavigationException();
            }

            return this.GoForwardInternalAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Real Dispose Function.
        /// </summary>
        /// <param name="disposing">true if managed dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
#if NET5_0_OR_GREATER
                    while (this.forwardStack.TryPop(out Page? page))
                    {
                        page?.Dispose();
                    }
#else
                    while (this.forwardStack.Count > 0)
                    {
                        this.forwardStack.Pop()?.Dispose();
                    }
#endif
                    this.CurrentPage?.Dispose();
#if NET5_0_OR_GREATER
                    while (this.backStack.TryPop(out Page? page))
                    {
                        page?.Dispose();
                    }
#else
                    while (this.backStack.Count > 0)
                    {
                        this.backStack.Pop()?.Dispose();
                    }
#endif
                }

                this.disposedValue = true;
            }
        }

        private async Task<bool> GoToInternalAsync(Page page)
        {
            NavigatingEventArgs? navigatingEventArgs = null;
            if (this.CurrentPage is not null)
            {
                navigatingEventArgs = new(this.CurrentPage, page);
                this.Navigating?.Invoke(this, navigatingEventArgs);
                if (navigatingEventArgs.Cancel)
                {
                    return false;
                }
            }

#if NET5_0_OR_GREATER
            while (this.forwardStack.TryPop(out Page? disposePage))
            {
                disposePage?.Dispose();
            }
#else
            while (this.forwardStack.Count > 0)
            {
                this.forwardStack.Pop()?.Dispose();
            }
#endif

            if (this.CurrentPage is not null)
            {
                var pg = this.CurrentPage as Page;
                this.backStack.Push(pg!);
            }

            this.CurrentPage = page;
            this.OnPropertyChanged();
            await page.RefreshAsync().ConfigureAwait(false);

            if (navigatingEventArgs is not null)
            {
                await this.Dispatcher.Invoke(() => this.Navigated?.Invoke(this, navigatingEventArgs) ?? Task.CompletedTask)
                    .ConfigureAwait(false);
            }

            return true;
        }

        private async Task<bool> GoBackInternalAsync()
        {
            Page backPage = this.backStack.Peek();
            NavigatingEventArgs navigatingEventArgs = new(this.CurrentPage!, backPage);
            this.Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            this.backStack.Pop();
            var pg = this.CurrentPage as Page;
            this.forwardStack.Push(pg!);

            this.CurrentPage = backPage;
            this.OnPropertyChanged();
            await backPage.RefreshAsync().ConfigureAwait(false);

            await this.Dispatcher.Invoke(() => this.Navigated?.Invoke(this, navigatingEventArgs) ?? Task.CompletedTask)
                .ConfigureAwait(false);

            return true;
        }

        private async Task<bool> GoForwardInternalAsync()
        {
            Page forwardPage = this.forwardStack.Peek();
            NavigatingEventArgs navigatingEventArgs = new(this.CurrentPage!, forwardPage);
            this.Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            this.forwardStack.Pop();
            var pg = this.CurrentPage as Page;
            this.backStack.Push(pg!);

            this.CurrentPage = forwardPage;
            this.OnPropertyChanged();
            await forwardPage.RefreshAsync().ConfigureAwait(false);

            await this.Dispatcher.Invoke(() => this.Navigated?.Invoke(this, navigatingEventArgs) ?? Task.CompletedTask)
                .ConfigureAwait(false);

            return true;
        }

        private void OnPropertyChanged()
        {
            this.PropertyChanged?.Invoke(this, new(nameof(this.CurrentPage)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.BackStack)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.CanGoBack)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.ForwardStack)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.CanGoForward)));

            (this.GoBackCommand as ActionCommand)?.OnCanExecuteChanged();
            (this.GoForwardCommand as ActionCommand)?.OnCanExecuteChanged();
        }
    }
}