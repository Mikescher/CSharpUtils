﻿using System;

namespace MSHC.WPF.MVVM
{
	public abstract class OneWayConverter<TSource, TTarget> : TwoWayConverter<TSource, TTarget>
	{
		protected override TSource ConvertBack(TTarget value, object parameter)
		{
			throw new NotSupportedException();
		}
	}
}
