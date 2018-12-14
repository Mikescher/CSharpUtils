using System.Text.RegularExpressions;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class TextToLines : OneWayConverter<string, int>
	{
		protected override int Convert(string value, object parameter)
		{
			return Regex.Split(value, @"\r?\n").Length;
		}
	}
}
