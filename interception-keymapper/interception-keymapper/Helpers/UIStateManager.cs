using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Helpers
{
    public class UiChangedEventArgs : EventArgs
    {
        public readonly string StateName;

        public UiChangedEventArgs(string Statename)
        {
            this.StateName = Statename;
        }
    }
    public class UIStateManager : LazySingleton<UIStateManager>
    {
        public event EventHandler<UiChangedEventArgs> OnUiStateChanged;

        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    Console.WriteLine($"{0}: {1} changed to {2}", this.GetType(), _state, value);
                    _state = value;
                    OnUiStateChanged(this, new UiChangedEventArgs(_state));
                }
            }
        }

        public UIStateManager() { }

    }
}
