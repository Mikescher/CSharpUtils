using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class TodayDateTimeToDisplay : OneWayConverter<System.DateTime, string>
	{
		protected override string Convert(System.DateTime value, object parameter)
		{
			var local = value.ToLocalTime();

			return local.ToString("HH:mm:ss");
		}
	}
}
