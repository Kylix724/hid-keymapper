using System;
using System.Windows.Input;

namespace InterceptionKeymapper.Helpers
{
	public class ActionCommand : ICommand

	{
		private readonly Action<object> _exec;
		private readonly Predicate<object> _canExecute;

		public ActionCommand(Action<object> exec) : this(exec, null) { }

		public ActionCommand(Action<object> exec, Predicate<object> canExecute)
		{
			if (exec == null)
			{
				throw new ArgumentNullException("execute");
			}
			_exec = exec;
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_exec(parameter);
		}
	}
}
