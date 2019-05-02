using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using InterceptionKeymapper.Helpers;
using InterceptionKeymapper.Model;
using System.ComponentModel;

namespace InterceptionKeymapper.ViewModel
{
    public class ApplicationObservableModel : ObservableModelBase
    {
		TaskFactory taskFactory;
		private ObservableCollection<Shortcut> _shortcuts = new ObservableCollection<Shortcut>();
		private ObservableCollection<Device> _devices = new ObservableCollection<Device>();
		public ObservableCollection<Shortcut> Shortcuts { get { return _shortcuts; } }
		public ObservableCollection<Device> Devices { get { return _devices; } }



		public ApplicationObservableModel()
		{
			taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
			ShortcutManager.Instance.Shortcuts.CollectionChanged += ShortcutsCollectionChanged;
			DeviceManager.Instance.Devices.CollectionChanged += DevicesCollectionChanged;
			TempStorage.Instance.PropertyChanged += SetShortcut;
			NewDeviceHwid = "Get Device";
			Title = "Interception Keymapper";
		}

		#region Binding Variables
		private string _title;
		public string Title
		{
			get => _title;
			set
			{
				_title = "InterceptionKeymapper - " + value;
				OnPropertyChanged("Title");
			}
		}

		private Device _shortcutDevice;
		public Device ShortcutDevice
		{
			get => _shortcutDevice;
			set
			{
				_shortcutDevice = value;
				OnPropertyChanged("ShortcutDevice");
			}
		}

		private string _shortcutKey;
		public string ShortcutKey
		{
			get => _shortcutKey;
			set
			{
				_shortcutKey = value;
				OnPropertyChanged("ShortcutKey");
			}
		}

		private string _shortcutTarget;
		public string ShortcutTarget
		{
			get => _shortcutTarget;
			set
			{
				_shortcutTarget = value;
				OnPropertyChanged("ShortcutTarget");
			}
		}

		private string _newDeviceName;
		public string NewDeviceName
		{
			get => _newDeviceName;
			set
			{
				_newDeviceName = value;
				OnPropertyChanged("NewDeviceName");
			}
		}

		private string _newDeviceHwid;
		public string NewDeviceHwid
		{
			get => _newDeviceHwid;
			set
			{
				_newDeviceHwid = value;
				OnPropertyChanged("NewDeviceHwid");
			}
		}

		private Shortcut _selectedShortcut;
		public Shortcut SelectedShortcut
		{
			get => _selectedShortcut;
			set
			{
				_selectedShortcut = value;
				OnPropertyChanged("SelectedShortcut");
			}
		}

		private Device _selectedDevice;
		public Device SelectedDevice
		{
			get => _selectedDevice;
			set
			{
				_selectedDevice = value;
				OnPropertyChanged("SelectedDevice");
				Console.WriteLine(value);
			}
		}

		private Device _editDevice;
		private Shortcut _editShortcut;

		private string _interruptKey = "Escape";
		public string InterruptKey
		{
			get => _interruptKey;
			set
			{
				_interruptKey = value;
				OnPropertyChanged("InterruptKey");
			}
		}

		private int _buttonDelay = 15;
		public int ButtonDelay
		{
			get => _buttonDelay;
			set
			{
				int x;
				_buttonDelay = int.TryParse(value.ToString(), out x) ? value : 0;
				OnPropertyChanged("ButtonDelay");
			}
		}
		#endregion

		#region Methods
		public void SetShortcut(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "DummyKey")
			{
				ShortcutKey = TempStorage.Instance.Key;
			}
			else if (e.PropertyName == "DummyTarget")
			{
				ShortcutTarget = TempStorage.Instance.Target;
			}
			else if (e.PropertyName == "Title")
			{
				Title = TempStorage.Instance.FileLocation.Split('\\').Last();
			}
		}
		#endregion

