using System.Windows;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class NonZeroToVisibility : OneWayConverter<int, Visibility>
	{
		protected override Visibility Convert(int v, object parameter)
		{
			return (v != 0) ? Visibility.Visible : Visibility.Hidden;
		}
	}
}
