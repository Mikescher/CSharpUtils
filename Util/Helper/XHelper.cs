﻿using MSHC.Lang.Exceptions;
using MSHC.Lang.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MSHC.Util.Helper
{
	public static class XHelper
	{
		#region GetChildValue optional

		public static string GetChildValue(XElement parent, string childName, string defaultValue)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			return child.Value;
		}

		public static int GetChildValue(XElement parent, string childName, int defaultValue)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			return int.Parse(child.Value);
		}

		public static int? GetChildValue(XElement parent, string childName, int? defaultValue)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			if (child.Value.Trim() == "") return null;

			return int.Parse(child.Value);
		}

		public static bool GetChildValue(XElement parent, string childName, bool defaultValue)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			return XElementExtensions.ParseBool(child.Value);
		}

		public static Guid GetChildValue(XElement parent, string childName, Guid defaultValue)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			return Guid.Parse(child.Value);
		}

		public static double GetChildValue(XElement parent, string childName, double defaultValue)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			return double.Parse(child.Value);
		}

		public static TEnumType GetChildValue<TEnumType>(XElement parent, string childName, TEnumType defaultValue) where TEnumType : struct, IComparable, IFormattable, IConvertible
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			int value;
			TEnumType evalue;
			if (int.TryParse(child.Value, out value))
			{
				foreach (var enumValue in Enum.GetValues(typeof(TEnumType)))
				{
					if (value == Convert.ToInt32(Enum.Parse(typeof(TEnumType), enumValue.ToString())))
						return (TEnumType)enumValue;
				}
			}
			if (Enum.TryParse(child.Value, true, out evalue))
			{
				return evalue;
			}

			throw new ArgumentException("'" + child.Value + "' is not a valid value for Enum");
		}

		public static object GetChildValue(XElement parent, string childName, object defaultValue, Type enumType)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) return defaultValue;

			int value;
			if (int.TryParse(child.Value, out value))
			{
				foreach (var enumValue in Enum.GetValues(enumType))
				{
					if (value == Convert.ToInt32(Enum.Parse(enumType, enumValue.ToString())))
						return enumValue;
				}
			}

			return Enum.Parse(enumType, child.Value);
		}

		#endregion

		#region GetChildValue

		public static string GetChildValueString(XElement parent, string childName)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			return child.Value;
		}

		public static int GetChildValueInt(XElement parent, string childName)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			return int.Parse(child.Value);
		}

		public static int? GetChildValueNint(XElement parent, string childName)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			if (child.Value.Trim() == "") return null;

			return int.Parse(child.Value);
		}

		public static bool GetChildValueBool(XElement parent, string childName)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			return XElementExtensions.ParseBool(child.Value);
		}

		public static Guid GetChildValueGUID(XElement parent, string childName)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			return Guid.Parse(child.Value);
		}

		public static TEnumType GetChildValueEnum<TEnumType>(XElement parent, string childName) where TEnumType : struct, IComparable, IFormattable, IConvertible
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			int value;
			TEnumType evalue;
			if (int.TryParse(child.Value, out value))
			{
				foreach (var enumValue in Enum.GetValues(typeof(TEnumType)))
				{
					if (value == Convert.ToInt32(Enum.Parse(typeof(TEnumType), enumValue.ToString())))
						return (TEnumType)enumValue;
				}
			}
			if (Enum.TryParse(child.Value, true, out evalue))
			{
				return evalue;
			}

			throw new ArgumentException("'" + child.Value + "' is not a valid value for Enum");
		}

		public static List<string> GetChildValueStringList(XElement parent, string childName, string subNodeName)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			return child.Elements(subNodeName).Select(p => p.Value).ToList();
		}

		public static DateTimeOffset GetChildValueDateTimeOffset(XElement parent, string childName)
		{
			var child = parent.Elements(childName).FirstOrDefault();
			if (child == null) throw new XMLStructureException("Node not found: " + childName);

			return DateTimeOffset.Parse(child.Value);
		}

		#endregion

		public static string ConvertToString(XDocument doc)
		{
			if (doc == null) throw new ArgumentNullException("doc");

			StringBuilder builder = new StringBuilder();
			using (TextWriter writer = new StringWriter(builder))
			{
				doc.Save(writer);
			}
			return builder.ToString();
		}

		#region GetAttribute

		public static Guid GetAttributeGuid(XElement elem, string attrname)
		{
			var attr = elem.Attribute(attrname);
			if (attr == null) throw new XMLStructureException("Attribute not found: " + attrname);
			return Guid.Parse(attr.Value);
		}

		public static string GetAttributeString(XElement elem, string attrname)
		{
			var attr = elem.Attribute(attrname);
			if (attr == null) throw new XMLStructureException("Attribute not found: " + attrname);
			return attr.Value;
		}

		public static int GetAttributeInt(XElement elem, string attrname)
		{
			var attr = elem.Attribute(attrname);
			if (attr == null) throw new XMLStructureException("Attribute not found: " + attrname);
			return int.Parse(attr.Value);
		}

		#endregion
	}
}
