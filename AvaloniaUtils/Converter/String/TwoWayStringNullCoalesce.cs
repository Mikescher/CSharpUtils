namespace MSHC.Avalonia.Converter;

/// <summary>
/// Two-way converter that handles null string values with a fallback.
/// </summary>
public class TwoWayStringNullCoalesce : TwoWayConverter<string, string>
{
    protected override string Convert(string value, object? parameter)
    {
        return value ?? parameter?.ToString() ?? "[[**NULL**]]";
    }

    protected override string ConvertBack(string value, object? parameter)
    {
        return value == (parameter?.ToString() ?? "[[**NULL**]]") ? null! : value;
    }
}
