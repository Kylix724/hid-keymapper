using InterceptionKeymapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Helpers
{
    class ShortcutDummy : ObservableModelBase
    {
        private Device _device = null;
        private string _key = "";
        private string _target = "";

        private static ShortcutDummy _instance;
        public static ShortcutDummy Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ShortcutDummy();
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

        public override string ToString()
        {
            return $"{Device} {Key}{Target}";
        }

        public void flush(){
            Device = null;
            Key = "";
            Target = "";
        }
    }
}
