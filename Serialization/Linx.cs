using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MSHC.Serialization
{
	public static class Linx
	{
		public static string XListSingleOrDefault(XContainer x, params string[] p)
		{
			var r = XList(x, p).FirstOrDefault();
			if (string.IsNullOrEmpty(r)) return null;
			return r;
		}

		/// <summary>
		/// ["x", "y"]               Get value of <y> node, with parent <x>
		/// ["x", "y", ".attr"]      Get attributevalue of attribute "attr" on node "y"
		/// ["x", "y@attr=3"]        Get value of <y> node where the attribute "attr" has the value 3
		/// ["x", "y@attr=~"]        Get value of <y> node where the attribute "attr" exists
		/// </summary>
		public static IEnumerable<string> XList(XContainer x, params string[] p)
		{
			var search = p[0];
			var nn = p.Skip(1).ToArray();

			if (search.StartsWith("."))
			{
				search = search.Substring(1);

				foreach (var attr in ((XElement)x).Attributes().Where(e => e.Name.LocalName.ToLower() == search.ToLower()))
				{
					yield return attr.Value;
				}
				yield break;
			}

			string attrName = null;
			string attrValue = null;
			if (search.Contains('@'))
			{
				attrName = search.Split('@')[1].Split('=')[0];
				attrValue = search.Split('@')[1].Split('=')[1];
				search = search.Split('@')[0];
			}

			var xf = x.Elements().Where(e => e.Name.LocalName.ToLower() == search.ToLower());

			if (attrName != null) xf = xf.Where(xx => xx.Attributes().Any(e => e.Name.LocalName.ToLower() == attrName.ToLower()));
			if (attrName != null && attrValue != "~") xf = xf.Where(xx => xx.Attributes().Where(e => e.Name.LocalName.ToLower() == attrName.ToLower()).Any(attr => attr.Value == attrValue));

			if (nn.Length == 0)
			{
				foreach (var f in xf) yield return f.Value;
			}
			else
			{
				foreach (var f in xf) foreach (var rf in XList(f, nn)) yield return rf;
			}
		}
	}
}
