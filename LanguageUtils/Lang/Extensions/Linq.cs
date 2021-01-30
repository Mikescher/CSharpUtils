using System;
using System.Collections.Generic;
using System.Linq;

namespace MSHC.Lang.Extensions
{
    public static class Linq
    {
        public static int? FirstOrDefaultIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            int i = 0;
            foreach (var elem in source)
            {
                if (predicate(elem)) return i;

                i++;
            }
            return null;
        }

		/// <summary>
		/// https://stackoverflow.com/a/2837527/1761622
		/// </summary>
		public static int IndexOf<T>(this IEnumerable<T> source, T item)
		{
			var entry = source.Select((x, i) => new { Value = x, Index = i })
						.Where(x => object.Equals(x.Value, item))
						.FirstOrDefault();
			return entry != null ? entry.Index : -1;
		}

		/// <summary>
		/// https://stackoverflow.com/a/2837527/1761622
		/// </summary>
		public static void CopyTo<T>(this IEnumerable<T> source, T[] array, int startIndex)
		{
			int lowerBound = array.GetLowerBound(0);
			int upperBound = array.GetUpperBound(0);
			if (startIndex < lowerBound)
				throw new ArgumentOutOfRangeException("startIndex", "The start index must be greater than or equal to the array lower bound");
			if (startIndex > upperBound)
				throw new ArgumentOutOfRangeException("startIndex", "The start index must be less than or equal to the array upper bound");

			int i = 0;
			foreach (var item in source)
			{
				if (startIndex + i > upperBound)
					throw new ArgumentException("The array capacity is insufficient to copy all items from the source sequence");
				array[startIndex + i] = item;
				i++;
			}
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		public static IEnumerable<T> SkipLastN<T>(this IEnumerable<T> source, int n)
		{
			using (var it = source.GetEnumerator())
			{
				bool hasRemainingItems = false;
				var cache = new Queue<T>(n + 1);

				do
				{
					// ReSharper disable once AssignmentInConditionalExpression
					if (hasRemainingItems = it.MoveNext())
					{
						cache.Enqueue(it.Current);
						if (cache.Count > n)
							yield return cache.Dequeue();
					}
				} while (hasRemainingItems);
			}
		}

		public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			int retVal = 0;
			foreach (var item in items)
			{
				if (predicate(item)) return retVal;
				retVal++;
			}
			return -1;
		}

        public static int FindLastIndex<TItem>(this IEnumerable<TItem> self, Func<TItem, bool> cond)
        {
            var result = -1;
            var i = 0;
            foreach (var item in self)
            {
                if (cond.Invoke(item)) result = i;
                i++;
            }
            return result;
        }
    }
}
