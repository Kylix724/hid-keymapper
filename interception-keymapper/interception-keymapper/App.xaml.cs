using System.Windows;

namespace InterceptionKeymapper
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			MainWindow MainWindow = new MainWindow();
			MainWindow.DataContext = new ViewModel.ApplicationObservableModel();
			MainWindow.Show();
		}
	}
}
