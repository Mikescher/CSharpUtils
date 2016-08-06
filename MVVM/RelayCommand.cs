using System;
using System.Windows.Input;

namespace MSHC.MVVM
{
	/// <summary>
	/// http://stackoverflow.com/a/22286816/1761622
	/// </summary>
	public class RelayCommand : ICommand
	{
		#region Fields

		readonly Action<object> _execute;
		readonly Predicate<object> _canExecute;

		#endregion

		#region Constructors

		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			if (execute == null)
				throw new ArgumentNullException(nameof(execute));

			_execute = execute;
			_canExecute = canExecute;
		}

		public RelayCommand(Action execute, Predicate<object> canExecute = null)
		{
			if (execute == null)
				throw new ArgumentNullException(nameof(execute));

			_execute = x=>execute();
			_canExecute = canExecute;
		}

		#endregion

		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke(parameter) ?? true;
		}
		
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		
		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		#endregion
	}
}
