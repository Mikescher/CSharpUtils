using System.Windows;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class BoolToFontWeight : OneWayConverter<bool, FontWeight>
	{
		protected override FontWeight Convert(bool value, object parameter)
		{
			if (value)
				return FontWeights.Bold;
			else
				return FontWeights.Normal;
		}
	}
}
