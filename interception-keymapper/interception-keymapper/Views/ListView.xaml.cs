using InterceptionKeymapper.Helpers;
using InterceptionKeymapper.Model;
using InterceptionKeymapper.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public string ShortcutTarget { get; set; } = "";
		public string ShortcutKey { get; set; } = "";
		public ListView()
		{
			InitializeComponent();
		}

		private void MenuSave_Click(object sender, RoutedEventArgs e)
		{

		}

		private void MenuOpen_Click(object sender, RoutedEventArgs e)
		{

		}

		private void MenuNew_Click(object sender, RoutedEventArgs e)
		{

		}

		private void MenuAddDevice_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			if (AddDeviceFlyout.IsOpen)
				AddDeviceFlyout.IsOpen = false;
			else
				AddDeviceFlyout.IsOpen = true;
		}

		private void MenuShowDevices_Click(object sender, RoutedEventArgs e)
		{
			new ShowDevicesView();
		}

		private void MenuTestDevice_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AddShortcut_Click(object sender, RoutedEventArgs e)
		{
			AddDeviceFlyout.IsOpen = false;
			EditShortcutFlyout.IsOpen = false;
			if (AddShortcutFlyout.IsOpen)
				AddShortcutFlyout.IsOpen = false;
			else
				AddShortcutFlyout.IsOpen = true;
		}

		private void RemoveShortcut_Click(object sender, RoutedEventArgs e)
		{

		}

		private void EditShortcut_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
			AddDeviceFlyout.IsOpen = false;
			if (EditShortcutFlyout.IsOpen)
				EditShortcutFlyout.IsOpen = false;
			else
				EditShortcutFlyout.IsOpen = true;
		}

		private void FlyoutAddShortcut_Click(object sender, RoutedEventArgs e)
		{
			AddShortcutFlyout.IsOpen = false;
		}

		private void TargetBox_Click(object sender, RoutedEventArgs e)
		{

		}

		private void KeyBox_Click(object sender, RoutedEventArgs e)
		{

		}

		private void KeyBox_KeyDown(object sender, KeyEventArgs e)
		{
			ShortcutDummy.Instance.Key = $"{e.Key}";
		}

		private void TargetBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (ShortcutDummy.Instance.Target != "")
				ShortcutDummy.Instance.Target = $"{ShortcutDummy.Instance.Target}+{e.Key}";
			else
				ShortcutDummy.Instance.Target = $"{e.Key}";
		}

		private void FlyoutAddDevice_Click(object sender, RoutedEventArgs e)
		{
			AddDeviceFlyout.IsOpen = false;
		}

		private void StartInterception_Click(object sender, RoutedEventArgs e)
		{
			ShortcutManager.Instance.StartInterception();
		}

		private void FlyoutEditShortcut_Click(object sender, RoutedEventArgs e)
		{

		}

		private void EditDevice_Click(object sender, RoutedEventArgs e)
		{

		}

		private void RemoveDevice_Click(object sender, RoutedEventArgs e)
		{

		}

	}
}
