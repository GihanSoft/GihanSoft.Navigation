// -----------------------------------------------------------------------
// <copyright file="Page.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Page type to use as base of navigation pages.
    /// </summary>
    public class Page : UserControl, IDisposable
    {
        /// <summary>Identifies the <see cref="LeftToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty LeftToolBarProperty
            = DependencyProperty.Register(
                nameof(LeftToolBar),
                typeof(ToolBar),
                typeof(Page),
                new PropertyMetadata((_, e) =>
                {
                    if(e.NewValue is not ToolBar stackPanel)
                    {
                        return;
                    }
                }));

        /// <summary>Identifies the <see cref="RightToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty RightToolBarProperty
            = DependencyProperty.Register(
                nameof(RightToolBar),
                typeof(ToolBar),
                typeof(Page),
                new PropertyMetadata((_, e) =>
                {
                    if (e.NewValue is not ToolBar stackPanel)
                    {
                        return;
                    }
                }));

        private bool disposedValue;

        /// <summary>
        /// Gets <see cref="Navigation.PageNavigator"/> of page.
        /// </summary>
        public PageNavigator? PageNavigator { get; internal set; }

        /// <summary>
        /// Gets or sets action to refresh page. called after navigation and going back and forward.
        /// </summary>
        public Action? Refresh { get; protected set; }

        /// <summary>
        /// Gets or sets left tool bar of page.
        /// </summary>
        public virtual ToolBar? LeftToolBar
        {
            get => (ToolBar?)this.GetValue(LeftToolBarProperty);
            set => this.SetValue(LeftToolBarProperty, value);
        }

        /// <summary>
        /// Gets or sets right tool bar of page.
        /// </summary>
        public virtual ToolBar? RightToolBar
        {
            get => (ToolBar?)this.GetValue(RightToolBarProperty);
            set => this.SetValue(RightToolBarProperty, value);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// dispose page.
        /// </summary>
        /// <param name="disposing">true to dispose managed types.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TO DO: dispose managed state (managed objects)
                }

                // TO DO: free unmanaged resources (unmanaged objects) and override finalizer
                // TO DO: set large fields to null
                this.disposedValue = true;
            }
        }
    }
}
