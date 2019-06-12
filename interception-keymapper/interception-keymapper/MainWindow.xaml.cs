using InterceptionKeymapper.Helpers;
using InterceptionKeymapper.Model;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace InterceptionKeymapper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		private Vars VARS;
		private ShortcutManager SM;
		private DeviceManager DM;
		public MainWindow()
		{
			InitializeComponent();
			Loader.OnLaunch();
			VARS = Vars.Instance;
			SM = ShortcutManager.Instance;
			DM = DeviceManager.Instance;
		}

		public static object AiLoggingService { get; private set; }

		private void MenuSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
			if (saveFileDialog.ShowDialog() == true)
			{
				Loader.SaveAllToFile(saveFileDialog.FileName);
			}
		}

		private void MenuOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
			{
				Loader.ReadAllFromFile(openFileDialog.FileName);
			}
		}

		private void MenuNew_Click(object sender, RoutedEventArgs e)
		{
			SM.Shortcuts.Clear();
			DM.Devices.Clear();
		}

		private void AddDevice_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			EditDeviceFlyout.IsOpen = false;
			SettingsFlyout.IsOpen = false;
			AddKeyFlyout.IsOpen = false;
			if (AddDeviceFlyout.IsOpen)
				AddDeviceFlyout.IsOpen = false;
			else
				AddDeviceFlyout.IsOpen = true;
		}

		private void AddShortcut_Click(object sender, RoutedEventArgs e)
		{
			AddDeviceFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			EditDeviceFlyout.IsOpen = false;
			SettingsFlyout.IsOpen = false;
			AddKeyFlyout.IsOpen = false;
			if (AddShortcutFlyout.IsOpen)
				AddShortcutFlyout.IsOpen = false;
			else
				AddShortcutFlyout.IsOpen = true;
		}

		private void EditShortcut_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
			AddDeviceFlyout.IsOpen = false;
			EditDeviceFlyout.IsOpen = false;
			SettingsFlyout.IsOpen = false;
			AddKeyFlyout.IsOpen = false;
			if (EditShortcutFlyout.IsOpen)
				EditShortcutFlyout.IsOpen = false;
			else
				EditShortcutFlyout.IsOpen = true;
		}

		private void Settings_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
			AddDeviceFlyout.IsOpen = false;
			EditDeviceFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			AddKeyFlyout.IsOpen = false;
			if (SettingsFlyout.IsOpen)
				SettingsFlyout.IsOpen = false;
			else
				SettingsFlyout.IsOpen = true;
		}

		private void EditDevice_Click(object sender, RoutedEventArgs e)
		{
			AddDeviceFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			AddShortcutFlyout.IsOpen = false;
			AddKeyFlyout.IsOpen = false;
			EditDeviceFlyout.IsOpen = true;
		}

		private void AddKey_Click(object sender, RoutedEventArgs e)
		{
			AddKeyFlyout.IsOpen = true;
		}

		private void KeyBox_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			if (e.Key == Key.Enter && IsNumpadEnterKey(e))
				VARS.Key = 1028;
			else if (e.Key == Key.System)
				VARS.Key = SM.KeyNum[e.SystemKey.ToString()];
			else VARS.Key = SM.KeyNum[e.Key.ToString()];
		}

		private void TargetBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.IsRepeat)
				return;
			e.Handled = true;
			if (VARS.Target != "")
			{
				if (e.Key == Key.Enter && IsNumpadEnterKey(e))
				{
					VARS.Target = $"{VARS.Target}+NumpadEnter";
					return;
				}
				if (e.Key == Key.System)
				{
					if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.F10)
					{
						VARS.Target = $"{VARS.Target}+{e.SystemKey}";
						return;
					}

				}
				VARS.Target = $"{VARS.Target}+{e.Key}";
			}
			else if (e.Key == Key.System)
				VARS.Target = $"{e.SystemKey}";
			else
			{
				if (e.Key == Key.Enter && IsNumpadEnterKey(e))
				{
					VARS.Target = $"NumpadEnter";
					return;
				}
				if (e.Key == Key.System)
					if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.F10)
					{
						VARS.Target = $"{e.SystemKey}";
						return;
					}
				VARS.Target = $"{e.Key}";
			}
		}

		private void StartInterception_Click(object sender, RoutedEventArgs e)
		{
			SM.StartInterception();
		}

		private void GetDevice_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		private void InterruptKeyBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			if (e.Key == Key.Enter && IsNumpadEnterKey(e))
				VARS.InterruptKey = $"NumpadEnter";
			else if (e.Key == Key.System)
				VARS.InterruptKey = $"{e.SystemKey}";
			else VARS.InterruptKey = $"{e.Key}";
		}

		private void ButtonDelayBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			var keyEnum = e.Key;
			if (!(keyEnum >= Key.D0 && keyEnum <= Key.D9 ||
				keyEnum >= Key.NumPad0 && keyEnum <= Key.NumPad9 ||
				keyEnum == Key.Back || keyEnum == Key.Delete ||
				keyEnum == Key.Left || keyEnum == Key.Right ||
				keyEnum == Key.Up || keyEnum == Key.Down))
				e.Handled = true;
			else if ((keyEnum == Key.Back || keyEnum == Key.Delete) && ButtonDelayBox.Text.Length == 1)
			{
				e.Handled = true;
				ButtonDelayBox.Text = "0";
			}
			else
			{
				e.Handled = ButtonDelayBox.Text.Length >= 10;
			}

		}

		private void KeyNameBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{

		}

		private void FlyoutAddShortcut_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
		}

		private void FlyoutAddDevice_Click(object sender, RoutedEventArgs e)
		{
			AddDeviceFlyout.IsOpen = false;
		}

		private void FlyoutEditShortcut_Click(object sender, RoutedEventArgs e)
		{
			EditShortcutFlyout.IsOpen = false;
		}

		private void FlyoutEditDevice_Click(object sender, RoutedEventArgs e)
		{
			EditDeviceFlyout.IsOpen = false;
		}

		private void FlyoutAddKey_Click(object sender, RoutedEventArgs e)
		{
			AddKeyFlyout.IsOpen = false;
		}

		private void FlyoutSettings_Click(object sender, RoutedEventArgs e)
		{
			SettingsFlyout.IsOpen = false;
		}

		private void FlyoutAddKey_Click_1(object sender, RoutedEventArgs e)
		{
			AddKeyFlyout.IsOpen = false;
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Loader.OnExit();
		}

		private static bool IsNumpadEnterKey(KeyEventArgs e)
		{
			if (e.Key != Key.Enter)
				return false;

			// To understand the following UGLY implementation please check this MSDN link. Suggested workaround to differentiate between the Return key and Enter key.
			// https://social.msdn.microsoft.com/Forums/vstudio/en-US/b59e38f1-38a1-4da9-97ab-c9a648e60af5/whats-the-difference-between-keyenter-and-keyreturn?forum=wpf
			try
			{
				return (bool)typeof(KeyEventArgs).InvokeMember("IsExtendedKey", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, e, null);
			}
			catch
			{
				throw;
			}
		}

	}
}

