using System;
using System.Windows.Input;

namespace Play.Flies.Client.Architecture
{
    public abstract class RelayCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
    }

    public class RelayCommand : RelayCommandBase
    {
        private readonly Func<bool> _canExcecute;
        private readonly Action _exceute;

        public RelayCommand(Action exceute, Func<bool> canExcecute = null)
        {
            _exceute = exceute;
            _canExcecute = canExcecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExcecute?.Invoke() ?? true;
        }

        public override void Execute(object parameter)
        {
            _exceute?.Invoke();
        }
    }

    public class RelayCommand<T> : RelayCommandBase
    {
        private readonly Func<T, bool> _canExcecute;
        private readonly Action<T> _exceute;

        public RelayCommand(Action<T> exceute, Func<T, bool> canExcecute = null)
        {
            _exceute = exceute;
            _canExcecute = canExcecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExcecute?.Invoke(Cast(parameter)) ?? true;
        }

        public override void Execute(object parameter)
        {
            _exceute?.Invoke(Cast(parameter));
        }

        private static T Cast(object parameter)
        {
            if (parameter is T) return (T) parameter;
            return (T) Convert.ChangeType(parameter, typeof(T));
        }
    }
}