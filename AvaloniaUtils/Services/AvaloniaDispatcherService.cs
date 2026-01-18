using Avalonia.Threading;

namespace MSHC.Avalonia.Services;

/// <summary>
/// Avalonia implementation of IDispatcherService.
/// </summary>
public class AvaloniaDispatcherService : IDispatcherService
{
    public void Invoke(Action action)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            action();
        }
        else
        {
            Dispatcher.UIThread.Invoke(action);
        }
    }

    public void BeginInvoke(Action action)
    {
        Dispatcher.UIThread.Post(action);
    }

    public void InvokeDelayed(Action action, TimeSpan delay)
    {
        Task.Delay(delay).ContinueWith(_ => Dispatcher.UIThread.Post(action));
    }

    public bool CheckAccess()
    {
        return Dispatcher.UIThread.CheckAccess();
    }
}
