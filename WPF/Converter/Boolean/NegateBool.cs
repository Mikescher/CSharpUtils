using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class NegateBool : OneWayConverter<bool, bool>
	{
		protected override bool Convert(bool value, object parameter) => !value;
	}
}
