using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace IEProxyManagment
{
    public class IEProxySetting
    {
        public static bool UnsetProxy()
        {
            return SetProxy(null, null);
        }
        public static bool SetProxy(string strProxy)
        {
            return SetProxy(strProxy, null);
        }

        public static bool SetProxy(string strProxy, string exceptions)
        {
            InternetPerConnOptionList internetPerConnOptionList = default;
            int num = string.IsNullOrEmpty(strProxy) ? 1 : (string.IsNullOrEmpty(exceptions) ? 2 : 3);
            InternetConnectionOption[] array = new InternetConnectionOption[num];
            // USE a proxy server ...
            array[0].m_Option = PerConnOption.INTERNET_PER_CONN_FLAGS;
            array[0].m_Value.m_Int = ((num < 2) ? 1 : 3);
            // use THIS proxy server
            if (num > 1)
            {
                array[1].m_Option = PerConnOption.INTERNET_PER_CONN_PROXY_SERVER;
                array[1].m_Value.m_StringPtr = Marshal.StringToHGlobalAuto(strProxy);
                // except for these addresses ...
                if (num > 2)
                {
                    array[2].m_Option = PerConnOption.INTERNET_PER_CONN_PROXY_BYPASS;
                    array[2].m_Value.m_StringPtr = Marshal.StringToHGlobalAuto(exceptions);
                }
            }

            // default stuff
            internetPerConnOptionList.dwSize = Marshal.SizeOf(internetPerConnOptionList);
            internetPerConnOptionList.szConnection = IntPtr.Zero;
            internetPerConnOptionList.dwOptionCount = array.Length;
            internetPerConnOptionList.dwOptionError = 0;

            int num2 = Marshal.SizeOf(typeof(InternetConnectionOption));
            // make a pointer out of all that ...
            IntPtr intPtr = Marshal.AllocCoTaskMem(num2 * array.Length);
            // copy the array over into that spot in memory ...
            for (int i = 0; i < array.Length; i++)
            {
                IntPtr ptr = new IntPtr(intPtr.ToInt32() + (i * num2));
                Marshal.StructureToPtr(array[i], ptr, false);
            }

            internetPerConnOptionList.options = intPtr;

            // and then make a pointer out of the whole list
            IntPtr intPtr2 = Marshal.AllocCoTaskMem(internetPerConnOptionList.dwSize);
            Marshal.StructureToPtr(internetPerConnOptionList, intPtr2, false);

            // and finally, call the API method!
            int num3 = NativeMethods.InternetSetOption(IntPtr.Zero, InternetOption.INTERNET_OPTION_PER_CONNECTION_OPTION, intPtr2, internetPerConnOptionList.dwSize) ? -1 : 0;
            if (num3 == 0)
            {  // get the error codes, they might be helpful
                num3 = Marshal.GetLastWin32Error();
            }
            // FREE the data ASAP
            Marshal.FreeCoTaskMem(intPtr);
            Marshal.FreeCoTaskMem(intPtr2);
            if (num3 > 0)
            {  // throw the error codes, they might be helpful
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return num3 < 0;
        }
    }
}