using System;

namespace IEProxyManagment
{
	[Flags]
	public enum PerConnFlags
	{
		PROXY_TYPE_DIRECT = 1,
		PROXY_TYPE_PROXY = 2,
		PROXY_TYPE_AUTO_PROXY_URL = 4,
		PROXY_TYPE_AUTO_DETECT = 8
	}
}