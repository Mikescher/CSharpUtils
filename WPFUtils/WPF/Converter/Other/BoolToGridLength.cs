using System.Windows;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class BoolToGridLength : OneWayConverter<bool, GridLength>
	{
		private static readonly GridLengthConverter _conv = new GridLengthConverter();

		protected override GridLength Convert(bool value, object parameter)
		{
			if (value) return (GridLength)_conv.ConvertFromString(parameter.ToString().Split(';')[0]);
			else       return (GridLength)_conv.ConvertFromString(parameter.ToString().Split(';')[1]);
		}
	}
}
