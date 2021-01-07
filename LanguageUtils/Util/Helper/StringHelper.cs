using System;
using System.Globalization;
using System.Text;

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

		// https://stackoverflow.com/a/368850/1761622
		public static string RemoveDiacritics(string text)
		{
			var formD = text.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder();

			foreach (char ch in formD)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
			}

			return sb.ToString().Normalize(NormalizationForm.FormC);
		}
	}
}
