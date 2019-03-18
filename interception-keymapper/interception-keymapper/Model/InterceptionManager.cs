using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using InterceptionKeymapper.Helpers;

namespace InterceptionKeymapper.Model
{
    public class InterceptionManager : LazySingleton<InterceptionManager>
    {
        //[DllImport("wrapper.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        //public extern static void add_pair(IntPtr hwid, int key, ushort[] vals);        
        //[DllImport("wrapper.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        //public extern static void remove_pair(IntPtr hwid, int key);
        //[DllImport("wrapper.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        //public extern static void remove_device(IntPtr hwid);
        //[DllImport("wrapper.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        //public extern static void add_device(string hwid);

        [DllImport("wrapper.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public extern static string get_hardware_id();
        [DllImport("wrapper.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public extern static void start_interception(IntPtr[] hwid, int[] key, ushort[] vals, int length);

    }
}
