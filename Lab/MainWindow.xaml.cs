// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Lab
{
    using System.Threading.Tasks;
    using System.Windows;

    using Lab.Views.Pages;

    /// <summary>
    /// Interaction logic for MainWindow.xaml .
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.PageHost.PageNavigator = new GihanSoft.Navigation.PageNavigator(App.Current.ServiceProvider);
            this.Loaded += this.MainWindow_LoadedAsync;
        }

        private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await this.PageHost.PageNavigator!.GoToAsync<PgMain>().ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(false);
            await this.PageHost.PageNavigator!.GoToAsync<PgNext>().ConfigureAwait(false);
        }
    }
}
