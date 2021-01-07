using System;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class SmallDateTimeToDisplay : OneWayConverter<DateTimeOffset, string>
	{
		private static readonly string[] MONTH_LIST = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};

		protected override string Convert(DateTimeOffset value, object parameter)
		{
			var local = value.ToLocalTime();
			var now = System.DateTime.Now;

			if (local.DayOfYear == now.DayOfYear && local.Year == now.Year)
			{
				return local.ToString("HH:mm");
			}
			else
			{
				return MONTH_LIST[local.Month - 1] + " " + local.Day;
			}
		}
	}
}
