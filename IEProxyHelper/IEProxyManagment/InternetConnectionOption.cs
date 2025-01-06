using System;
using System.Runtime.InteropServices;

namespace IEProxyManagment
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct InternetConnectionOption
	{
		private static readonly int Size = Marshal.SizeOf(typeof(InternetConnectionOption));

		public PerConnOption m_Option;

		public InternetConnectionOption.InternetConnectionOptionValue m_Value;

		[StructLayout(LayoutKind.Explicit)]
		public struct InternetConnectionOptionValue
		{
			[FieldOffset(0)]
			public System.Runtime.InteropServices.ComTypes.FILETIME m_FileTime;

			[FieldOffset(0)]
			public int m_Int;

			[FieldOffset(0)]
			public IntPtr m_StringPtr;
		}
	}
}