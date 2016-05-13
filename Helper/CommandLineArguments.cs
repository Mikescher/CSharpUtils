using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace MSHC.Helper
{
	public class CommandLineArguments
	{
		private StringDictionary Parameters;

		public CommandLineArguments(string[] Args)
		{
			Parameters = new StringDictionary();

			Regex rexSplitter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex rexRemover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string parameter = null;

			foreach (string Txt in Args)
			{
				var parts = rexSplitter.Split(Txt, 3);

				switch (parts.Length)
				{
					case 1:
						if (parameter != null)
						{
							if (!Parameters.ContainsKey(parameter))
							{
								parts[0] = rexRemover.Replace(parts[0], "$1");
								Parameters.Add(parameter, parts[0]);
							}
							parameter = null;
						}
						break;

					case 2:
						if (parameter != null)
						{
							if (!Parameters.ContainsKey(parameter))
								Parameters.Add(parameter, "true");
						}
						parameter = parts[1];
						break;

					case 3:
						if (parameter != null)
						{
							if (!Parameters.ContainsKey(parameter))
								Parameters.Add(parameter, "true");
						}

						parameter = parts[1];

						if (!Parameters.ContainsKey(parameter))
						{
							parts[2] = rexRemover.Replace(parts[2], "$1");
							Parameters.Add(parameter, parts[2]);
						}

						parameter = null;
						break;
				}
			}
			if (parameter != null)
			{
				if (!Parameters.ContainsKey(parameter))
					Parameters.Add(parameter, "true");
			}
		}

		public bool Contains(string key)
		{
			return Parameters.ContainsKey(key);
		}

		public bool IsSet(string key)
		{
			return Parameters.ContainsKey(key) && Parameters[key] != null;
		}

		public string this[string Param]
		{
			get
			{
				return (Parameters[Param]);
			}
		}

		public bool isEmpty()
		{
			return Parameters.Count == 0;
		}

		#region String

		public string GetStringDefault(string p, string def)
		{
			return Contains(p) ? this[p] : def;
		}

		public List<String> GetStringList(string p, string delimiter, StringSplitOptions options = StringSplitOptions.None)
		{
			if (Contains(p))
				return this[p].Split(new string[] { delimiter }, options).ToList();
			else
				return null;
		}

		#endregion

		#region Long

		public bool IsLong(string p)
		{
			long a;
			return IsSet(p) && long.TryParse(Parameters[p], out a);
		}

		public long GetLong(string p)
		{
			return long.Parse(this[p]);
		}

		public long GetLongDefault(string p, long def)
		{
			return IsLong(p) ? GetLong(p) : def;
		}

		public long? GetLongDefaultNull(string p)
		{
			return IsLong(p) ? GetLong(p) : (long?)null;
		}

		public long GetLongDefaultRange(string p, long def, long min, long max)
		{
			return Math.Min(max - 1, Math.Max(min, (IsLong(p) ? GetLong(p) : def)));
		}

		public List<long> GetLongList(string p, string delimiter, bool sanitize = false)
		{
			List<String> ls = GetStringList(p, delimiter, sanitize ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

			long aout;
			if (ls.Any(pp => !long.TryParse(pp, out aout)))
				return null;

			return ls.Select(long.Parse).ToList();
		}

		#endregion

		#region Integer

		public bool IsInt(string p)
		{
			int a;
			return IsSet(p) && int.TryParse(Parameters[p], out a);
		}

		public int GetInt(string p)
		{
			return int.Parse(this[p]);
		}

		public int GetIntDefault(string p, int def)
		{
			return IsInt(p) ? GetInt(p) : def;
		}

		public int? GetIntDefaultNull(string p)
		{
			return IsInt(p) ? GetInt(p) : (int?)null;
		}

		public int GetIntDefaultRange(string p, int def, int min, int max)
		{
			return Math.Min(max - 1, Math.Max(min, (IsInt(p) ? GetInt(p) : def)));
		}

		public List<int> GetIntList(string p, string delimiter, bool sanitize = false)
		{
			List<String> ls = GetStringList(p, delimiter, sanitize ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

			int aout;
			if (ls.Any(pp => !int.TryParse(pp, out aout)))
				return null;

			return ls.Select(int.Parse).ToList();
		}

		#endregion

		#region UInteger

		public bool IsUInt(string p)
		{
			uint a;
			return IsSet(p) && uint.TryParse(Parameters[p], out a);
		}

		public uint GetUInt(string p)
		{
			return uint.Parse(this[p]);
		}

		public uint GetUIntDefault(string p, uint def)
		{
			return IsUInt(p) ? GetUInt(p) : def;
		}

		public uint? GetUIntDefaultNull(string p)
		{
			return IsUInt(p) ? GetUInt(p) : (uint?)null;
		}

		public uint GetUIntDefaultRange(string p, uint def, uint min, uint max)
		{
			return Math.Min(max - 1, Math.Max(min, (IsUInt(p) ? GetUInt(p) : def)));
		}

		public List<uint> GetUIntList(string p, string delimiter, bool sanitize = false)
		{
			List<String> ls = GetStringList(p, delimiter, sanitize ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

			uint aout;
			if (ls.Any(pp => !uint.TryParse(pp, out aout)))
				return null;

			return ls.Select(uint.Parse).ToList();
		}

		#endregion

		#region Enum

		private bool TryParseEnum<T>(string input, out T value) where T : struct, IConvertible
		{
			T result;
			if (Enum.TryParse(input, true, out result))
			{
				value = result;
				return true;
			}

			int resultOrd;
			if (int.TryParse(input, out resultOrd))
			{
				value =(T)Enum.ToObject(typeof(T), resultOrd);
				return true;
			}

			value = default(T);
			return false;
		}

		private T ParseEnum<T>(string p) where T : struct, IConvertible
		{
			T value;
			if (TryParseEnum(p, out value))
				return value;

			throw new FormatException(string.Format("The value {0} is not a valid enum member", p));
		}

		public bool IsEnum<T>(string p) where T : struct, IConvertible
		{
			if (!IsSet(p)) return false;

			T tmp;
			return TryParseEnum(p, out tmp);
		}

		public T GetEnum<T>(string p) where T : struct, IConvertible
		{
			T value;
			if (TryParseEnum(p, out value))
				return value;

			throw new FormatException(string.Format("The parameter {0} is not a valid enum value", p));
		}

		public T GetEnumDefault<T>(string p, T def) where T : struct, IConvertible
		{
			T value;
			if (TryParseEnum(p, out value))
				return value;
			else
				return def;
		}

		public List<T> GetEnumList<T>(string p, string delimiter, bool sanitize = false) where T : struct, IConvertible
		{
			var ls = GetStringList(p, delimiter, sanitize ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

			T aout;
			if (ls.Any(pp => !TryParseEnum(pp, out aout)))
				return null;

			return ls.Select(ParseEnum<T>).ToList();
		}

		#endregion

	}
}
