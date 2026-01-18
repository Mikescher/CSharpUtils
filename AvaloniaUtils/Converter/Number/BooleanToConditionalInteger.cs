namespace MSHC.Avalonia.Converter;

/// <summary>
/// Converts a bool to one of two integers.
/// Parameter format: "trueValue;falseValue"
/// </summary>
public class BooleanToConditionalInteger : OneWayConverter<bool, int>
{
    protected override int Convert(bool value, object? parameter)
    {
        var split = parameter?.ToString()?.Split(';') ?? ["0", "0"];
        return value ? int.Parse(split[0]) : int.Parse(split[1]);
    }
}
