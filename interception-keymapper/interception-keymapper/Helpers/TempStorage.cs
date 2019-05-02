using InterceptionKeymapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Helpers
{
    class TempStorage : ObservableModelBase
    {
        private Device _device = null;
        private string _key = "";
        private string _target = "";
		private string _interruptKey = "Escape";
		private int _buttonDelay = 15;
		private string _fileLocation = null;

        private static TempStorage _instance;
        public static TempStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TempStorage();
                }

                return _instance;
            }
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
        public string Key
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

        public void FlushShortcut(){
            Device = null;
            Key = "";
            Target = "";
        }

		public void ResetSettings()
		{
			ButtonDelay = 15;
			InterruptKey = "Escape";
		}
    }
}
