﻿using System;
using System.Windows.Input;

namespace Battleship_2.Command
{
    public class AutoEventCommandBase : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _predicate;

        public AutoEventCommandBase(Action<object> action, Func<object, bool> predicate)
        {
            _action = action;
            _predicate = predicate;
        }

#pragma warning disable CS8604 // Possible null reference argument is safe here.

        public bool CanExecute(object? parameter) => _predicate(parameter);
        public void Execute(object? parameter) => _action(parameter);

#pragma warning restore CS8604 // Possible null reference argument.

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
