using Avalonia.Media;

namespace MSHC.Avalonia.Converter;

/// <summary>
/// Converts a bool to one of two brushes.
/// Parameter format: "TrueColor;FalseColor" (e.g., "Black;Red")
/// </summary>
public class BoolToBrushSwitch : OneWayConverter<bool, IBrush>
{
    protected override IBrush Convert(bool value, object? parameter)
    {
        var split = parameter?.ToString()?.Split(';') ?? ["Black", "Black"];
        var colorName = value ? split[0] : split[1];

        return Color.TryParse(colorName, out var color)
            ? new SolidColorBrush(color)
            : Brushes.Black;
    }
}
