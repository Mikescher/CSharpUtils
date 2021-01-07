using System;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class EnumValueToBoolean : OneWayConverter<object, bool>
	{
		protected override bool Convert(object value, object parameter)
		{
			return System.String.Equals(value.ToString(), (parameter ?? "").ToString(), StringComparison.CurrentCultureIgnoreCase);
		}
	}
}
