using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class GreaterZero : OneWayConverter<int, bool>
	{
		protected override bool Convert(int value, object parameter) => (value>0);
	}
}
