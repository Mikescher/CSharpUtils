using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using MSHC.Lang.Exceptions;

namespace MSHC.Serialization
{
	public static class XElementExtensions
	{
		#region Attribute Accessor

		public static string StringAttribute(this XElement element, string key, string defaultValue)
		{
			var attr = element.Attribute(key);
			if (attr != null) return attr.Value;

			return defaultValue;
		}

		public static string StringAttribute(this XElement element, string key)
		{
			var attr = element.Attribute(key);
			if (attr != null) return attr.Value;

			throw new XMLStructureException(string.Format("Attribute {0} not found on element {1}", key, element.Name.LocalName));
		}

		public static double DoubleAttribute(this XElement element, string key, double defaultValue)
		{
			var attr = element.Attribute(key);
			if (attr != null)
			{
				double temp;
				if (double.TryParse(attr.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out temp)) return temp;
			}

			return defaultValue;
		}

		public static double DoubleAttribute(this XElement element, string key)
		{
			var attr = element.Attribute(key);
			if (attr != null)
			{
				double temp;
				if (double.TryParse(attr.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out temp)) return temp;
			}

			throw new XMLStructureException(string.Format("Attribute {0} not found on element {1}", key, element.Name.LocalName));
		}

		public static int IntAttribute(this XElement element, string key, int defaultValue)
		{
			var attr = element.Attribute(key);
			if (attr != null)
			{
				int temp;
				if (int.TryParse(attr.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out temp)) return temp;
			}

			return defaultValue;
		}

		public static int IntAttribute(this XElement element, string key)
		{
			var attr = element.Attribute(key);
			if (attr != null)
			{
				int temp;
				if (int.TryParse(attr.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out temp)) return temp;
			}

			throw new XMLStructureException(string.Format("Attribute {0} not found on element {1}", key, element.Name.LocalName));
		}

		public static bool BoolAttribute(this XElement element, string key)
		{
			var attr = element.Attribute(key);
			if (attr != null)
			{
				return ParseBool(attr.Value);
			}

			throw new XMLStructureException(string.Format("Attribute {0} not found on element {1}", key, element.Name.LocalName));
		}

		public static bool BoolAttribute(this XElement element, string key, bool defaultValue)
		{
			var attr = element.Attribute(key);
			if (attr != null)
			{
				bool? temp = TryParseBool(attr.Value);
				if (temp != null) return temp.Value;
			}

			return defaultValue;
		}

		public static Guid GuidAttribute(this XElement element, string key)
		{
			var attr = element.Attribute(key);
			if (attr != null)
			{
				Guid temp;
				if (Guid.TryParse(attr.Value, out temp)) return temp;
			}

			throw new XMLStructureException(string.Format("Attribute {0} not found on element {1}", key, element.Name.LocalName));
		}

		#endregion

		#region ParseBool

		public static bool ParseBool(string value)
		{
			var b = TryParseBool(value);

			if (b == null) throw new XMLStructureException("Unparseable boolean value: " + value);

			return b.Value;
		}

		public static bool? TryParseBool(string value)
		{
			if (value == null) return null;

			if (value.ToLower() == "true") return true;
			if (value.ToLower() == "false") return false;

			if (value.ToLower() == "1") return true;
			if (value.ToLower() == "0") return false;

			return null;
		}

		#endregion

		#region XList

		public static string XListSingleOrDefault(this XContainer x, params string[] p)
		{
			return XList(x, p).FirstOrDefault();
		}

		public static string XListSingle(this XContainer x, params string[] p)
		{
			var r = XList(x, p).FirstOrDefault();
			if (r == null) throw new Exception("XML Entry not found: " + string.Join(" --> ", p.Select(pp => $"[{pp}]")));
			return r;
		}

		public static IEnumerable<string> XList(this XContainer x, params string[] p)
		{
			if (p.Last().StartsWith("."))
			{
				var last = p.Last();
				var search = last.Substring(1);

				foreach (var elem in XElemList(x, p.Reverse().Skip(1).Reverse().ToArray()))
				{
					foreach (var attr in elem.Attributes().Where(e => e.Name.LocalName.ToLower() == search.ToLower()))
					{
						yield return attr.Value;
					}
				}
			}
			else
			{
				foreach (var elem in XElemList(x, p))
				{
					yield return elem.Value;
				}
			}
		}

