// -----------------------------------------------------------------------
// <copyright file="PageHost.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for PageHost.xaml.
    /// </summary>
    public class PageHost : UserControl
    {
        private PageNavigator? pageNavigator;

        /// <summary>
        /// Gets or sets page navigator. please set it before doing anything else.
        /// </summary>
        public PageNavigator? PageNavigator
        {
            get => this.pageNavigator;
            set
            {
                this.SetBinding(ContentProperty, new Binding
                {
                    Source = value,
                    Path = new(nameof(value.Current), null),
                });
                this.pageNavigator = value;
            }
        }
    }
}
