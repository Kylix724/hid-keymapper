using HidKeymapper.Helpers;
using HidKeymapper.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HidKeymapper.ViewModel
{
	public class ApplicationObservableModel : ObservableModelBase
	{
		private Vars VARS;
		private ShortcutManager SM;
		private DeviceManager DM;
		TaskFactory taskFactory;
		private ObservableCollection<Shortcut> _shortcuts = new ObservableCollection<Shortcut>();
		private ObservableCollection<Device> _devices = new ObservableCollection<Device>();
		public ObservableCollection<Shortcut> Shortcuts { get { return _shortcuts; } }
		public ObservableCollection<Device> Devices { get { return _devices; } }



		public ApplicationObservableModel()
		{
			VARS = Vars.Instance;
			SM = ShortcutManager.Instance;
			DM = DeviceManager.Instance;

			taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
			SM.Shortcuts.CollectionChanged += ShortcutsCollectionChanged;
			DM.Devices.CollectionChanged += DevicesCollectionChanged;
			NewDeviceHwid = "Get Device";
			NewKeyId = "Get Key Id";
			Title = null;
			VARS.PropertyChanged += PropertyChangedHandler;
		}

		#region Binding Variables
		private string _title;
		public string Title
		{
			get => _title;
			set
			{
				if (value != null)
					_title = $"{value} - Interception Keymapper";
				else
					_title = "Interception Keymapper";
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
				_buttonDelay = int.TryParse(value.ToString(), out _) ? value : 0;
				OnPropertyChanged("ButtonDelay");
			}
		}

		private ushort _newKeyId = 0;
		public string NewKeyId
		{
			get => _newKeyId.ToString();
			set
			{
				if (ushort.TryParse(value.ToString(), out ushort x))
					_newKeyId = x;
				else
					_newKeyId = 0;
				OnPropertyChanged("NewKeyId");
			}
		}

		private string _newKeyName;
		public string NewKeyName
		{
			get => _newKeyName;
			set
			{
				_newKeyName = value;
				OnPropertyChanged("NewKeyName");
			}
		}
		#endregion

		#region Methods
		public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "DummyKey")
				try
				{
					ShortcutKey = SM.KeyNumReverse[VARS.Key];
				}
				catch { }
			else if (e.PropertyName == "DummyTarget")
				ShortcutTarget = VARS.Target;
			else if (e.PropertyName == "FileLocation")
				Title = VARS.FileLocation.Split('\\').Last();
			else if (e.PropertyName == "InterruptKey")
				InterruptKey = VARS.InterruptKey;
			else if (e.PropertyName == "NewKeyId")
				NewKeyId = VARS.NewKeyId.ToString();
			else if (e.PropertyName == "NewKeyName")
				NewKeyName = VARS.NewKeyName;

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
						SM.AddShortcut(ShortcutDevice);
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
						NewDeviceHwid = DM.GetDevice();
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
						DM.Devices.Add(new Device(NewDeviceHwid, NewDeviceName));
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
						ShortcutKey = SM.KeyNumReverse[VARS.Key];
						ShortcutTarget = VARS.Target;
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
						if (SelectedShortcut != null)
						{
							ShortcutDevice = DM.DevicesByName[SelectedShortcut.Device];
							VARS.Key = SelectedShortcut.KeyId;
							VARS.Target = SelectedShortcut.TargetString;
							ShortcutKey = SM.KeyNumReverse[SelectedShortcut.KeyId];
							ShortcutTarget = SelectedShortcut.TargetString;
							_editShortcut = SelectedShortcut;
						}
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
						if (SM.Shortcuts.Contains(_editShortcut))
							SM.Shortcuts[SM.Shortcuts.IndexOf(_editShortcut)]
							= new Shortcut(ShortcutDevice, VARS.Key, VARS.TargetList);
						else
							SM.Shortcuts.Add(new Shortcut(ShortcutDevice, VARS.Key, VARS.TargetList));
					});
				}
				return _editShortcutFlyoutCommand;
			}
		}

		private ICommand _addKeyFlyoutCommand;
		public ICommand AddKeyFlyoutCommand
		{
			get
			{
				if (_addKeyFlyoutCommand == null)
					_addKeyFlyoutCommand = new ActionCommand(e => SM.KeyNum[VARS.NewKeyName] = VARS.NewKeyId);
				return _addKeyFlyoutCommand;
			}
		}

		private ICommand _getKeyNumCommand;
		public ICommand GetKeyNumCommand
		{
			get
			{
				if (_getKeyNumCommand == null)
					_getKeyNumCommand = new ActionCommand(e => VARS.GetNewKeyNum());
				return _getKeyNumCommand;
			}
		}

		private ICommand _clearShortcutKey;
		public ICommand ClearShortcutKey
		{
			get
			{
				if (_clearShortcutKey == null)
					_clearShortcutKey = new ActionCommand(e => { VARS.Key = 0; });
				return _clearShortcutKey;
			}
		}

		private ICommand _clearShortcutTarget;
		public ICommand ClearShortcutTarget
		{
			get
			{
				if (_clearShortcutTarget == null)
					_clearShortcutTarget = new ActionCommand(e => { VARS.Target = ""; });
				return _clearShortcutTarget;
			}
		}

		private ICommand _removeShortcutCommand;
		public ICommand RemoveShortcutCommand
		{
			get
			{
				if (_removeShortcutCommand == null)
					_removeShortcutCommand = new ActionCommand(e => { SM.Shortcuts.Remove(SelectedShortcut); });
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
						if (SelectedDevice != null)
						{
							NewDeviceName = SelectedDevice.Name;
							NewDeviceHwid = SelectedDevice.Hwid;
							_editDevice = SelectedDevice;
						}
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
						DM.Devices.Remove(SelectedDevice);
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
						if (DM.Devices.Contains(_editDevice))
							DM.Devices[DM.Devices.IndexOf(_editDevice)] = new Device(NewDeviceHwid, NewDeviceName);
						else
							DM.Devices.Add(new Device(NewDeviceHwid, NewDeviceName));
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
						VARS.ButtonDelay = _buttonDelay;
						VARS.InterruptKey = _interruptKey;
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
						ButtonDelay = VARS.ButtonDelay;
						InterruptKey = VARS.InterruptKey;
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
					_clearInterruptKeyCommand = new ActionCommand(e => { VARS.InterruptKey = ""; });
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



