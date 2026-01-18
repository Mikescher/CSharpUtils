namespace MSHC.Avalonia.Converter;

/// <summary>
/// Returns true if the integer value is greater than zero.
/// </summary>
public class GreaterZero : OneWayConverter<int, bool>
{
    protected override bool Convert(int value, object? parameter) => value > 0;
}
