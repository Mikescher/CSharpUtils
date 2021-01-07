using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class StringIsNotWhitespace : OneWayConverter<string, bool>
	{
		protected override bool Convert(string value, object parameter)
		{
			return !string.IsNullOrWhiteSpace(value);
		}
	}
}
