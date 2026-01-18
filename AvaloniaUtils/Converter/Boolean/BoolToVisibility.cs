namespace MSHC.Avalonia.Converter;

/// <summary>
/// Converts a bool to a visibility bool (for IsVisible property).
/// Parameter format: "True;False" - first value when true, second when false.
/// Default: true = visible, false = hidden (invisible).
/// </summary>
public class BoolToVisibility : OneWayConverter<bool, bool>
{
    protected override bool Convert(bool value, object? parameter)
    {
        if (string.IsNullOrWhiteSpace(parameter?.ToString()))
        {
            return value;
        }

        var parts = parameter.ToString()!.Split(';');
        return value ? bool.Parse(parts[0]) : bool.Parse(parts[1]);
    }
}
