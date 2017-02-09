using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MSHC.Lang.Extensions
{
	public static class ObservableCollectionExtension
	{
		public static void Synchronize<T>(this ObservableCollection<T> target, List<T> source)
		{
			foreach (var v in target.Except(source))
			{
				target.Remove(v);
			}

			foreach (var v in source.Except(target))
			{
				target.Add(v);
			}
		}
	}
}
