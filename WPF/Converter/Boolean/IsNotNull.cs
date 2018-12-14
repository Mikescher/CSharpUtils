using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class IsNotNull : OneWayConverter<object, bool>
	{
		protected override bool Convert(object value, object parameter)
		{
			return (value != null);
		}
	}
}
