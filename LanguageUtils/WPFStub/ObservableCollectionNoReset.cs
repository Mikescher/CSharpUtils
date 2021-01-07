﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MSHC.WPF.MVVM
{
	public class ObservableCollectionNoReset<T> : ObservableCollection<T>
	{
		// Some CollectionChanged listeners don't support range actions.
		public bool RangeActionsSupported { get; set; }

		protected override void ClearItems()
		{
			if (RangeActionsSupported)
			{
				List<T> removed = new List<T>(this);
				base.ClearItems();
				base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
			}
			else
			{
				while (Count > 0) base.RemoveAt(Count - 1);
			}
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (e.Action != NotifyCollectionChangedAction.Reset) base.OnCollectionChanged(e);
		}

		public ObservableCollectionNoReset(bool rangeActionsSupported = false)
		{
			RangeActionsSupported = rangeActionsSupported;
		}

		public ObservableCollectionNoReset(IEnumerable<T> data)
		{
			RangeActionsSupported = false;
			foreach (var d in data) Add(d);
		}
	}
}
