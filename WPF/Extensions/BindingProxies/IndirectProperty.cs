using System;
using System.Reflection;
using System.Windows;
using MSHC.Lang.Reflection;

namespace MSHC.WPF.Extensions.BindingProxies
{
	public sealed class IndirectProperty<TType>
	{
		private readonly object _element;
		private readonly PropertyInfo _info;

		private IndirectProperty(object element, PropertyInfo info)
		{
			_element = element;
			_info = info;
		}

		public TType Get()
		{
			return (TType) _info.GetValue(_element, null);
		}

		public void Set(TType v)
		{
			_info.SetValue(_element, v);
		}

		public static IndirectProperty<TType> Create(FrameworkElement element, string propertyPath)
		{
			try
			{
				var path = ReflectionPathResolver.GetProperty(element, propertyPath);
				return new IndirectProperty<TType>(path.Item1, path.Item2);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
