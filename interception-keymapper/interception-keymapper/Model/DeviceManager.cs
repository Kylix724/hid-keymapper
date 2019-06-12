using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InterceptionKeymapper.Helpers;

namespace InterceptionKeymapper.Model
{

	public class DeviceManager : LazySingleton<DeviceManager>
	{
		private ObservableCollection<Device> _activeDevices = new ObservableCollection<Device>();
		public ObservableCollection<Device> ActiveDevices => _activeDevices;
		private ObservableCollection<Device> _devices = new ObservableCollection<Device>();
		public ObservableCollection<Device> Devices => _devices;

		public Dictionary<string, Device> DevicesByName
		{
			get
			{
				return Devices.ToDictionary(x => x.Name, x => x);
			}
		}

		public string GetDevice()
		{
			return InterceptionManager.get_hardware_id();
		}

		public void RemoveDevice(Device device)
		{
			Devices.Remove(device);
			//InterceptionManager.remove_device(System.Runtime.InteropServices.Marshal.StringToHGlobalAuto(device.Hwid));
		}
	}
}
