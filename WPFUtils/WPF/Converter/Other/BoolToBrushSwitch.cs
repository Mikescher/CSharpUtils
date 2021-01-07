using System.Windows.Media;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class BoolToBrushSwitch : OneWayConverter<bool, Brush>
	{
		private static readonly BrushConverter _conv = new BrushConverter();

		protected override Brush Convert(bool value, object parameter)
		{
			var split = parameter.ToString().Split(';');

			return (Brush)_conv.ConvertFromString(split[value?0:1]);
		}
	}
}
