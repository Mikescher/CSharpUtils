using System;
using System.Threading;

namespace MSHC.Util.Threads
{
	public class DelayedCombiningInvoker
	{
		private readonly object _syncLock = new object();

		private readonly Action _action;
		private readonly int _delay;
		private readonly int _maxDelay;
		private readonly bool _highspeed;

		private Thread _executor;
		private long _lastRequestTime = -1;
		private long _initialRequestTime = -1;
		private bool _cancelled = false;

		private DelayedCombiningInvoker(Action a, int d, int md, bool hs)
		{
			_action = a;
			_delay = d;
			_maxDelay = md;
			_highspeed = hs;
		}

		public static DelayedCombiningInvoker Create(Action a, int delay, int maxDelay)
		{
			return new DelayedCombiningInvoker(a, delay, maxDelay, false);
		}

		public static DelayedCombiningInvoker CreateHighspeed(Action a, int delay, int maxDelay)
		{
			return new DelayedCombiningInvoker(a, delay, maxDelay, true);
		}

		private void Start()
		{
			if (_executor == null || !_executor.IsAlive)
			{
				_executor = new Thread(Run) { IsBackground = true };
				_executor.Start();
			}
		}

		public void Request()
		{
			lock (_syncLock)
			{
				_lastRequestTime = Environment.TickCount;
				if (_executor == null || !_executor.IsAlive) Start();
			}
		}

		private void Run()
		{
			lock (_syncLock)
			{
				_initialRequestTime = Environment.TickCount;
			}

			for (; ; )
			{
				if (_cancelled) return;

				lock (_syncLock)
				{
					long durationLast = Environment.TickCount - _lastRequestTime;
					long durationTotal = Environment.TickCount - _initialRequestTime;

					if (durationLast > _delay || durationTotal > _maxDelay)
					{
						_action();
						return;
					}
				}

				Thread.Sleep((_highspeed) ? (0) : (1 + _delay / 100));
			}
		}

		public void CancelPendingRequests()
		{
			if (_executor != null && _executor.IsAlive)
			{
				_cancelled = true;
				while (_executor.IsAlive)
				{
					Thread.Sleep((_highspeed) ? (0) : (_delay / 200));
				}
				_cancelled = false;
			}
		}

		public bool HasPendingRequests()
		{
			return (_executor != null && _executor.IsAlive);
		}
	}
}
