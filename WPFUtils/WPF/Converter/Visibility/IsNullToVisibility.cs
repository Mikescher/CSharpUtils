using System;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class IsNullToVisibility : OneWayConverter<object, System.Windows.Visibility>
	{
		protected override System.Windows.Visibility Convert(object value, object parameter)
		{
			if (value == null)
				return (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), parameter.ToString().Split(';')[0]);
			else
				return (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), parameter.ToString().Split(';')[1]);
		}
	}
}
