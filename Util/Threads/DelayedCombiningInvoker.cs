using System;
using System.Threading;

namespace MSHC.Util.Threads
{
	public class DelayedCombiningInvoker
	{
		private readonly object syncLock = new object();

		private readonly Action action;
		private readonly int delay;

		private Thread executor;
		private long lastRequestTime = -1;

		private DelayedCombiningInvoker(Action a, int d)
		{
			action = a;
			delay = d;
		}

		public static DelayedCombiningInvoker Create(Action a, int delay)
		{
			return new DelayedCombiningInvoker(a, delay);
		}

		private void Start()
		{
			if (executor == null || !executor.IsAlive)
			{
				executor = new Thread(Run);
				executor.IsBackground = true;
				executor.Start();
			}
		}

		public void Request()
		{
			lock (syncLock)
			{
				lastRequestTime = Environment.TickCount;
				if (executor == null || !executor.IsAlive) Start();
			}
		}

		private void Run()
		{
			for (; ; )
			{
				lock (syncLock)
				{
					long duration = Environment.TickCount - lastRequestTime;
					if (duration > delay)
					{
						action();
						return;
					}
				}

				Thread.Sleep(1 + delay / 100);
			}
		}
	}
}
