using InterceptionKeymapper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InterceptionKeymapper.Helpers
{
	class Loader
	{
		public static void SaveAllToFile(string location)
		{
			using (var writer = File.CreateText(location))
			{
				foreach (var device in DeviceManager.Instance.Devices)
				{
					writer.WriteLine($"{device.Name},{device.Hwid}");
				}
				writer.WriteLine("");
				foreach (var shortcut in ShortcutManager.Instance.Shortcuts)
				{
					string x = "";
					foreach (var key in shortcut.Target)
					{
						if (x != "")
							x = $"{x}+";
						x = $"{x}{key.ToString()}";

					}
					writer.WriteLine($"{shortcut.Device},{shortcut.KeyId},{x}");
				}
			}
		}

		public static void ReadAllFromFile(string location)
		{
			DeviceManager.Instance.Devices.Clear();
			ShortcutManager.Instance.Shortcuts.Clear();
			ShortcutManager x = ShortcutManager.Instance;
			using (var reader = new StreamReader(location))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					if (line == "")
						break;
					string[] split = line.Split(',');
					DeviceManager.Instance.Devices.Add(new Device(split[1], split[0]));
				}
				Dictionary<string, Device> devicesByName = DeviceManager.Instance.DevicesByName;
				while (!reader.EndOfStream)
				{
					string[] split = reader.ReadLine().Split(',');
					ushort z;
					if (ushort.TryParse(split[1], out z))
						x.Shortcuts.Add(new Shortcut(split[0], ushort.Parse(split[1]), (from y in split[2].Split('+') select ushort.Parse(y)).ToList()));
					else
						x.Shortcuts.Add(new Shortcut(split[0], x.KeyNum[split[1]], (from y in split[2].Split('+') select x.KeyNum[y]).ToList()));
				}
			}
		}

		public static void SaveSettings()
		{
			Vars VARS = Vars.Instance;
			using (var sw = new StreamWriter("config.ini"))
			{
				sw.WriteLine($"InterruptKey={VARS.InterruptKey}");
				sw.WriteLine($"Delay={VARS.ButtonDelay}");
			}
		}

		public static void LoadSettings()
		{
			Vars VARS = Vars.Instance;
			if (File.Exists("config.ini"))
				using (var sr = new StreamReader("config.ini"))
				{
					var x = sr.ReadLine();
					if (x != null)
						VARS.InterruptKey = x.Split('=')[1];
					x = sr.ReadLine();
					if (x != null)
						VARS.ButtonDelay = Int32.Parse(x.Split('=')[1]);
				}
		}

		public static void LoadKeys()
		{
			if (File.Exists("keys.ini"))
				using (var sr = new StreamReader("keys.ini"))
				{
					string x;
					while ((x = sr.ReadLine()) != null)
					{
						var y = x.Split('=');
						ShortcutManager.Instance.KeyNum.Add(y[0], ushort.Parse(y[1]));
					}
				}
			else
			{
				ShortcutManager.Instance.KeyNum = KeyHelper.KeyNum;
			}
		}

		public static void SaveKeys()
		{
			using (var sw = new StreamWriter("keys.ini"))
			{
				foreach (string key in ShortcutManager.Instance.KeyNum.Keys)
				{
					sw.WriteLine($"{key}={ShortcutManager.Instance.KeyNum[key]}");
				}
			}
		}

		public static void OnLaunch()
		{
			LoadSettings();
			LoadKeys();
		}
		public static void OnExit()
		{
			SaveSettings();
			SaveKeys();
		}
	}
}
