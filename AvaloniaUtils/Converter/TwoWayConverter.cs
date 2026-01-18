using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MSHC.Avalonia.Converter;

/// <summary>
/// Base class for two-way value converters with strongly-typed source and target types.
/// </summary>
public abstract class TwoWayConverter<TSource, TTarget> : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TSource)
        {
            if (value == null && typeof(TSource).IsClass)
                return Convert((TSource)value!, parameter);

            return AvaloniaProperty.UnsetValue;
        }

        return Convert((TSource)value, parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TTarget)
        {
            if (value == null && typeof(TTarget).IsClass)
                return ConvertBack((TTarget)value!, parameter);

            return AvaloniaProperty.UnsetValue;
        }

        return ConvertBack((TTarget)value, parameter);
    }

    protected abstract TTarget Convert(TSource value, object? parameter);
    protected abstract TSource ConvertBack(TTarget value, object? parameter);
}
