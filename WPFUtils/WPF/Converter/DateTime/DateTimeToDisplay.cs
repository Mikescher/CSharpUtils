using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class DateTimeToDisplay : OneWayConverter<System.DateTime, string>
	{
		protected override string Convert(System.DateTime value, object parameter)
		{
			return value.ToString("yyyy-MM-dd HH:mm:ss");
		}
	}
}
