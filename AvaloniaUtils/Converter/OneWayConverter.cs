using System;

namespace MSHC.Avalonia.Converter;

/// <summary>
/// Base class for one-way value converters with strongly-typed source and target types.
/// </summary>
public abstract class OneWayConverter<TSource, TTarget> : TwoWayConverter<TSource, TTarget>
{
    protected override TSource ConvertBack(TTarget value, object? parameter)
    {
        throw new NotSupportedException();
    }
}
