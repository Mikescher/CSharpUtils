namespace MSHC.Avalonia.Converter;

/// <summary>
/// Converts null/non-null to visibility bool.
/// Parameter format: "True;False" - first value when null, second when not null.
/// </summary>
public class IsNullToVisibility : OneWayConverter<object, bool>
{
    protected override bool Convert(object value, object? parameter)
    {
        if (parameter == null)
        {
            return value == null;
        }

        var parts = parameter.ToString()!.Split(';');
        return value == null ? bool.Parse(parts[0]) : bool.Parse(parts[1]);
    }
}
