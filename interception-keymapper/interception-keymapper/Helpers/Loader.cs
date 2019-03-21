using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using InterceptionKeymapper.Model;

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
				foreach(var shortcut in ShortcutManager.Instance.Shortcuts)
				{
					writer.WriteLine($"{shortcut.Device.Name},{shortcut.Key},{shortcut.Target}");
				}
			}
		}

		public static void ReadAllFromFile(string location)
		{
			DeviceManager.Instance.Devices.Clear();
			ShortcutManager.Instance.Shortcuts.Clear();
			using (var reader = new StreamReader(location)){
				while(!reader.EndOfStream){
					string line = reader.ReadLine();
					if (line == "")
						break;
					string[] split = line.Split(',');
					DeviceManager.Instance.Devices.Add(new Device(split[1], split[0]));
				}
				Dictionary<string, Device> devicesByName = DeviceManager.Instance.DevicesByName;
				while(!reader.EndOfStream){
					string[] split = reader.ReadLine().Split(',');
					ShortcutManager.Instance.Shortcuts.Add(new Shortcut(devicesByName[split[0].Split('(')[0]], split[1], split[2]));
				}
			}
		}
	}
}
