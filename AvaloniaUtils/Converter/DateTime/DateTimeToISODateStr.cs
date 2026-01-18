using System;

namespace MSHC.Avalonia.Converter;

/// <summary>
/// Converts a DateTime to ISO date string format (yyyy-MM-dd).
/// </summary>
public class DateTimeToISODateStr : OneWayConverter<DateTime, string>
{
    protected override string Convert(DateTime value, object? parameter) => value.ToString("yyyy-MM-dd");
}
