using InterceptionKeymapper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Model
{
    public class ShortcutManager : Helpers.LazySingleton<ShortcutManager>
    {
        private ObservableCollection<Shortcut> _shortcuts = new ObservableCollection<Shortcut>();
        public ObservableCollection<Shortcut> Shortcuts => _shortcuts;
		private Thread t;

		public void AddShortcut(Device device, string key, string target)
        {
            List<ushort> vals = new List<ushort>();
            foreach (var t in target.Split('+'))
            {
                vals.Add(KeyToShort(t));
            }

            //InterceptionManager.add_pair(Marshal.StringToHGlobalAuto(device.Hwid), KeyToShort(key), vals.ToArray());
            Shortcuts.Add(new Shortcut(device, key, target));
        }

        public void AddShortcut(Device device)
        {
            if (device != null && TempStorage.Instance.Key != "" && TempStorage.Instance.Target.Count() != 0)
            {
                List<ushort> vals = new List<ushort>();
                foreach (var t in TempStorage.Instance.Target.Split('+'))
                {
                    vals.Add(KeyToShort(t));
                }
                //InterceptionManager.add_pair(Marshal.StringToHGlobalAuto(device.Hwid), KeyToShort(ShortcutDummy.Instance.Key), vals.ToArray());
                Shortcuts.Add(new Shortcut(device, TempStorage.Instance.Key, TempStorage.Instance.Target));
                TempStorage.Instance.FlushShortcut();
            }
        }

		public void StartInterception(){
			if (t==null || (t != null && !t.IsAlive))
			{
				t = new Thread(new ThreadStart(Intercept));
				t.Start();
			}
		}

        private void Intercept()
        {
            List<IntPtr> hwid = new List<IntPtr>();
            List<int> key = new List<int>();
            List<ushort> vals = new List<ushort>();
			foreach (var x in Shortcuts)
			{
				if (!DeviceManager.Instance.DevicesByName[x.Device].Active)
					continue;
				List<ushort> temp = new List<ushort>();
				foreach (var t in x.Target.Split('+'))
					temp.Add(KeyToShort(t));
				if (temp.Contains(0))
					continue;
				int i = 0;
                foreach (var t in x.Target.Split('+'))
                {
					vals.Add(KeyToShort(t));
                    i++;
                }
                while (i < 16)
                {
                    vals.Add(0);
                    i++;
                }
                
                hwid.Add(Marshal.StringToBSTR(DeviceManager.Instance.DevicesByName[x.Device].Hwid));
                key.Add(KeyToShort(x.Key));
            }

			InterceptionManager.start_interception(hwid.ToArray(), key.ToArray(), vals.ToArray(), hwid.Count, TempStorage.Instance.ButtonDelay, KeyToShort(TempStorage.Instance.InterruptKey));
        }

        public ushort KeyToShort(string key)
        {
			if (Helpers.KeyHelper.KeyNum.ContainsKey(key.ToUpper()))
				return Helpers.KeyHelper.KeyNum[key.ToUpper()];
			else
				return 0;
        }		
	}
}
