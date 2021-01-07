using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MSHC.WPFStub
{
    public class SimpleCommand<T> : ICommand, ITypedCommand<T>
    {
        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;

        public SimpleCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute.Invoke((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
