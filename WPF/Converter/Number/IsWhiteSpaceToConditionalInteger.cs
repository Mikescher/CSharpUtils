using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class IsWhiteSpaceToConditionalInteger : OneWayConverter<string, int>
	{
		protected override int Convert(string value, object parameter)
		{
			var split = parameter.ToString().Split(';');

			return string.IsNullOrWhiteSpace(value) ? int.Parse(split[0]) : int.Parse(split[1]);
		}
	}
}
