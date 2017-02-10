using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MSHC.Lang.Extensions
{
	public static class ObservableCollectionExtension
	{
		public static void Synchronize<T>(this ObservableCollection<T> target, IEnumerable<T> esource)
		{
			var source = esource.ToList();

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
