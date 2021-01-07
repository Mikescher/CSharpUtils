using System;
using System.Windows;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class IntegerGreaterToVisibility : OneWayConverter<int, Visibility>
	{
		protected override Visibility Convert(int value, object parameter)
		{
			var v  = int.Parse(parameter.ToString().Split(';')[0]);
			var p1 = (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[1]);
			var p2 = (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[2]);

			return (value > v) ? p1 : p2;

		}
	}
}
