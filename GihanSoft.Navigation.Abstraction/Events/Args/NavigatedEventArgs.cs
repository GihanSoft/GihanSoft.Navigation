// -----------------------------------------------------------------------
// <copyright file="NavigatedEventArgs.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation.Abstraction.Events.Args
{
    using System;

    using GihanSoft.Navigation.Abstraction;

    /// <summary>
    /// Arguments of Navigated Event.
    /// </summary>
    public class NavigatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatedEventArgs"/> class.
        /// </summary>
        /// <param name="previous">previous page.</param>
        /// <param name="current">current page.</param>
        public NavigatedEventArgs(IPage previous, IPage current)
        {
            this.Previous = previous;
            this.Current = current;
        }

        /// <summary>
        /// Gets previous page.
        /// </summary>
        public IPage Previous { get; }

        /// <summary>
        /// Gets current page.
        /// </summary>
        public IPage Current { get; }
    }
}