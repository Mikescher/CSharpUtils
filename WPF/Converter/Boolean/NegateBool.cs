using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class NegateBool : TwoWayConverter<bool, bool>
	{
		protected override bool Convert(bool value, object parameter) => !value;
		protected override bool ConvertBack(bool value, object parameter) => !value;
	}
}
