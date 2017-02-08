using MSHC.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MSHC.Helper
{
	public static class XHelper
	{
		#region GetChildValue

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

			throw new ArgumentException("'"+child.Value+"' is not a valid value for Enum");
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
	}
}
