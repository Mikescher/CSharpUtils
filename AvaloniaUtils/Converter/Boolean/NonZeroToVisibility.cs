namespace MSHC.Avalonia.Converter;

/// <summary>
/// Converts a non-zero integer to visible (true), zero to hidden (false).
/// </summary>
public class NonZeroToVisibility : OneWayConverter<int, bool>
{
    protected override bool Convert(int value, object? parameter)
    {
        return value != 0;
    }
}
