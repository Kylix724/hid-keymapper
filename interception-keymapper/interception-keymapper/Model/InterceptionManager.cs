using System;
using System.Runtime.InteropServices;

namespace InterceptionKeymapper.Model
{
	public static class InterceptionManager
	{
		public static string get_hardware_id()
		{
			return NativeMethods.get_hardware_id();
		}

		public static ushort get_key()
		{
			return NativeMethods.get_key();
		}
		public static void start_interception(IntPtr[] hwid, int[] key, ushort[] vals, int length, int delay, int interrupt)
		{
			NativeMethods.start_interception(hwid, key, vals, length, delay, interrupt);
		}
	}

	internal static class NativeMethods
	{
		[DllImport("wrapper", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public static extern string get_hardware_id();

		[DllImport("wrapper", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ushort get_key();
		[DllImport("wrapper", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void start_interception(IntPtr[] hwid, int[] key, ushort[] vals, int length, int delay, int interrupt);
	}
}
