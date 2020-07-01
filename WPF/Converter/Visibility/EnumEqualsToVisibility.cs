using System;
using System.Windows;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class EnumEqualsToVisibility : OneWayConverter<object, Visibility>
	{
		protected override Visibility Convert(object value, object parameter)
		{
			var split = (parameter?.ToString() ?? "").Split(';');

			var eq = string.Equals(value.ToString(), split[0], StringComparison.CurrentCultureIgnoreCase);

			if (split.Length == 1)
			{
				return eq ? Visibility.Visible : Visibility.Hidden;
			}
			else if (split.Length == 3)
			{
				if (eq)
					return (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[1]);
				else
					return (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[2]);
			}

			throw new ArgumentException();
		}
	}
}
