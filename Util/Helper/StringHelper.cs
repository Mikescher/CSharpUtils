using System;

namespace MSHC.Util.Helper
{
	public static class StringHelper
	{
		// https://stackoverflow.com/a/8809437/1761622
		public static string ReplaceFirst(string text, string search, string replace)
		{
			var pos = text.IndexOf(search, StringComparison.Ordinal);
			if (pos < 0) return text;
			return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}
	}
}