		#region Commands
		private ICommand _addShortcutCommand;
		public ICommand AddShortcutCommand
		{
			get
			{
				if (_addShortcutCommand == null)
				{
					_addShortcutCommand = new ActionCommand(e =>
					{
						ShortcutManager.Instance.AddShortcut(ShortcutDevice);
					});
				}
				return _addShortcutCommand;
			}
		}

		private ICommand _getDeviceCommand;
		public ICommand GetDeviceCommand
		{
			get
			{
				if (_getDeviceCommand == null)
				{
					_getDeviceCommand = new ActionCommand(e =>
					{
						NewDeviceHwid = DeviceManager.Instance.GetDevice();
					});
				}
				return _getDeviceCommand;
			}
		}

		private ICommand _addDeviceCommand;
		public ICommand AddDeviceCommand
		{
			get
			{
				if (_addDeviceCommand == null)
				{
					_addDeviceCommand = new ActionCommand(e =>
					{
						DeviceManager.Instance.Devices.Add(new Device(NewDeviceHwid, NewDeviceName));
					});
				}
				return _addDeviceCommand;
			}
		}

		private ICommand _shortcutButtonLostFocus;
		public ICommand ShortcutButtonLostFocus
		{
			get
			{
				if (_shortcutButtonLostFocus == null)
				{
					_shortcutButtonLostFocus = new ActionCommand(e =>
					{
						ShortcutKey = TempStorage.Instance.Key;
						ShortcutTarget = TempStorage.Instance.Target;
					});
				}
				return _shortcutButtonLostFocus;
			}
		}

		private ICommand _editShortcutCommand;
		public ICommand EditShortcutCommand
		{
			get
			{
				if (_editShortcutCommand == null)
				{
					_editShortcutCommand = new ActionCommand(e =>
					{
						ShortcutDevice = DeviceManager.Instance.DevicesByName[SelectedShortcut.Device];
						TempStorage.Instance.Key = SelectedShortcut.Key;
						TempStorage.Instance.Target = SelectedShortcut.Target;
						ShortcutKey = SelectedShortcut.Key;
						ShortcutTarget = SelectedShortcut.Target;
						_editShortcut = SelectedShortcut;
					});
				}
				return _editShortcutCommand;
			}
		}

		private ICommand _editShortcutFlyoutCommand;
		public ICommand EditShortcutFlyoutCommand
		{
			get
			{
				if (_editShortcutFlyoutCommand == null)
				{
					_editShortcutFlyoutCommand = new ActionCommand(e =>
					{
						Shortcuts.Remove(_editShortcut);
						ShortcutManager.Instance.Shortcuts[ShortcutManager.Instance.Shortcuts.IndexOf(_editShortcut)]
						= new Shortcut(ShortcutDevice, TempStorage.Instance.Key, TempStorage.Instance.Target);
					});
				}
				return _editShortcutFlyoutCommand;
			}
		}

		private ICommand _clearShortcutKey;
		public ICommand ClearShortcutKey
		{
			get
			{
				if (_clearShortcutKey == null)
					_clearShortcutKey = new ActionCommand(e => { TempStorage.Instance.Key = ""; });
				return _clearShortcutKey;
			}
		}

		private ICommand _clearShortcutTarget;
		public ICommand ClearShortcutTarget
		{
			get
			{
				if (_clearShortcutTarget == null)
					_clearShortcutTarget = new ActionCommand(e => { TempStorage.Instance.Target = ""; });
				return _clearShortcutTarget;
			}
		}

		private ICommand _removeShortcutCommand;
		public ICommand RemoveShortcutCommand
		{
			get
			{
				if (_removeShortcutCommand == null)
					_removeShortcutCommand = new ActionCommand(e => { ShortcutManager.Instance.Shortcuts.Remove(SelectedShortcut); });
				return _removeShortcutCommand;
			}
		}

