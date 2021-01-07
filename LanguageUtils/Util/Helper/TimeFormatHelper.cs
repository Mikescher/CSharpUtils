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
		
		public static string FormatMillisecondsTwoParts(long v)
		{
			if (v < 0) return string.Empty; 

			var hours = (int)(v / 1000f / 60f / 60f);
			v -= hours * 60 * 60 * 1000;

			var minutes = (int)(v / 1000f / 60f);
			v -= minutes * 60 * 1000;

			var seconds = (int)(v / 1000f);
			v -= seconds * 1000;

			var millis = v;

			if (hours>0)   return $"{hours}h {minutes:00}m";
			if (minutes>0) return $"{minutes:0}m {seconds:00}s";
			if (seconds>0) return $"{seconds:0}s {millis:000}ms";
			if (millis>0)  return $"{millis:0}ms";

			return "0ms";
		}
	}
}
