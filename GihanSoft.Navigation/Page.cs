// -----------------------------------------------------------------------
// <copyright file="Page.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Page type to use as base of navigation pages.
    /// </summary>
    public class Page : UserControl, IDisposable
    {

        private static readonly PropertyChangedCallback ThrowOnDisposed = (d, _) =>
        {
            if (d is Page page && page.disposedValue)
            {
                throw new ObjectDisposedException(nameof(Page));
            }
        };

        private static readonly DependencyPropertyKey TitlePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Title),
            typeof(string),
            typeof(Page),
            new PropertyMetadata(default(string)));

        /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
        public static readonly DependencyProperty TitleProperty = TitlePropertyKey.DependencyProperty;

        /// <summary>Identifies the <see cref="LeftToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty LeftToolBarProperty = DependencyProperty.Register(
            nameof(LeftToolBar),
            typeof(ToolBar),
            typeof(Page),
            new(default(ToolBar), ThrowOnDisposed));

        /// <summary>Identifies the <see cref="RightToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty RightToolBarProperty = DependencyProperty.Register(
            nameof(RightToolBar),
            typeof(ToolBar),
            typeof(Page),
            new(default(ToolBar), ThrowOnDisposed));

        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        public Page()
        {
            Title = GetType().Name;
        }

        /// <summary>
        /// Gets title of page.
        /// </summary>
        public virtual string? Title
        {
            get => (string?)this.GetValue(TitleProperty);
            private set => this.SetValue(TitlePropertyKey, value);
        }

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

        /// <summary>
        /// refresh page. called after navigation and going back and forward.
        /// </summary>
        /// <returns>task of refresh.</returns>
        public virtual Task RefreshAsync()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(Page));
            }

            return Task.CompletedTask;
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
