﻿// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Lab
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    using GihanSoft.Navigation;

    using Lab.Views.Pages;

    /// <summary>
    /// Interaction logic for MainWindow.xaml .
    /// </summary>
    public sealed partial class MainWindow : Window, IDisposable
    {
        private static readonly DependencyPropertyKey PageNavigatorPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(PageNavigator),
            typeof(PageNavigator),
            typeof(MainWindow),
            new PropertyMetadata(default(PageNavigator), (d, _) =>
            {
                (d as MainWindow) !.ThrowIfDisposed();
            }));

        /// <summary>Identifies the <see cref="PageNavigator"/> dependency property.</summary>
        public static readonly DependencyProperty PageNavigatorProperty = PageNavigatorPropertyKey.DependencyProperty;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.PageNavigator = new PageNavigator(App.Current.ServiceProvider);
            this.Loaded += this.MainWindow_LoadedAsync;
        }

        /// <summary>
        /// Gets Page Navigator.
        /// </summary>
        public PageNavigator? PageNavigator
        {
            get => (PageNavigator?)this.GetValue(PageNavigatorProperty);
            private set => this.SetValue(PageNavigatorPropertyKey, value);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.PageNavigator!.Dispose();

            this.disposed = true;
        }

        private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await this.PageNavigator!.GoToAsync<PgMain>().ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(false);
            await this.Dispatcher.Invoke(async () =>
            {
                await this.PageNavigator!.GoToAsync<PgNext>().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
