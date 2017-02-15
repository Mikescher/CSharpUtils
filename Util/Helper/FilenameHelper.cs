using System.Linq;
using System.Text;

namespace MSHC.Util.Helper
{
	public static class FilenameHelper
	{
		private const char CONVERT_ESCAPE_CHARACTER = '%';
		private const string ALLOWED_CHARACTER = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~#+-_.,;%& {}()=ÄÖÜäöüµ@";

		public static string ConvertStringForFilename(string input)
		{
			StringBuilder output = new StringBuilder(input.Length);

			foreach (var c in input)
			{
				if (c == CONVERT_ESCAPE_CHARACTER)
				{
					output.Append(c);
					output.Append(string.Format("{0:X4}", (int)c));
				}
				else if (ALLOWED_CHARACTER.Contains(c))
				{
					output.Append(c);
				}
				else
				{
					output.Append(c);
					output.Append(string.Format("{0:X4}", (int)c));
				}
			}

			return output.ToString();
		}
	}
}
