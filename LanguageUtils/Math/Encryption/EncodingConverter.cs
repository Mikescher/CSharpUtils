using System;
using System.Collections.Generic;
using System.Linq;

namespace MSHC.Math.Encryption
{
	public static class EncodingConverter
	{
		/// <summary>
		/// @source: http://stackoverflow.com/a/14333437/1761622
		/// </summary>
		public static string ByteToHexBitFiddleUppercase(byte[] bytes)
		{
			char[] c = new char[bytes.Length * 2];
			int b;
			for (int i = 0; i < bytes.Length; i++)
			{
				b = bytes[i] >> 4;
				c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
				b = bytes[i] & 0xF;
				c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
			}
			return new string(c);
		}

		/// <summary>
		/// @source: http://stackoverflow.com/a/14333437/1761622
		/// </summary>
		public static string ByteToHexBitFiddleLowercase(byte[] bytes)
		{
			char[] c = new char[bytes.Length * 2];
			int b;
			for (int i = 0; i < bytes.Length; i++)
			{
				b = bytes[i] >> 4;
				c[i * 2] = (char)(87 + b + (((b - 10) >> 31) & -39));
				b = bytes[i] & 0xF;
				c[i * 2 + 1] = (char)(87 + b + (((b - 10) >> 31) & -39));
			}
			return new string(c);
		}

		/// <summary>
		/// @source: http://stackoverflow.com/a/9995303/1761622
		/// </summary>
		public static byte[] StringToByteArrayCaseInsensitive(string hex)
		{
			if (hex.Length % 2 == 1) throw new Exception("The binary key cannot have an odd number of digits");

			byte[] arr = new byte[hex.Length >> 1];

			for (int i = 0; i < (hex.Length >> 1); ++i)
			{
				arr[i] = (byte)((GetHexValCaseInsensitive(hex[i << 1]) << 4) + GetHexValCaseInsensitive(hex[(i << 1) + 1]));
			}

			return arr;
		}

		public static int GetHexValCaseInsensitive(char hex)
		{
			return hex - (hex < 58 ? 48 : (hex < 97 ? 55 : 87));
		}

		/// <summary>
		/// @source: http://stackoverflow.com/a/9995303/1761622
		/// </summary>
		public static byte[] StringToByteArrayCaseUppercase(string hex)
		{
			if (hex.Length % 2 == 1) throw new Exception("The binary key cannot have an odd number of digits");

			byte[] arr = new byte[hex.Length >> 1];

			for (int i = 0; i < (hex.Length >> 1); ++i)
			{
				arr[i] = (byte)((GetHexValUppercase(hex[i << 1]) << 4) + GetHexValUppercase(hex[(i << 1) + 1]));
			}

			return arr;
		}

		public static int GetHexValUppercase(char hex)
		{
			return hex - (hex < 58 ? 48 : 55);
		}

		/// <summary>
		/// @source: http://stackoverflow.com/a/9995303/1761622
		/// </summary>
		public static byte[] StringToByteArrayCaseLowercase(string hex)
		{
			if (hex.Length % 2 == 1) throw new Exception("The binary key cannot have an odd number of digits");

			byte[] arr = new byte[hex.Length >> 1];

			for (int i = 0; i < (hex.Length >> 1); ++i)
			{
				arr[i] = (byte)((GetHexValLowercase(hex[i << 1]) << 4) + GetHexValLowercase(hex[(i << 1) + 1]));
			}

			return arr;
		}

		public static int GetHexValLowercase(char hex)
		{
			return hex - (hex < 58 ? 48 : 87);
		}

		public static string ByteArrayToHexDump(IList<byte> b, string bytesep = "", int linelen = -1, string nullValue = null, bool upper = false)
		{
			if (b == null) return nullValue;

			string ToHex(byte p) => (upper ? (Convert.ToString(p, 16).PadLeft(2, '0').ToUpper()) : (Convert.ToString(p, 16).PadLeft(2, '0')));

			if (linelen <= 0) return string.Join(bytesep, b.Select(ToHex));

			return string.Join(Environment.NewLine, Enumerable
				.Range(0, (int)System.Math.Ceiling(b.Count/(linelen*1d)))
				.Select(i1 => Enumerable.Range(i1*linelen, System.Math.Min(b.Count, i1*linelen+linelen)-i1*linelen))
				.Select(rr => string.Join(bytesep, rr.Select(idx => ToHex(b[idx])))));
		}
	}
}
