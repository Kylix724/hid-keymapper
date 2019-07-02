using System;
using System.Windows.Input;

namespace HidKeymapper.Helpers
{
	/// <summary>
	/// https://stackoverflow.com/a/629450
	/// </summary>
	public abstract class CommandModelBase : ICommand
	{
		RoutedCommand routedCommand_;

		/// <summary>
		/// Expose a command that can be bound to from XAML.
		/// </summary>
		public RoutedCommand Command
		{
			get { return routedCommand_; }
		}

		/// <summary>
		/// Initialise the command.
		/// </summary>
		public CommandModelBase()
		{
			routedCommand_ = new RoutedCommand();
		}

		/// <summary>
		/// Default implementation always allows the command to execute.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnQueryEnabled(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CanExecute(e.Parameter);
			e.Handled = true;
		}

		/// <summary>
		/// Subclasses must provide the execution logic.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnExecute(object sender, ExecutedRoutedEventArgs e)
		{
			Execute(e.Parameter);
		}

		#region ICommand Members

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public abstract void Execute(object parameter);

		#endregion
	}
}
