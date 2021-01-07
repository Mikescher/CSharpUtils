using System;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class DTOToDisplay : OneWayConverter<DateTimeOffset, string>
	{
		protected override string Convert(DateTimeOffset value, object parameter)
		{
			return value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
		}
	}
}
