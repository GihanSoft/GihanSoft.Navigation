// -----------------------------------------------------------------------
// <copyright file="NavigatingEventArgs.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation.Abstraction.Events.Args
{
    using GihanSoft.Navigation.Abstraction;

    /// <summary>
    /// Arguments of Navigating event.
    /// </summary>
    public class NavigatingEventArgs : NavigatedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatingEventArgs"/> class.
        /// </summary>
        /// <param name="current">current page.</param>
        /// <param name="future">future page.</param>
        public NavigatingEventArgs(IPage current, IPage future)
            : base(current, future)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether navigation should be canceled or not.
        /// </summary>
        public bool Cancel { get; set; }
    }
}