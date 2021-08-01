// -----------------------------------------------------------------------
// <copyright file="SimpleCommand.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A simple command to use internally.
    /// </summary>
    internal class SimpleCommand : ICommand
    {
        private readonly Action action;
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand"/> class.
        /// </summary>
        /// <param name="action">action to execute.</param>
        /// <param name="canExecute">function to check can execute.</param>
        public SimpleCommand(Action action, Func<bool> canExecute)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc />
        public bool CanExecute(object? parameter) => this.canExecute();

        /// <inheritdoc />
        public void Execute(object? parameter) => this.action();

        /// <summary>
        /// Call on <see cref="CanExecute"/> changed.
        /// </summary>
        public void OnCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, new());
    }
}