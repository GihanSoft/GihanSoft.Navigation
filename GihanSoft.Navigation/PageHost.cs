﻿// -----------------------------------------------------------------------
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
        /// <summary>Identifies the <see cref="PageNavigator"/> dependency property.</summary>
        public static readonly DependencyProperty PageNavigatorProperty = DependencyProperty.Register(
            nameof(PageNavigator),
            typeof(PageNavigator),
            typeof(PageHost),
            new PropertyMetadata((d, e) =>
            {
                if (d is not PageHost pageHost)
                {
                    return;
                }

                Binding binding = new()
                {
                    Source = e.NewValue,
                    Path = new PropertyPath(nameof(Navigation.PageNavigator.Current), null),
                };
                pageHost.SetBinding(ContentProperty, binding);
            }));

        /// <summary>
        /// Gets or sets page navigator. please set it before doing anything else.
        /// </summary>
        public PageNavigator? PageNavigator
        {
            get => (PageNavigator?)this.GetValue(PageNavigatorProperty);
            set => this.SetValue(PageNavigatorProperty, value);
        }
    }
}