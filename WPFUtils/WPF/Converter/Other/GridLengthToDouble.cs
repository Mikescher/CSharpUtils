using System;
using System.Windows;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class GridLengthToDouble : OneWayConverter<GridLength, double>
	{
		protected override double Convert(GridLength value, object parameter)
		{
			if (!int.TryParse(System.Convert.ToString(parameter), out var delta)) delta = 0;

			if (! value.IsAbsolute) throw new Exception("GridLength must be absolute");

			return value.Value + delta;
		}
	}
}
