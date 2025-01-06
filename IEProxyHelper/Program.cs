using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using Utils.Helper;

namespace ProxyHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                string index = "2";

                if(args.Length > 1)
                {
                    index = args[1];
                }

                string proxyServerStr = ConfigHelper.GetSettings($"ProxyServer_{index}");
				string proxyOverrideStr = ConfigHelper.GetSettings("ProxyOverride");

                switch(args[0].ToLower())
                {
                    case "on":
                        SetProxy(proxyServerStr, proxyOverrideStr);
						break;

                    case "off":
						UnsetProxy();
                        break;

					case "info":
						ShowProxyInfo(string.Empty);
						break;
                }
            }

			Console.Read();
        }

		// Token: 0x0600000C RID: 12 RVA: 0x00002398 File Offset: 0x00000598
		public static void SetProxy(string proxyServer, string proxyOverride)
		{
			using (RegistryKey currentUser = Registry.CurrentUser)
			{
				string name = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
				RegistryKey registryKey = currentUser.OpenSubKey(name, true);
				registryKey.SetValue("ProxyEnable", 1);
				registryKey.SetValue("ProxyServer", proxyServer);
				registryKey.SetValue("ProxyOverride", proxyOverride);
				InternetSetOption(0, 39, IntPtr.Zero, 0);
				InternetSetOption(0, 37, IntPtr.Zero, 0);
				currentUser.Flush();
				ShowProxyInfo("代理开启成功！");
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002430 File Offset: 0x00000630
		public static void UnsetProxy()
		{
			RegistryKey currentUser = Registry.CurrentUser;
			string name = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
			RegistryKey registryKey = currentUser.OpenSubKey(name, true);
			registryKey.SetValue("ProxyEnable", 0);
			currentUser.Flush();
			InternetSetOption(0, 39, IntPtr.Zero, 0);
			InternetSetOption(0, 37, IntPtr.Zero, 0);
			currentUser.Close();
			ShowProxyInfo("代理关闭失败！");
		}

		private static void ShowProxyInfo(string text)
		{
			if (!GetProxyStatus())
			{
				Console.WriteLine("代理已关闭！");
			}
			else
            {
				if(!string.IsNullOrEmpty(text))
                {
					Console.WriteLine(text);
                }
				Console.WriteLine($"当前代理：{GetProxyServer()}");
			}
		}

		private static string GetProxyServer()
		{
			RegistryKey currentUser = Registry.CurrentUser;
			string name = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
			RegistryKey registryKey = currentUser.OpenSubKey(name, true);
			string result = (registryKey.GetValue("ProxyServer") == null) ? "" : registryKey.GetValue("ProxyServer").ToString();
			currentUser.Close();
			return result;
		}

		private static bool GetProxyStatus()
		{
			RegistryKey currentUser = Registry.CurrentUser;
			string name = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
			RegistryKey registryKey = currentUser.OpenSubKey(name, true);
			int num = Convert.ToInt32(registryKey.GetValue("ProxyEnable"));
			currentUser.Close();
			return num == 1;
		}

		// Token: 0x0600000E RID: 14
		[DllImport("wininet", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InternetSetOption(int hInternet, int dmOption, IntPtr lpBuffer, int dwBufferLength);
	}
}