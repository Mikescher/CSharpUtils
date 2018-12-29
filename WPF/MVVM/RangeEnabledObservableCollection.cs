using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MSHC.WPF.MVVM
{
	public class RangeEnabledObservableCollection<T> : ObservableCollection<T>
	{
		public void InsertRange(IEnumerable<T> items) 
		{
			this.CheckReentrancy();
			foreach(var item in items) this.Items.Add(item);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Count")); 
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
		}
	}
}
