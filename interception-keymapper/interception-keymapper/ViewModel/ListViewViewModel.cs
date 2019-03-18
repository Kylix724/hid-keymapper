using InterceptionKeymapper.Helpers;
using InterceptionKeymapper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterceptionKeymapper.ViewModel
{
	class ListViewViewModel : ObservableModelBase
	{
		TaskFactory taskFactory;
		private ObservableCollection<Shortcut> _shortcuts = new ObservableCollection<Shortcut>();
		private ObservableCollection<Device> _devices = new ObservableCollection<Device>();
		public ObservableCollection<Shortcut> Shortcuts { get { return _shortcuts; } }
		public ObservableCollection<Device> Devices { get { return _devices; } }

		public ListViewViewModel()
		{
			taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
			ShortcutManager.Instance.Shortcuts.CollectionChanged += ShortcutsCollectionChanged;
			DeviceManager.Instance.Devices.CollectionChanged += DevicesCollectionChanged;
			ShortcutDummy.Instance.PropertyChanged += SetShortcut;
			NewDeviceHwid = "Get Device";
		}

		#region Variables
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
				Console.WriteLine(value);
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
		#endregion

		#region Methods
		public void SetShortcut(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "DummyKey")
			{
				ShortcutKey = ShortcutDummy.Instance.Key;
			}
			else if (e.PropertyName == "DummyTarget")
			{
				ShortcutTarget = ShortcutDummy.Instance.Target;
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
						ShortcutKey = ShortcutDummy.Instance.Key;
						ShortcutTarget = ShortcutDummy.Instance.Target;
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
						ShortcutDevice = SelectedShortcut.Device;
						ShortcutDummy.Instance.Key = SelectedShortcut.Key;
						ShortcutDummy.Instance.Target = SelectedShortcut.Target;
						ShortcutKey = SelectedShortcut.Key;
						ShortcutTarget = SelectedShortcut.Target;
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
						Shortcuts.Remove(SelectedShortcut);
						ShortcutManager.Instance.AddShortcut(ShortcutDevice);
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
					_clearShortcutKey = new ActionCommand(e => { ShortcutDummy.Instance.Key = ""; });
				return _clearShortcutKey;
			}
		}

		private ICommand _clearShortcutTarget;
		public ICommand ClearShortcutTarget
		{
			get
			{
				if (_clearShortcutTarget == null)
					_clearShortcutTarget = new ActionCommand(e => { ShortcutDummy.Instance.Target = ""; });
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
						foreach (Device item in e.NewItems)
						{
							Devices.Remove(item);
						}
						break;
					case NotifyCollectionChangedAction.Reset:
						Devices.Clear();
						break;
					default:
						throw new System.ArgumentException("Unbehandelter DevicesCollectionChanged:" + e.Action.ToString());
				}
			});
		}
		#endregion

	}
}
