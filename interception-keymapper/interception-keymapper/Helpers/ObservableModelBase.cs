
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Helpers{ 
    /*
     * Standard-Basisklasse für ViewModels
     * mit Boilerplate-Code für Property-Änderungen
     * 
     */
    public class ObservableModelBase : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}