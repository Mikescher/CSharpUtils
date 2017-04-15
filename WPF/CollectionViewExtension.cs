using System.ComponentModel;

namespace MSHC.WPF
{
	public static class CollectionViewExtension
	{
		public static T FirstOrDefault<T>(this ICollectionView view)
		{
			var enumerator = view.GetEnumerator();
			if (enumerator.MoveNext())
				return (T)enumerator.Current;
			else
				return default(T);
		}
	}
}
