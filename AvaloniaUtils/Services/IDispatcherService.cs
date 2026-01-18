namespace MSHC.Avalonia.Services;

/// <summary>
/// Abstraction for UI thread dispatcher operations.
/// Replaces direct use of Application.Current.Dispatcher.
/// </summary>
public interface IDispatcherService
{
    /// <summary>
    /// Executes an action synchronously on the UI thread.
    /// </summary>
    void Invoke(Action action);

    /// <summary>
    /// Executes an action asynchronously on the UI thread.
    /// </summary>
    void BeginInvoke(Action action);

    /// <summary>
    /// Executes an action on the UI thread after a delay.
    /// </summary>
    void InvokeDelayed(Action action, TimeSpan delay);

    /// <summary>
    /// Returns true if the current thread is the UI thread.
    /// </summary>
    bool CheckAccess();
}
