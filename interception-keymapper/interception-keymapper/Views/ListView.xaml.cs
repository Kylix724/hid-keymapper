using InterceptionKeymapper.Helpers;
using InterceptionKeymapper.Model;
using InterceptionKeymapper.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterceptionKeymapper.Views
{
	/// <summary>
	/// Interaction logic for ListView.xaml
	/// </summary>
	public partial class ListView : UserControl
	{
		public static object AiLoggingService { get; private set; }

		public ListView()
		{
			InitializeComponent();
		}

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

		private void MenuAddDevice_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			EditDeviceFlyout.IsOpen = false;
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
			if (EditShortcutFlyout.IsOpen)
				EditShortcutFlyout.IsOpen = false;
			else
				EditShortcutFlyout.IsOpen = true;
		}

		private void FlyoutAddShortcut_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
		}

		private void KeyBox_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			if (e.Key == Key.Enter && IsNumpadEnterKey(e))
				ShortcutDummy.Instance.Key = $"NUMPADENTER";
			else if (e.Key == Key.System)
				ShortcutDummy.Instance.Key = $"{e.SystemKey}";
			else ShortcutDummy.Instance.Key = $"{e.Key}";
		}

		private void TargetBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.IsRepeat)
				return;
			e.Handled = true;
			if (ShortcutDummy.Instance.Target != "")
			{
				if (e.Key == Key.Enter && IsNumpadEnterKey(e))
				{
					ShortcutDummy.Instance.Target = $"{ShortcutDummy.Instance.Target}+NUMPADENTER";
					return;
				}
				if (e.Key == Key.System)
				{
					if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.F10)
					{
						ShortcutDummy.Instance.Target = $"{ShortcutDummy.Instance.Target}+{e.SystemKey}";
						return;
					}
					
				}
				ShortcutDummy.Instance.Target = $"{ShortcutDummy.Instance.Target}+{e.Key}";
			}
			else if (e.Key == Key.System)
				ShortcutDummy.Instance.Target = $"{e.SystemKey}";
			else
			{
				if (e.Key == Key.Enter && IsNumpadEnterKey(e))
				{
					ShortcutDummy.Instance.Target = $"NUMPADENTER";
					return;
				}
				if (e.Key == Key.System)
					if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.F10)
					{
						ShortcutDummy.Instance.Target = $"{e.SystemKey}";
						return;
					}
				ShortcutDummy.Instance.Target = $"{e.Key}";
			}
		}

		private void FlyoutAddDevice_Click(object sender, RoutedEventArgs e)
		{
			AddDeviceFlyout.IsOpen = false;
		}

		private void StartInterception_Click(object sender, RoutedEventArgs e)
		{
			ShortcutManager.Instance.StartInterception();
		}

		private void EditDevice_Click(object sender, RoutedEventArgs e)
		{
			AddDeviceFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			AddShortcutFlyout.IsOpen = false;
			EditDeviceFlyout.IsOpen = true;
		}

		private void GetDevice_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
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
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
