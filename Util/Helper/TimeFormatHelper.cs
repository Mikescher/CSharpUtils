using System;

namespace MSHC.Util.Helper
{
	public static class TimeFormatHelper
	{
		public static string FormatMilliseconds(long v, bool forceMinutes = false)
		{
			if (v < 0) return string.Empty; 

			var minutes = (int)(v / 1000f / 60f);
			var seconds = (int)((v - minutes * 1000 * 60) / 1000f);
			var millis = v - minutes * 1000 * 60 - seconds * 1000;

			if (forceMinutes)
				return string.Format("{0}m {1}s {2}ms", minutes, seconds, millis);
			else if (minutes > 0)
				return string.Format("{0}m {1:00}s {2}ms", minutes, seconds, millis);
			else if (seconds>0)
				return string.Format("{0}s {1}ms", seconds, millis);
			else
				return string.Format("{1}ms", seconds, millis);
		}

		public static string FormatTimespan(TimeSpan t) => FormatMilliseconds((long)t.TotalMilliseconds);
	}
}
