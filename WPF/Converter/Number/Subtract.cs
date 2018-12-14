using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter.Numbers
{
	class Subtract : OneWayConverter<double, double>
	{
		protected override double Convert(double value, object parameter)
		{
			if (!int.TryParse(System.Convert.ToString(parameter), out var delta)) delta = 0;

			return value - delta;
		}
	}
}
