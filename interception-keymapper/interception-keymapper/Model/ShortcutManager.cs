﻿using InterceptionKeymapper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Model
{
    public class ShortcutManager : Helpers.LazySingleton<ShortcutManager>
    {
        private ObservableCollection<Shortcut> _shortcuts = new ObservableCollection<Shortcut>();
        public ObservableCollection<Shortcut> Shortcuts => _shortcuts;
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
            if (device != null && ShortcutDummy.Instance.Key != "" && ShortcutDummy.Instance.Target.Count() != 0)
            {
                List<ushort> vals = new List<ushort>();
                foreach (var t in ShortcutDummy.Instance.Target.Split('+'))
                {
                    vals.Add(KeyToShort(t));
                }
                //InterceptionManager.add_pair(Marshal.StringToHGlobalAuto(device.Hwid), KeyToShort(ShortcutDummy.Instance.Key), vals.ToArray());
                Shortcuts.Add(new Shortcut(device, ShortcutDummy.Instance.Key, ShortcutDummy.Instance.Target));
                ShortcutDummy.Instance.flush();
            }
        }

        public void StartInterception()
        {
            List<IntPtr> hwid = new List<IntPtr>();
            List<int> key = new List<int>();
            List<ushort> vals = new List<ushort>();
            foreach (var x in Shortcuts)
            {
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
                
                hwid.Add(Marshal.StringToBSTR(x.Device.Hwid));
                key.Add(KeyToShort(x.Key));
            }

            InterceptionManager.start_interception(hwid.ToArray(), key.ToArray(), vals.ToArray(), hwid.Count);
        }

        public ushort KeyToShort(string key)
        {
            return Helpers.KeyHelper.Instance.KeyNum[key.ToUpper()];
        }
    }
}