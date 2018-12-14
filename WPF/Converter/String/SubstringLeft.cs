using MSHC.WPF.MVVM;

namespace MSHC.WPF.Converter
{
	public class SubstringLeft : OneWayConverter<string, string>
	{
		protected override string Convert(string value, object parameter)
		{
			if (!int.TryParse(System.Convert.ToString(parameter), out var size)) return "";

			var v = (value ?? "").Replace("\r", "").Replace("\n", " ");

			return v.Substring(0, System.Math.Min(size, v.Length));
		}
	}
}
