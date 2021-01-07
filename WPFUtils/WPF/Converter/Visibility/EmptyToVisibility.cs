using System;
using System.Windows;
using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class EmptyToVisibility : OneWayConverter<string, Visibility>
	{
		protected override Visibility Convert(string value, object parameter)
		{
			if (string.IsNullOrWhiteSpace(parameter?.ToString()))
			{
				return string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Hidden;
			}

			if (string.IsNullOrEmpty(value))
				return (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[0]);
			return (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString().Split(';')[1]);

		}
	}
}
