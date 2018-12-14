using System;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class SmartDateTimeToDisplay : OneWayConverter<DateTimeOffset, string>
	{
		protected override string Convert(DateTimeOffset value, object parameter)
		{
			var local = value.ToLocalTime();
			var now = System.DateTime.Now;

			if (local.DayOfYear == now.DayOfYear && local.Year == now.Year) 
				return local.ToString("HH:mm:ss");
			else
				return local.ToString("yyyy-MM-dd");
		}
	}
}
