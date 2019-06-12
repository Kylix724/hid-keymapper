using InterceptionKeymapper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace InterceptionKeymapper.Model
{

	public class ShortcutManager : Helpers.LazySingleton<ShortcutManager>
	{
		private static Vars VARS;
		private ObservableCollection<Shortcut> _shortcuts = new ObservableCollection<Shortcut>();
		public ObservableCollection<Shortcut> Shortcuts => _shortcuts;
		private Thread t;
		private Dictionary<string, ushort> _keyNum = new Dictionary<string, ushort>();

		static ShortcutManager()
		{
			VARS = Vars.Instance;
		}

		public Dictionary<string, ushort> KeyNum
		{
			get
			{
				return _keyNum;
			}
			set => _keyNum = value;
		}

		public Dictionary<ushort, string> KeyNumReverse
		{
			get
			{
				return _keyNum.ToDictionary((i) => i.Value, (i) => i.Key);
			}
		}

		public void AddShortcut(Device device)
		{
			if (device != null && VARS.Key != 0 && VARS.Target.Count() != 0)
			{
				List<ushort> vals = new List<ushort>();
				foreach (var t in VARS.Target.Split('+'))
				{
					vals.Add(KeyToShort(t));
				}
				//InterceptionManager.add_pair(Marshal.StringToHGlobalAuto(device.Hwid), KeyToShort(ShortcutDummy.Instance.KeyId), vals.ToArray());
				Shortcuts.Add(new Shortcut(device, VARS.Key, VARS.TargetList));
				VARS.FlushShortcut();
			}
		}

		public void StartInterception()
		{
			if (t == null || (t != null && !t.IsAlive))
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
				List<ushort> temp = x.Target;
				vals.AddRange(x.Target);
				for (int i = x.Target.Count(); i < 16; i++)
					vals.Add(0);
				hwid.Add(Marshal.StringToBSTR(DeviceManager.Instance.DevicesByName[x.Device].Hwid));
				key.Add(x.KeyId);
			}
			InterceptionManager.start_interception(hwid.ToArray(), key.ToArray(), vals.ToArray(), hwid.Count, VARS.ButtonDelay, KeyToShort(VARS.InterruptKey));
		}

		public ushort KeyToShort(string key)
		{
			if (KeyNum.ContainsKey(key))
				return KeyNum[key];
			else
				return 0;
		}

	}
}