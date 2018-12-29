using System;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class DateTimeToISODateStr : OneWayConverter<DateTime, string>
	{
		protected override string Convert(DateTime value, object parameter) => value.ToString("yyyy-MM-dd");
	}
}
