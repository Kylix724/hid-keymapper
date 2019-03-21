using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptionKeymapper.Model
{

    public class DeviceManager : Helpers.LazySingleton<DeviceManager>
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
            string s = InterceptionManager.get_hardware_id();
            Console.WriteLine(s);         
            return s;
        }

        public void RemoveDevice(Device device)
        {
            Devices.Remove(device);
            //InterceptionManager.remove_device(System.Runtime.InteropServices.Marshal.StringToHGlobalAuto(device.Hwid));
        }
    }
}
