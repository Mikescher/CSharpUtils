using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MSHC.Util.Helper
{
	public static class RegexHelper
	{
		public static string RegexRemove(string hay, Group g)
		{
			return hay.Substring(0, g.Index) + hay.Substring(g.Index + g.Length);
		}

		public static List<string> List(string rex, string data)
		{
			return Regex.Matches(data, rex).Cast<Match>().Where(p => p.Success).Select(p => p.Groups[1].Value).ToList();
		}
	}
}
