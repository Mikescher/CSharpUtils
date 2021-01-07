using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class IsNull : OneWayConverter<object, bool>
	{
		protected override bool Convert(object value, object parameter)
		{
			return (value == null);
		}
	}
}
