using InterceptionKeymapper.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Model
{
    public class Device : ObservableModelBase
    {
        private string _hwid;
        private string _name;

        public Device(string hwid, string name){
            Hwid = hwid;
            Name = name;
        }

        public string Hwid
        {
            get => _hwid;
            set
            {
                _hwid = value;
                OnPropertyChanged("Hwid");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public override string ToString()
        {
            return $"{Name}({Hwid})";
        }
    }
}
