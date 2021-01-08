using AlephNote.PluginInterface.Util;
using MSHC.Math.Encryption;
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace MSHC.Serialization
{
    public static class XHelper2
    {
        public static XHelper2Def<string> TypeString = new XHelper2Def<string>("String", true)
        {
            FromAttribute = (v) => v,
            ToAttribute   = (v) => v,
        };

        public static XHelper2Def<string> TypeStringC80Base64 = new XHelper2Def<string>("String(C80Base64)", true)
        {
            FromAttribute = (v) => XHelper.ConvertFromC80Base64(v),
            ToAttribute   = (v) => XHelper.ConvertToC80Base64(v),
        };

        public static XHelper2Def<int> TypeInt = new XHelper2Def<int>("Integer", false)
        {
            FromAttribute = (v) => int.Parse(v, CultureInfo.InvariantCulture),
            ToAttribute   = (v) => v.ToString(CultureInfo.InvariantCulture),
        };

        public static XHelper2Def<int?> TypeNullableInt = new XHelper2Def<int?>("Nullable<Integer>", true)
        {
            FromAttribute = (v) => int.Parse(v, CultureInfo.InvariantCulture),
            ToAttribute   = (v) => v.Value.ToString(CultureInfo.InvariantCulture),
        };

        public static XHelper2Def<double> TypeDouble = new XHelper2Def<double>("Double", false)
        {
            FromAttribute = (v) => double.Parse(v, NumberStyles.Float, CultureInfo.InvariantCulture),
            ToAttribute   = (v) => v.ToString(CultureInfo.InvariantCulture),
        };

        public static XHelper2Def<double?> TypeNullableDouble = new XHelper2Def<double?>("Nullable<Double>", true)
        {
            FromAttribute = (v) => double.Parse(v, NumberStyles.Float, CultureInfo.InvariantCulture),
            ToAttribute   = (v) => v.Value.ToString(CultureInfo.InvariantCulture),
        };

        public static XHelper2Def<bool> TypeBool = new XHelper2Def<bool>("Bool", false)
        {
            FromAttribute = (v) => XElementExtensions.ParseBool(v),
            ToAttribute   = (v) => v.ToString(),
        };

        public static XHelper2Def<bool?> TypeNullableBool = new XHelper2Def<bool?>("Nullable<Bool>", true)
        {
            FromAttribute = (v) => XElementExtensions.ParseBool(v),
            ToAttribute   = (v) => v.Value.ToString(),
        };

        public static XHelper2Def<Guid> TypeGuid = new XHelper2Def<Guid>("Guid", false)
        {
            FromAttribute = (v) => Guid.Parse(v),
            ToAttribute   = (v) => v.ToString("B"),
        };

        public static XHelper2Def<Guid?> TypeNullableGuid = new XHelper2Def<Guid?>("Nullable<Guid>", true)
        {
            FromAttribute = (v) => Guid.Parse(v),
            ToAttribute   = (v) => v.Value.ToString("B"),
        };

        public static XHelper2Def<byte[]> TypeByteArrayHex = new XHelper2Def<byte[]>("ByteArray(Hex)", true)
        {
            FromAttribute = (v) => EncodingConverter.StringToByteArrayCaseInsensitive(v),
            ToAttribute   = (v) => EncodingConverter.ByteToHexBitFiddleUppercase(v),
        };

        public static XHelper2Def<byte[]> TypeByteArrayBase64 = new XHelper2Def<byte[]>("ByteArray(Base64)", true)
        {
            FromAttribute = (v) => Convert.FromBase64String(v),
            ToAttribute   = (v) => Convert.ToBase64String(v),
        };

        public static XHelper2Def<DateTimeOffset> TypeDateTimeOffset = new XHelper2Def<DateTimeOffset>("DateTimeOffset", false)
        {
            FromAttribute = (v) => DateTimeOffset.Parse(v),
            ToAttribute   = (v) => v.ToString("B"),
        };

        public static XHelper2Def<DateTimeOffset?> TypeNullableDateTimeOffset = new XHelper2Def<DateTimeOffset?>("Nullable<DateTimeOffset>", true)
        {
            FromAttribute = (v) => DateTimeOffset.Parse(v),
            ToAttribute   = (v) => v.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture),
        };
    }

    public class XHelper2Def<TVal>
    {
        public readonly string Name;
        public readonly bool Nullable;

        internal Func<string, TVal> FromAttribute { get; set; } = (_) => throw new NotImplementedException();
        internal Func<TVal, string> ToAttribute { get; set; } = (_) => throw new NotImplementedException();

        public XHelper2Def(string name, bool nullable)
        {
            Name = name;
            Nullable = nullable;
        }

        public XElement ToXElem(string key, TVal value)
        {
            if (Nullable && value == null) return new XElement(key, new XAttribute("null", true));
            return new XElement(key, ToAttribute(value));
        }

        public TVal FromXElem(XElement value)
        {
            if (Nullable && value.Attributes().Any(a => a.Name == "null" && a.Value.ToLower() == "true")) return default;
            return FromAttribute(value.Value);
        }

        public TVal FromAttributeSafe(string attrval, TVal exceptionValue)
        {
            try
            {
                return FromAttribute(attrval);
            }
            catch (Exception)
            {
                return exceptionValue;
            }
        }

        public TVal FromXElemSafe(XElement value, TVal exceptionValue)
        {
            if (Nullable && value.Attributes().Any(a => a.Name == "null" && a.Value.ToLower() == "true")) return default;
            return FromAttributeSafe(value.Value, exceptionValue);
        }

        public TVal FromChildXElem(XElement value, string childName)
        {
            var child = value.Elements(childName).Single();
            return FromXElem(child);
        }

        public TVal FromChildXElemSafe(XElement value, string childName, TVal exceptionValue)
        {
            var child = value.Elements(childName).FirstOrDefault();
            if (child == null) return exceptionValue; // no child is also an error/exception

            return FromXElemSafe(child, exceptionValue);
        }

        public TVal FromOptionalChildXElem(XElement value, string childName, TVal noChildValue)
        {
            var child = value.Elements(childName).FirstOrDefault();
            if (child == null) return noChildValue;

            return FromXElem(child);
        }

        public TVal FromOptionalChildXElemSafe(XElement value, string childName, TVal noChildValue, TVal exceptionValue)
        {
            var child = value.Elements(childName).FirstOrDefault();
            if (child == null) return noChildValue;

            return FromXElemSafe(child, exceptionValue);
        }
    }
}
