using System;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class DateOnlyToDisplay : OneWayConverter<DateTimeOffset, string>
	{
		protected override string Convert(DateTimeOffset value, object parameter)
		{
			var local = value.ToLocalTime();

			return local.ToLocalTime().ToString("yyyy-MM-dd");
		}
	}
}
