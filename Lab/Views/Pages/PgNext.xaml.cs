// -----------------------------------------------------------------------
// <copyright file="PgNext.xaml.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Lab.Views.Pages
{
    using System.Threading.Tasks;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for PgNext.xaml.
    /// </summary>
    public partial class PgNext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgNext"/> class.
        /// </summary>
        public PgNext()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public override async Task RefreshAsync()
        {
            for (int i = 5; i > 0; i--)
            {
                this.Dispatcher.Invoke(() => this.Tb.SetCurrentValue(TextBlock.TextProperty, $"{i}..."));

                await Task.Delay(1000 * 1).ConfigureAwait(false);
            }

            await base.RefreshAsync().ConfigureAwait(false);
            this.Dispatcher.Invoke(() => this.Tb.SetCurrentValue(TextBlock.TextProperty, "refreshed"));
        }
    }
}