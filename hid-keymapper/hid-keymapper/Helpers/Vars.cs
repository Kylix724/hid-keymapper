using HidKeymapper.Model;
using System;
using System.Collections.Generic;

namespace HidKeymapper.Helpers
{
	class Vars : ObservableModelBase
	{
		private Device _device = null;
		private ushort _key = 0;
		private string _target = "";
		private string _interruptKey = "Escape";
		private int _buttonDelay = 15;
		private string _fileLocation = null;
		private string _newKeyName = "";
		private ushort _newKeyId = 0;
		private ShortcutManager SM;

		private static Vars _instance;
		public static Vars Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Vars();
				}
				return _instance;
			}
		}

		private Vars()
		{
			SM = ShortcutManager.Instance;
		}

		public Device Device
		{
			get => _device;
			set
			{
				_device = value;
				OnPropertyChanged("DummyDevice");
			}
		}
		public ushort Key
		{
			get => _key;
			set
			{
				_key = value;
				OnPropertyChanged("DummyKey");
			}
		}
		public string Target
		{
			get => _target;
			set
			{
				_target = value;
				_targetList = new List<ushort>();
				foreach (var x in value.Split('+'))
				{
					_targetList.Add(SM.KeyNum[x]);
				}
				OnPropertyChanged("DummyTarget");
				OnPropertyChanged("TargetList");
			}
		}

		private List<ushort> _targetList;
		public List<ushort> TargetList
		{
			get => _targetList;
			private set
			{
				_targetList = value;
				_target = String.Join("+", _targetList.ToArray());
				OnPropertyChanged("TargetList");
				OnPropertyChanged("DummyTarget");
			}
		}
		public string InterruptKey
		{
			get => _interruptKey;
			set
			{
				_interruptKey = value;
				OnPropertyChanged("InterruptKey");
			}
		}
		public int ButtonDelay
		{
			get => _buttonDelay;
			set
			{
				_buttonDelay = value;
				OnPropertyChanged("ButtonDelay");
			}
		}
		public string FileLocation
		{
			get => _fileLocation;
			set
			{
				_fileLocation = value;
				OnPropertyChanged("FileLocation");
			}
		}
		public string NewKeyName
		{
			get => _newKeyName;
			set
			{
				_newKeyName = value;
				OnPropertyChanged("NewKeyName");
			}
		}
		public ushort NewKeyId
		{
			get => _newKeyId;
			set
			{
				_newKeyId = value;
				OnPropertyChanged("NewKeyId");
			}
		}


		public void GetNewKeyNum()
		{
			NewKeyId = InterceptionManager.get_key();
			Console.WriteLine(NewKeyId);
		}
		public void FlushShortcut()
		{
			Device = null;
			Key = 0;
			Target = "";
		}
		public void ResetSettings()
		{
			ButtonDelay = 15;
			InterruptKey = "Escape";
		}
	}
}
