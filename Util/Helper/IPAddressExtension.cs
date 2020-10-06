using System;
using System.Net;

namespace MSHC.Util.Helper
{
    public static class IPAddressExtension
	{
		public static IPAddress GetBroadcastAddress(this IPAddress subnet, IPAddress mask)
		{
			var bsnad = subnet.GetAddressBytes();
			var bmask = mask.GetAddressBytes();

			if (bmask.Length != bsnad.Length) return null;

			return new IPAddress(BitConverter.GetBytes(BitConverter.ToUInt32(bsnad, 0) | ~BitConverter.ToUInt32(bmask, 0)));
		}

		public static bool IsPartOfSubnet(this IPAddress addr, IPAddress subnet, IPAddress mask)
		{
			var bsnad = subnet.GetAddressBytes();
			var bmask = mask.GetAddressBytes();
			var baddr = addr.GetAddressBytes();

			if (bmask.Length != baddr.Length)
			{
				bsnad = subnet.MapToIPv4().GetAddressBytes();
				bmask = mask.MapToIPv4().GetAddressBytes();
				baddr = addr.MapToIPv4().GetAddressBytes();
			}

			if (bsnad.Length != bmask.Length || bmask.Length != baddr.Length) return false;

			for (int i = 0; i < bsnad.Length; i++)
			{
				if ((bsnad[i] & bmask[i]) != (baddr[i] & bmask[i])) return false;
			}

			return true;
		}
	}
}