		/// <summary>
		/// Reserved Chars:  * . @ ~ & = ? #
		/// 
		/// XML-Tag by name (case-insensitive):
		///     "asdf"
		/// 
		/// Optional XML-Tag by name (case-insensitive):
		///     "?asdf"
		/// 
		/// None,One,Many tags by name (case-insensitive):
		///     "*asdf"
		/// 
		/// Tag with attribute+value
		///     "asdf@attr=value"
		/// 
		/// Tag with multiple attribute+value
		///     "asdf@attr=value&other=umts"
		/// 
		/// Tag with attribute + any value
		///     "asdf@attr=~"
		/// 
		/// Query for attribute value (only in XList)
		///     ".attr"
		/// 
		/// Any XML-Tag regardless of name:
		///     "*"
		/// 
		/// Zero or one XML-Tag regardless of name:
		///     "*?"
		/// 
		/// Only first matching tag:
		///     "asdf#first"
		///     "asdf@attr=value#first"
		/// 
		/// Match tag name case-sensitive:
		///     "asdf#exact"
		/// 
		/// </summary>
		public static IEnumerable<XElement> XElemList(this XContainer x, params string[] p)
		{
			const int SM_NORMAL   = 0;
			const int SM_MULTI    = 1;
			const int SM_OPTIONAL = 2;

			var search = p[0];
			var nn = p.Skip(1).ToArray();

			int searchMode = SM_NORMAL;
			bool wildcard = false; // match all tags

			bool optFirstOnly = false;
			bool optMatchCase = false;

			if (search == "*?" || search.StartsWith("*?@"))
			{
				search = "";
				searchMode = SM_OPTIONAL;
				wildcard = true;
			}
			else if (search == "*" || search.StartsWith("*@"))
			{
				search = "";
				searchMode = SM_NORMAL;
				wildcard = true;
			}
			else if (search.Length > 1 && search.StartsWith("*"))
			{
				search = search.Substring(1);
				searchMode = SM_MULTI;
			}
			else if (search.Length > 1 && search.StartsWith("?"))
			{
				search = search.Substring(1);
				searchMode = SM_OPTIONAL;
			}

			var options = "";

			List<Tuple<string, string>> attrFilter = new List<Tuple<string, string>>();
			if (search.Contains('@'))
			{
				var split = search.Split('@');

				search = split[0];

				var attrSearch = split[1];
				if (attrSearch.Contains('#'))
				{
					var split2 = attrSearch.Split('#');
					attrSearch = split2[0];
					options = split2[1];
				}

				foreach (var filter in attrSearch.Split('&'))
				{
					attrFilter.Add(Tuple.Create(filter.Split('=')[0], filter.Split('=')[1]));
				}
			}
			else if (search.Contains('#'))
			{
				var split = search.Split('#');
				search = split[0];
				options = split[1];
			}
			
			// parse options
			if (options != "")
			{
				var optlist = options.Split('&');
				foreach (var opt in optlist)
				{
					if (opt.ToLower() == "first") optFirstOnly = true;
					else if (opt.ToLower() == "exact") optMatchCase = true;
					else throw new Exception("Unknown Option: " + opt);
				}
			}

			// get matching children (by tagname)
			var matched_children = x.Elements().Where(e => wildcard || e.Name.LocalName.ToLower() == search.ToLower());

			if (optMatchCase) matched_children = matched_children.Where(e => wildcard || e.Name.LocalName == search);

			// filter matches some more (by tags etc)
			foreach (var filter in attrFilter)
			{
				matched_children = matched_children.Where(xx => xx.Attributes().Any(e => e.Name.LocalName.ToLower() == filter.Item1.ToLower()));
				if (filter.Item2 != "~") matched_children = matched_children.Where(xx => xx.Attributes().Where(e => e.Name.LocalName.ToLower() == filter.Item1.ToLower()).Any(attr => attr.Value == filter.Item2));
			}
			
			if (optFirstOnly) matched_children = matched_children.Take(1);

			var matches = matched_children.ToList();

			if (nn.Length == 0) // end of recursion, return data
			{
				foreach (var f in matches) yield return f;
				yield break;
			}
			
			if (searchMode == SM_NORMAL)
			{
				// continue recursion normally
				foreach (var f in matches) foreach (var rf in XElemList(f, nn)) yield return rf;
			}
			else if (searchMode == SM_OPTIONAL)
			{
				// continue recursion with this elem skipped
				// (interprete ? as zero-times)
				foreach (var rf in XElemList(x, nn)) yield return rf;

				// continue recursion normally
				// (interprete ? as one-time)
				foreach (var f in matches) foreach (var rf in XElemList(f, nn)) yield return rf;
			}
			else if (searchMode == SM_MULTI)
			{
				// continue recursion with this elem skipped
				// (interprete * as zero-times)
				foreach (var rf in XElemList(x, nn)) yield return rf;

				// continue recursion normally
				// (interprete * as one-time)
				foreach (var f in matches) foreach (var rf in XElemList(f, nn)) yield return rf;

				// continue recursion with same same again (so the current tag can repeat)
				// (interprete * as more-than-one-time)
				foreach (var f in matches) foreach (var rf in XElemList(f, p)) yield return rf;
			}
		}

		#endregion
	}
}
