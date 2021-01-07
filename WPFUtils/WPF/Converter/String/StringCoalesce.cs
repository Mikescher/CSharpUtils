using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class StringCoalesce : OneWayConverter<string, string>
	{
		protected override string Convert(string value, object parameter)
		{
			if (string.IsNullOrWhiteSpace(value)) return parameter.ToString();
			return value;
		}
	}
}
