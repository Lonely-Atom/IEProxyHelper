using System;
using System.Runtime.InteropServices;

namespace IEProxyManagment
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct InternetPerConnOptionList
	{
		public int dwSize;

		public IntPtr szConnection;

		public int dwOptionCount;

		public int dwOptionError;

		public IntPtr options;
	}
}