		private ICommand _editDeviceCommand;
		public ICommand EditDeviceCommand
		{
			get
			{
				if (_editDeviceCommand == null)
					_editDeviceCommand = new ActionCommand(e =>
					{
						NewDeviceName = SelectedDevice.Name;
						NewDeviceHwid = SelectedDevice.Hwid;
						_editDevice = SelectedDevice;
					});
				return _editDeviceCommand;
			}
		}

		private ICommand _removeDeviceCommand;
		public ICommand RemoveDeviceCommand
		{
			get
			{
				if (_removeDeviceCommand == null)
					_removeDeviceCommand = new ActionCommand(e =>
					{
						DeviceManager.Instance.Devices.Remove(SelectedDevice);
					});
				return _removeDeviceCommand;
			}
		}

		private ICommand _editDeviceFlyoutCommand;
		public ICommand EditDeviceFlyoutCommand
		{
			get
			{
				if (_editDeviceFlyoutCommand == null)
					_editDeviceFlyoutCommand = new ActionCommand(e =>
					{
						DeviceManager.Instance.Devices[DeviceManager.Instance.Devices.IndexOf(_editDevice)] = new Device(NewDeviceHwid, NewDeviceName);
					});
				return _editDeviceFlyoutCommand;
			}
		}

		private ICommand _settingsFlyoutCommand;
		public ICommand SettingsFlyoutCommand
		{
			get
			{
				if (_settingsFlyoutCommand == null)
					_settingsFlyoutCommand = new ActionCommand(e =>
					{
						TempStorage.Instance.ButtonDelay = _buttonDelay;
						TempStorage.Instance.InterruptKey = _interruptKey;
					});
				return _settingsFlyoutCommand;
			}
		}

		private ICommand _settingsCommand;
		public ICommand SettingsCommand
		{
			get
			{
				if (_settingsCommand == null)
					_settingsCommand = new ActionCommand(e =>
					{
						ButtonDelay = TempStorage.Instance.ButtonDelay;
						InterruptKey = TempStorage.Instance.InterruptKey;
					});
				return _settingsCommand;
			}
		}

		private ICommand _clearInterruptKeyCommand;
		public ICommand ClearInterruptKeyCommand
		{
			get
			{
				if (_clearInterruptKeyCommand == null)
					_clearInterruptKeyCommand = new ActionCommand(e => { InterruptKey = ""; });
				return _clearInterruptKeyCommand;
			}
		}
		#endregion

		#region Event Handlers
		public void ShortcutsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			taskFactory.StartNew(() =>
			{
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						foreach (Shortcut item in e.NewItems)
						{
							Shortcuts.Add(item);
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						foreach (Shortcut item in e.OldItems)
						{
							Shortcuts.Remove(item);
						}
						break;
					case NotifyCollectionChangedAction.Reset:
						Shortcuts.Clear();
						break;
					case NotifyCollectionChangedAction.Replace:
						int i = 0;
						while (i < e.OldItems.Count)
						{
							Shortcuts[Shortcuts.IndexOf((Shortcut)e.OldItems[i])] = (Shortcut)e.NewItems[i];
							i++;
						}
						break;
					default:
						throw new System.ArgumentException("Unbehandelter ShortcutsCollectionChanged:" + e.Action.ToString());

				}
			});
		}

		public void DevicesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			taskFactory.StartNew(() =>
			{
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						foreach (Device item in e.NewItems)
						{
							Devices.Add(item);
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						foreach (Device item in e.OldItems)
						{
							Devices.Remove(item);
						}
						break;
					case NotifyCollectionChangedAction.Reset:
						Devices.Clear();
						break;
					case NotifyCollectionChangedAction.Replace:
						int i = 0;
						while (i < e.OldItems.Count)
						{
							Devices[Devices.IndexOf((Device)e.OldItems[i])] = (Device)e.NewItems[i];
							i++;
						}
						break;

					default:
						throw new System.ArgumentException("Unbehandelter DevicesCollectionChanged:" + e.Action.ToString());
				}
			});
		}
		#endregion



	}
}



