using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class TwoWayStringNullCoalesce : TwoWayConverter<string, string>
	{
		protected override string Convert(string value, object parameter)
		{
			return value ?? parameter?.ToString() ?? "[[**NULL**]]";
		}

		protected override string ConvertBack(string value, object parameter)
		{
			return (value == (parameter?.ToString() ?? "[[**NULL**]]") ) ? null : value;
		}
	}
}
