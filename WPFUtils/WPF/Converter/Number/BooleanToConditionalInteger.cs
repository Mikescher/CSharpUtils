using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class BooleanToConditionalInteger : OneWayConverter<bool, int>
	{
		protected override int Convert(bool value, object parameter)
		{
			var split = parameter.ToString().Split(';');

			return value ? int.Parse(split[0]) : int.Parse(split[1]);
		}
	}
}
