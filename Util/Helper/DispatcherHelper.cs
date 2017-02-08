using System;
using System.Threading;
using System.Windows;

namespace MSHC.Util.Helper
{
	public static class DispatcherHelper
	{
		/// <summary>
		/// Rufe act() auf wenn im Dispatcher
		/// Ansonsten Dispatcher.Invoke
		/// 
		/// Achtung! Beim Beenden kann es vorkommen dass Application.Current NULL ist und dann wird act _nicht_ mehr aufgerufen
		/// </summary>
		public static void SmartInvoke(Action act)
		{
			var app = Application.Current;
			if (app == null) return;
			if (app.CheckAccess())
			{
				act();
			}
			else
			{
				app.Dispatcher.Invoke(act);
			}
		}

		/// <summary>
		/// Rufe act() auf wenn im Dispatcher
		/// Ansonsten Dispatcher.BeginInvoke
		/// 
		/// Achtung! Beim Beenden kann es vorkommen dass Application.Current NULL ist und dann wird act _nicht_ mehr aufgerufen
		/// Achtung! wenn im Dispatcher dann blockiert der Aufruf von act()
		/// </summary>
		public static void SmartBeginInvoke(Action act)
		{
			var app = Application.Current;
			if (app == null) return;
			if (app.CheckAccess())
			{
				act();
			}
			else
			{
				app.Dispatcher.BeginInvoke(act);
			}
		}

		/// <summary>
		/// Ruft act() verzögert im Dispatcher auf
		/// </summary>
		/// <param name="act">Auszuführende Aktion</param>
		/// <param name="delay">delay in ms</param>
		public static void InvokeDelayed(Action act, int delay)
		{
			new Thread(() => { Thread.Sleep(delay); SmartBeginInvoke(act); }).Start();
		}
	}
}
