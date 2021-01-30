﻿using System.Collections.Generic;
using System.Linq;
using MSHC.Lang.Collections;
using MSHC.Lang.Extensions;

namespace MSHC.Lang.Weak
{
	/// <summary>
	/// https://stackoverflow.com/a/2837527/1761622
	/// </summary>
	public class WeakList<T> : IList<T>
	{
		private List<TypedWeakReference<T>> _innerList = new List<TypedWeakReference<T>>();

		public int IndexOf(T item)
		{
			return _innerList.Select(wr => wr.Target).IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			_innerList.Insert(index, new TypedWeakReference<T>(item));
		}

		public void RemoveAt(int index)
		{
			_innerList.RemoveAt(index);
		}

		public T this[int index]
		{
			get => _innerList[index].Target;
			set => _innerList[index] = new TypedWeakReference<T>(value);
		}

		public void Add(T item)
		{
			_innerList.Add(new TypedWeakReference<T>(item));
		}

		public void Clear()
		{
			_innerList.Clear();
		}

		public bool Contains(T item)
		{
			return _innerList.Any(wr => object.Equals(wr.Target, item));
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_innerList.Select(wr => wr.Target).CopyTo(array, arrayIndex);
		}

		public int Count => _innerList.Count;

		public bool IsReadOnly => false;

		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index > -1)
			{
				RemoveAt(index);
				return true;
			}
			return false;
		}
		public IEnumerator<T> GetEnumerator()
		{
			return _innerList.Select(x => x.Target).Where(p=>p!=null).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Purge()
		{
			_innerList.RemoveAll(wr => !wr.IsAlive);
		}
	}	
}
