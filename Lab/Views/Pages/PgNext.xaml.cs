// -----------------------------------------------------------------------
// <copyright file="PgNext.xaml.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Lab.Views.Pages
{
    using System;
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

        /// <summary>
        /// Gets page type.
        /// </summary>
        public override Type Type { get; } = typeof(PgNext);

        /// <inheritdoc/>
        public override void Refresh()
        {
            async Task RefreshAsync()
            {
                for (int i = 5; i > 0; i--)
                {
                    this.Tb.SetCurrentValue(TextBlock.TextProperty, $"{i}...");
                    await Task.Delay(1000 * 1).ConfigureAwait(true);
                }

                base.Refresh();
                this.Tb.SetCurrentValue(TextBlock.TextProperty, "refreshed");
            }

            _ = RefreshAsync();
        }
    }
}