// -----------------------------------------------------------------------
// <copyright file="PageNavigator.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using GihanSoft.Navigation.Events;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Page navigator.
    /// </summary>
    public class PageNavigator : DispatcherObject, INotifyPropertyChanged, IDisposable
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
        }

        /// <summary>
        /// Fires before navigation. Can be used to cancel navigation.
        /// </summary>
        public event EventHandler<NavigatingEventArgs>? Navigating;

        /// <summary>
        /// Fires after navigation.
        /// </summary>
        public event EventHandler<NavigatedEventArgs>? Navigated;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

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
        public IReadOnlyList<Page> BackStack
        {
            get
            {
                return this.backStack.ToImmutableArray();
            }
        }

        /// <summary>
        /// Gets forward history pages stack.
        /// </summary>
        public IReadOnlyList<Page> ForwardStack
        {
            get
            {
                return this.forwardStack.ToImmutableArray();
            }
        }

        /// <summary>
        /// Gets current page.
        /// </summary>
        public Page? Current { get; private set; }

        /// <summary>
        /// Create and navigate to a new page.
        /// </summary>
        /// <typeparam name="TPage">type of page to create and navigating to.</typeparam>
        /// <returns>true on successful navigation.</returns>
        public Task<bool> GoToAsync<TPage>()
            where TPage : Page
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(PageNavigator));
            }

            Page page = this.Dispatcher.Invoke(() =>
                ActivatorUtilities.GetServiceOrCreateInstance<TPage>(this.serviceProvider));
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
        public Task<bool> GoFrowardAsync()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(PageNavigator));
            }

            if (!this.CanGoForward)
            {
                throw new NavigationException();
            }

            return this.GoFrowardInternalAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
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
                    while (this.forwardStack.TryPop(out Page? page))
                    {
                        page?.Dispose();
                    }

#pragma warning disable IDISP007 // Don't dispose injected.
                    this.Current?.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected.
                    while (this.backStack.TryPop(out Page? page))
                    {
                        page?.Dispose();
                    }
                }

                this.disposedValue = true;
            }
        }

        private async Task<bool> GoToInternalAsync(Page page)
        {
            page.PageNavigator = this;
            NavigatingEventArgs? navigatingEventArgs = null;
            if (this.Current is not null)
            {
                navigatingEventArgs = new(this.Current, page);
                this.Navigating?.Invoke(this, navigatingEventArgs);
                if (navigatingEventArgs.Cancel)
                {
                    return false;
                }
            }

            while (this.forwardStack.TryPop(out Page? disposePage))
            {
                disposePage?.Dispose();
            }

            if (this.Current is not null)
            {
                this.backStack.Push(this.Current);
            }

            this.Current = page;
            this.OnPropertyChanged();
            await page.RefreshAsync().ConfigureAwait(false);

            if (navigatingEventArgs is not null)
            {
                this.Navigated?.Invoke(this, navigatingEventArgs);
            }

            return true;
        }

        private async Task<bool> GoBackInternalAsync()
        {
            Page backPage = this.backStack.Peek();
            NavigatingEventArgs navigatingEventArgs = new(this.Current!, backPage);
            this.Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            this.backStack.Pop();
            this.forwardStack.Push(this.Current!);

            this.Current = backPage;
            this.OnPropertyChanged();
            await backPage.RefreshAsync().ConfigureAwait(false);

            this.Navigated?.Invoke(this, navigatingEventArgs);

            return true;
        }

        private async Task<bool> GoFrowardInternalAsync()
        {
            Page forwardPage = this.forwardStack.Peek();
            NavigatingEventArgs navigatingEventArgs = new(this.Current!, forwardPage);
            this.Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            this.forwardStack.Pop();
            this.backStack.Push(this.Current!);

            this.Current = forwardPage;
            this.OnPropertyChanged();
            await forwardPage.RefreshAsync().ConfigureAwait(false);

            this.Navigated?.Invoke(this, navigatingEventArgs);

            return true;
        }

        private void OnPropertyChanged()
        {
            this.PropertyChanged?.Invoke(this, new(nameof(this.Current)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.BackStack)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.CanGoBack)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.ForwardStack)));
            this.PropertyChanged?.Invoke(this, new(nameof(this.CanGoForward)));
        }
    }
}
