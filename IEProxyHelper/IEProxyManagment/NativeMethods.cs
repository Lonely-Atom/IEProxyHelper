using System;
using System.Runtime.InteropServices;

namespace IEProxyManagment
{
	internal static class NativeMethods
	{
		[DllImport("WinInet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool InternetSetOption(IntPtr hInternet, InternetOption dwOption, IntPtr lpBuffer, int dwBufferLength);
	}
}