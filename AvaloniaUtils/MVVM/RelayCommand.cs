using System.Windows.Input;

namespace MSHC.Avalonia.MVVM;

/// <summary>
/// A command that relays its functionality to delegates.
/// Compatible with the WPFUtils RelayCommand API.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// A command that relays its functionality to delegates with a typed parameter.
/// </summary>
public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Predicate<T?>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke((T?)parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute((T?)parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// Alias for RelayCommand&lt;T&gt; to match WPFUtils naming.
/// </summary>
public class TypedRelayCommand<T> : RelayCommand<T>
{
    public TypedRelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
        : base(execute, canExecute)
    {
    }
}
