using HidKeymapper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HidKeymapper.Helpers
{
	class Loader
	{
		public static void SaveAllToFile(string location)
		{
			ShortcutManager SM = ShortcutManager.Instance;
			DeviceManager DM = DeviceManager.Instance;
			using (var writer = File.CreateText(location))
			{
				foreach (var device in DM.Devices)
				{
					writer.WriteLine($"{device.Name},{device.Hwid}");
				}
				writer.WriteLine("");
				foreach (var shortcut in SM.Shortcuts)
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
			DeviceManager DM = DeviceManager.Instance;
			ShortcutManager SM = ShortcutManager.Instance;
			DM.Devices.Clear();
			SM.Shortcuts.Clear();
			using (var reader = new StreamReader(location))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					if (line == "")
						break;
					string[] split = line.Split(',');
					DM.Devices.Add(new Device(split[1], split[0]));
				}
				Dictionary<string, Device> devicesByName = DM.DevicesByName;
				while (!reader.EndOfStream)
				{
					string[] split = reader.ReadLine().Split(',');
					try
					{
						if (ushort.TryParse(split[1], out ushort z))
							SM.Shortcuts.Add(new Shortcut(split[0], ushort.Parse(split[1]), (from y in split[2].Split('+') select ushort.Parse(y)).ToList()));
						else
							SM.Shortcuts.Add(new Shortcut(split[0], SM.KeyNum[split[1]], (from y in split[2].Split('+') select SM.KeyNum[y]).ToList()));
					}
					catch (KeyNotFoundException e)
					{
						Console.Error.Write("File incompatible with key library");
						Console.Error.Write(e.StackTrace);
						return;
					}

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
			ShortcutManager SM = ShortcutManager.Instance;
			if (File.Exists("keys.ini"))
				using (var sr = new StreamReader("keys.ini"))
				{
					string x;
					while ((x = sr.ReadLine()) != null)
					{
						var y = x.Split('=');
						SM.KeyNum.Add(y[0], ushort.Parse(y[1]));
					}
				}
			else
			{
				SM.KeyNum = KeyHelper.KeyNum;
			}
		}

		public static void SaveKeys()
		{
			ShortcutManager SM = ShortcutManager.Instance;
			using (var sw = new StreamWriter("keys.ini"))
			{
				foreach (string key in SM.KeyNum.Keys)
				{
					sw.WriteLine($"{key}={SM.KeyNum[key]}");
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
