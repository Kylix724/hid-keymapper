using InterceptionKeymapper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace InterceptionKeymapper.Model
{

	public class ShortcutManager : LazySingleton<ShortcutManager>
	{
		private static Vars VARS;
		private static DeviceManager DM;
		private Thread t;

		/// <summary>
		/// A Collection of all Shortcuts
		/// </summary>
		public ObservableCollection<Shortcut> Shortcuts { get; } = new ObservableCollection<Shortcut>();

		static ShortcutManager()
		{
			VARS = Vars.Instance;
			DM = DeviceManager.Instance;
		}

		private Dictionary<string, ushort> _keyNum = new Dictionary<string, ushort>();
		/// <summary>
		/// The Dictionary of all keys with their corresponding integers
		/// </summary>
		public Dictionary<string, ushort> KeyNum
		{
			get
			{
				return _keyNum;
			}
			set => _keyNum = value;
		}
		public Dictionary<ushort, string> _keyNumReverse;
		/// <summary>
		/// The Dictionary of all key integers with their string name, reverse of KeyNum
		/// </summary>
		public Dictionary<ushort, string> KeyNumReverse
		{
			get
			{
				if (_keyNumReverse == null)
				{
					_keyNumReverse = new Dictionary<ushort, string>();
					foreach (var x in KeyNum.Keys)
					{
						_keyNumReverse[KeyNum[x]] = x;
					}
				}
				return _keyNumReverse;
			}
		}

		/// <summary>
		/// Add a shortcut to the List with the key and number values set in Vars
		/// </summary>
		/// <param name="device">The input device the Shortcut should work on</param>
		public void AddShortcut(Device device)
		{
			if (device != null && VARS.Key != 0 && VARS.Target.Count() != 0)
			{
				List<ushort> vals = new List<ushort>();
				foreach (var t in VARS.Target.Split('+'))
				{
					vals.Add(KeyToShort(t));
				}
				Shortcuts.Add(new Shortcut(device, VARS.Key, VARS.TargetList));
				VARS.FlushShortcut();
			}
		}

		/// <summary>
		/// Starts the thread in which the current list of shortcuts is activated
		/// </summary>
		public void StartInterception()
		{
			if (t == null || (t != null && !t.IsAlive))
			{
				t = new Thread(new ThreadStart(Intercept));
				t.Start();
			}
		}

		/// <summary>
		/// Starts the interception process
		/// </summary>
		private void Intercept()
		{
			List<IntPtr> hwid = new List<IntPtr>();
			List<int> key = new List<int>();
			List<ushort> vals = new List<ushort>();
			foreach (var x in Shortcuts)
			{
				if (!DM.DevicesByName[x.Device].Active)
					continue;
				List<ushort> temp = x.Target;
				vals.AddRange(x.Target);
				for (int i = x.Target.Count(); i < 16; i++)
					vals.Add(0);
				hwid.Add(Marshal.StringToBSTR(DM.DevicesByName[x.Device].Hwid));
				key.Add(x.KeyId);
			}
			InterceptionManager.start_interception(hwid.ToArray(), key.ToArray(), vals.ToArray(), hwid.Count, VARS.ButtonDelay, KeyToShort(VARS.InterruptKey));
		}

		/// <summary>
		/// Finds the corresponding number for a key string, 0 if not found
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public ushort KeyToShort(string key)
		{
			if (KeyNum.ContainsKey(key))
				return KeyNum[key];
			else
				return 0;
		}
	}
}