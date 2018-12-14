using System.Windows.Data;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	[ValueConversion(typeof(bool), typeof(System.Windows.Visibility))]
	public class InvertedBoolToVisibility : OneWayConverter<bool, System.Windows.Visibility>
	{
		protected override System.Windows.Visibility Convert(bool value, object parameter) => value ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
	}
}
