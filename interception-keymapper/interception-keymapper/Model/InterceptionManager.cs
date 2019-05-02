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
		[DllImport("wrapper", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public extern static string get_hardware_id();
        [DllImport("wrapper", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public extern static void start_interception(IntPtr[] hwid, int[] key, ushort[] vals, int length, int delay, int interrupt);

    }
}
