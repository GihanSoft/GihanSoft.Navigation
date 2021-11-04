// -----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Lab
{
    using System;
    using System.Windows;
    using System.Windows.Markup;

    using Lab.Views.Pages;

    /// <summary>
    /// Interaction logic for App.xaml .
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "...")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable.", Justification = "...")]
        public App()
        {
            ServiceProviders serviceProviders = new();
            serviceProviders.AddService(typeof(PgMain), new PgMain());
            serviceProviders.AddService(typeof(PgNext), new PgNext());
            this.ServiceProvider = serviceProviders;
        }

        /// <summary>
        /// Gets the <see cref="App"/> object for the current System.AppDomain.
        /// </summary>
        public static new App Current => (App)Application.Current;

        /// <summary>
        /// Gets service provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }
    }
}