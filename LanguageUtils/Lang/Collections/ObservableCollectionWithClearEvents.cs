using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MSHC.Lang.Collections
{
	/// <summary>
	/// Sends many [[Remove]] Events instead of a single [[Clear]] event on this.Clear()
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObservableCollectionWithClearEvents<T> : ObservableCollection<T>
	{
		public ObservableCollectionWithClearEvents() : base() { }

		public ObservableCollectionWithClearEvents(List<T> collection) : base(collection) { }

		public ObservableCollectionWithClearEvents(IEnumerable<T> collection) : base(collection) { }

		public new void Clear()
		{
			lock (((ICollection)Items).SyncRoot)
			{
				for (int i = this.Items.Count - 1; i >= 0; i--)
				{
					var item = Items[i];

					Items.RemoveAt(i);
					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, i));
				}
			}
		}
	}
}
