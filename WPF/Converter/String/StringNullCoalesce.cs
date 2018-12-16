using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class StringNullCoalesce : OneWayConverter<string, string>
	{
		protected override string Convert(string value, object parameter)
		{
			return value ?? parameter?.ToString() ?? "[[**NULL**]]";
		}
	}
}
