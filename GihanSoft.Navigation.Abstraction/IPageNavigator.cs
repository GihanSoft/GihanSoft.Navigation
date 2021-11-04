// -----------------------------------------------------------------------
// <copyright file="IPageNavigator.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using GihanSoft.Navigation.Abstraction.Events;
    using GihanSoft.Navigation.Abstraction.Events.Args;

    /// <summary>
    /// Page navigator. main service interface to use in logic (ViewModel, etc.).
    /// </summary>
    [SuppressMessage("Design", "CA1003:Use generic event handler instances", Justification = "Generic is king.")]
    public interface IPageNavigator : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Fires before navigation. Can be used to cancel navigation.
        /// </summary>
        event EventHandlerAsync<IPageNavigator, NavigatingEventArgs>? Navigating;

        /// <summary>
        /// Fires after navigation.
        /// </summary>
        event EventHandlerAsync<IPageNavigator, NavigatedEventArgs>? Navigated;

        /// <summary>
        /// Gets a value indicating whether is there any page to go back.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether is there any page to go forward.
        /// </summary>
        bool CanGoForward { get; }

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
        /// get page from services and navigate to it.
        /// </summary>
        /// <typeparam name="TPage">type of page to create and navigating to.</typeparam>
        /// <returns>true on successful navigation.</returns>
        Task<bool> GoToAsync<TPage>()
            where TPage : IPage;

        /// <summary>
        /// get page from services and navigate to it.
        /// </summary>
        /// <param name="pageType">type of page to create and navigating to.</param>
        /// <returns>true on successful navigation.</returns>
        Task<bool> GoToAsync(Type pageType);

        /// <summary>
        /// go back.
        /// </summary>
        /// <returns>true on successful going back.</returns>
        Task<bool> GoBackAsync();

        /// <summary>
        /// go forward.
        /// </summary>
        /// <returns>true on successful going forward.</returns>
        Task<bool> GoForwardAsync();
    }
}