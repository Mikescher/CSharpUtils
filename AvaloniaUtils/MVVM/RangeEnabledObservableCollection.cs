using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MSHC.Avalonia.MVVM;

/// <summary>
/// An ObservableCollection that supports bulk operations without raising events for each item.
/// </summary>
public class RangeEnabledObservableCollection<T> : ObservableCollection<T>
{
    public void AddRange(IEnumerable<T> items)
    {
        CheckReentrancy();
        foreach (var item in items)
            Items.Add(item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
    }

    public void ReplaceAll(IEnumerable<T> items)
    {
        CheckReentrancy();
        Items.Clear();
        foreach (var item in items)
            Items.Add(item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
    }
}
