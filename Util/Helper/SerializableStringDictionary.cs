using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MSHC.Util.Helper
{
	[Serializable]
	[XmlRoot("dictionary")]
	public class SerializableStringDictionary<TValue> : Dictionary<string, TValue>, IXmlSerializable
	{
		private static readonly XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

		public SerializableStringDictionary() { }

		protected SerializableStringDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			var wasEmpty = reader.IsEmptyElement;

			reader.Read();
			if (wasEmpty)
			{
				return;
			}

			try
			{
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					ReadItem(reader);
					reader.MoveToContent();
				}
			}
			finally
			{
				reader.ReadEndElement();
			}
		}

		private void ReadItem(XmlReader reader)
		{
			try
			{
				string key = reader.GetAttribute("key");

				reader.ReadStartElement("item");
				reader.MoveToElement();
				var value = (TValue)valueSerializer.Deserialize(reader);

				if (key != null)
					Add(key, value);
			}
			finally
			{
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			foreach (var keyValuePair in this)
			{
				WriteItem(writer, keyValuePair);
			}
		}

		private void WriteItem(XmlWriter writer, KeyValuePair<string, TValue> keyValuePair)
		{
			writer.WriteStartElement("item");
			writer.WriteAttributeString("key", keyValuePair.Key);

			try
			{
				valueSerializer.Serialize(writer, keyValuePair.Value);
			}
			finally
			{
				writer.WriteEndElement();
			}
		}

		public TValue GetValueOrDefault(string key)
		{
			TValue v;

			if (TryGetValue(key, out v))
			{
				return v;
			}

			return default(TValue);
		}

		public TValue GetValueOrDefault(string key, TValue defValue)
		{
			TValue v;

			if (TryGetValue(key, out v))
			{
				return v;
			}

			return defValue;
		}
	}
}
