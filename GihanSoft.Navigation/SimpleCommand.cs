// -----------------------------------------------------------------------
// <copyright file="PageNavigator.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace GihanSoft.Navigation
{
    internal class SimpleCommand : ICommand
    {
        private readonly Action action;
        private readonly Func<bool> canExecute;

        public SimpleCommand(Action action, Func<bool> canExecute)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public void Execute(object? parameter)
        {
            action();
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new());
        }
    }
}