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
    using System.Windows;

    using GihanSoft.Navigation.Events;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Page navigator.
    /// </summary>
    public class PageNavigator : DependencyObject
    {
        private static readonly DependencyPropertyKey CurrentPropertyKey
            = DependencyProperty.RegisterReadOnly(
                nameof(Current),
                typeof(Page),
                typeof(PageNavigator),
                new PropertyMetadata());

        /// <summary>Identifies the <see cref="Current"/> dependency property.</summary>
        public static readonly DependencyProperty CurrentProperty = CurrentPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey CanGoBackPropertyKey
            = DependencyProperty.RegisterReadOnly(
                nameof(CanGoBack),
                typeof(bool),
                typeof(PageNavigator),
                new PropertyMetadata(false));

        /// <summary>Identifies the <see cref="CanGoBack"/> dependency property.</summary>
        public static readonly DependencyProperty CanGoBackProperty
            = CanGoBackPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey CanGoForwardPropertyKey
            = DependencyProperty.RegisterReadOnly(
                nameof(CanGoForward),
                typeof(bool),
                typeof(PageNavigator),
                new PropertyMetadata(false));

        /// <summary>Identifies the <see cref="CanGoForward"/> dependency property.</summary>
        public static readonly DependencyProperty CanGoForwardProperty
            = CanGoForwardPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey BackStackPropertyKey
            = DependencyProperty.RegisterReadOnly(
                nameof(BackStack),
                typeof(IEnumerable<Page>),
                typeof(PageNavigator),
                new PropertyMetadata());

        /// <summary>Identifies the <see cref="BackStack"/> dependency property.</summary>
        public static readonly DependencyProperty BackStackProperty
            = BackStackPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ForwardStackPropertyKey
            = DependencyProperty.RegisterReadOnly(
                nameof(ForwardStack),
                typeof(IEnumerable<Page>),
                typeof(PageNavigator),
                new PropertyMetadata());

        /// <summary>Identifies the <see cref="ForwardStack"/> dependency property.</summary>
        public static readonly DependencyProperty ForwardStackProperty
            = ForwardStackPropertyKey.DependencyProperty;

        private readonly IServiceProvider serviceProvider;

        private readonly Stack<Page> backStack;
        private readonly Stack<Page> forwardStack;

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

        /// <summary>
        /// Gets a value indicating whether is there any page to go back.
        /// </summary>
        public bool CanGoBack => (bool)this.GetValue(CanGoBackProperty);

        /// <summary>
        /// Gets a value indicating whether is there any page to go forward.
        /// </summary>
        public bool CanGoForward => (bool)this.GetValue(CanGoForwardProperty);

        /// <summary>
        /// Gets back history pages stack.
        /// </summary>
        public IEnumerable<Page> BackStack => (IEnumerable<Page>)this.GetValue(BackStackProperty);

        /// <summary>
        /// Gets forward history pages stack.
        /// </summary>
        public IEnumerable<Page> ForwardStack => (IEnumerable<Page>)this.GetValue(ForwardStackProperty);

        /// <summary>
        /// Gets current page.
        /// </summary>
        public Page Current => (Page)this.GetValue(CurrentProperty);

        /// <summary>
        /// go back.
        /// </summary>
        /// <returns>true on successful going back.</returns>
        public bool GoBack()
        {
            if (!this.CanGoBack)
            {
                throw new NavigationException();
            }

            Page backPage = this.backStack.Peek();
            NavigatingEventArgs navigatingEventArgs = new(this.Current, backPage);
            this.Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            this.backStack.Pop();
            this.forwardStack.Push(this.Current);

            this.SetValue(CurrentPropertyKey, backPage);
            backPage.Refresh?.Invoke();
            this.SetValue(CanGoBackPropertyKey, this.backStack.Count > 0);
            this.SetValue(CanGoForwardPropertyKey, true);
            this.SetValue(BackStackPropertyKey, this.backStack.ToImmutableArray());
            this.SetValue(ForwardStackPropertyKey, this.forwardStack.ToImmutableArray());

            this.Navigated?.Invoke(this, navigatingEventArgs);

            return true;
        }

        /// <summary>
        /// go forward.
        /// </summary>
        /// <returns>true on successful going forward.</returns>
        public bool GoFroward()
        {
            if (!this.CanGoForward)
            {
                throw new NavigationException();
            }

            Page forwardPage = this.forwardStack.Peek();
            NavigatingEventArgs navigatingEventArgs = new(this.Current, forwardPage);
            this.Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            this.forwardStack.Pop();
            this.backStack.Push(this.Current);

            this.SetValue(CurrentPropertyKey, forwardPage);
            forwardPage.Refresh?.Invoke();
            this.SetValue(CanGoBackPropertyKey, true);
            this.SetValue(CanGoForwardPropertyKey, this.forwardStack.Count > 0);
            this.SetValue(BackStackPropertyKey, this.backStack.ToImmutableArray());
            this.SetValue(ForwardStackPropertyKey, this.forwardStack.ToImmutableArray());

            this.Navigated?.Invoke(this, navigatingEventArgs);

            return true;
        }

        /// <summary>
        /// Create and navigate to a new page.
        /// </summary>
        /// <typeparam name="TPage">type of page to create and navigating to.</typeparam>
        /// <returns>true on successful navigation.</returns>
        public bool GoTo<TPage>()
            where TPage : Page
        {
            Page page = ActivatorUtilities.GetServiceOrCreateInstance<TPage>(this.serviceProvider);
            return this.GoTo(page);
        }

        /// <summary>
        /// Create and navigate to a new page.
        /// </summary>
        /// <param name="page">page which navigating to. </param>
        /// <returns>true on successful navigation.</returns>
        public bool GoTo(Page page)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            NavigatingEventArgs navigatingEventArgs = new(this.Current, page);
            this.Navigating?.Invoke(this, navigatingEventArgs);
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            while (this.forwardStack.Count > 0)
            {
                Page disposePage = this.forwardStack.Pop();
                disposePage.Dispose();
            }

            if (this.Current is not null)
            {
                this.backStack.Push(this.Current);
            }

            this.SetValue(CurrentPropertyKey, page);
            page.Refresh?.Invoke();
            this.SetValue(CanGoBackPropertyKey, this.backStack.Count > 0);
            this.SetValue(CanGoForwardPropertyKey, false);
            this.SetValue(BackStackPropertyKey, this.backStack.ToImmutableArray());
            this.SetValue(ForwardStackPropertyKey, this.forwardStack.ToImmutableArray());

            this.Navigated?.Invoke(this, navigatingEventArgs);

            return true;
        }
    }
}
