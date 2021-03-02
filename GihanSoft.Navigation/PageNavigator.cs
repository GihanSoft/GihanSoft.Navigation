// <copyright file="PageNavigator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GihanSoft.Navigation
{
    using System.Windows;

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

        /// <summary>
        /// Gets current page.
        /// </summary>
        public Page Current => (Page)this.GetValue(CurrentProperty);
    }
}
