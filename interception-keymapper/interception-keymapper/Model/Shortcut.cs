using InterceptionKeymapper.Helpers;
using System.Collections.Generic;

namespace InterceptionKeymapper.Model
{
	public class Shortcut : ObservableModelBase
	{
		private string _device;
		private ushort _keyId;
		//private string _target;
		private List<ushort> _target;
		public Shortcut(string device, ushort keyId, List<ushort> target)
		{
			Device = device;
			KeyId = keyId;
			Target = target;
		}

		public Shortcut(Device device, ushort keyId, List<ushort> target)
		{
			Device = device.Name;
			KeyId = keyId;
			Target = target;
		}

		public string Device
		{
			get => _device;
			set
			{
				_device = value;
				OnPropertyChanged("Device");
			}
		}
		public ushort KeyId
		{
			get => _keyId;
			set
			{
				_keyId = value;
				Key = ShortcutManager.Instance.KeyNumReverse[value];
				OnPropertyChanged("KeyNumber");
			}
		}

		private string _keyString;
		public string Key
		{
			get => _keyString;
			set
			{
				_keyString = value;
				OnPropertyChanged("KeyId");
			}
		}
		public List<ushort> Target
		{
			get => _target;
			set
			{
				_target = value;
				TargetString = "";
				foreach (var x in _target)
				{
					if (TargetString != "")
						TargetString = $"{TargetString}+{ShortcutManager.Instance.KeyNumReverse[x]}";
					else TargetString = ShortcutManager.Instance.KeyNumReverse[x];
				}
				OnPropertyChanged("TargetList");
			}
		}

		private string _targetString;
		public string TargetString
		{
			get => _targetString;
			set
			{
				_targetString = value;
				OnPropertyChanged("Target");
			}
		}

		public override string ToString()
		{
			return $"Shortcut: {Device}, {KeyId}, {TargetString}";
		}
	}
}
