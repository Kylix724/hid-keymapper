using InterceptionKeymapper.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Model
{
    public class Shortcut : ObservableModelBase
    {
        private string _device;
        private string _key;
        private string _target;

        public Shortcut(string device, string key, string target)
        {
            Device = device;
            Key = key;
            Target = target;
        }

		public Shortcut(Device device, string key, string target)
		{
			Device = device.Name;
			Key = key;
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
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }
        public string Target
        {
            get => _target;
            set
            {
                _target = value;
                OnPropertyChanged("Target");
            }
        }

        public override string ToString()
        {
            return $"{Device} {Key} {Target}";
        }
    }
}
