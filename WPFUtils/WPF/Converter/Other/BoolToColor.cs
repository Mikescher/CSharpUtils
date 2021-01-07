using System.Windows.Media;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class BoolToColor : OneWayConverter<bool, Brush>
	{
		private static readonly BrushConverter _conv = new BrushConverter();

		protected override Brush Convert(bool value, object parameter)
		{
			if (string.IsNullOrWhiteSpace(parameter?.ToString()))
			{
				return Brushes.Black;
			}
			else
			{
				if (value)
					return (Brush)_conv.ConvertFromString(parameter.ToString().Split(';')[0]);
				else
					return (Brush)_conv.ConvertFromString(parameter.ToString().Split(';')[1]);
			}

		}
	}
}
