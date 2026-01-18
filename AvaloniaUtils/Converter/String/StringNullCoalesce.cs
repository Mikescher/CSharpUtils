namespace MSHC.Avalonia.Converter;

/// <summary>
/// Returns the string value, or the parameter value if null.
/// </summary>
public class StringNullCoalesce : OneWayConverter<string, string>
{
    protected override string Convert(string value, object? parameter)
    {
        return value ?? parameter?.ToString() ?? "[[**NULL**]]";
    }
}
