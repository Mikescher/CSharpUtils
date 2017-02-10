﻿using System;
using System.Threading;

namespace MSHC.Util.Threads
{
	public class DelayedCombiningInvoker
	{
		private readonly object syncLock = new object();

		private readonly Action action;
		private readonly int delay;
		private readonly int mayDelay;

		private Thread executor;
		private long lastRequestTime = -1;
		private long initialRequestTime = -1;
		private bool cancelled = false;

		private DelayedCombiningInvoker(Action a, int d, int md)
		{
			action = a;
			delay = d;
			mayDelay = md;
		}

		public static DelayedCombiningInvoker Create(Action a, int delay, int maxDelay)
		{
			return new DelayedCombiningInvoker(a, delay, maxDelay);
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
			lock (syncLock)
			{
				initialRequestTime = Environment.TickCount;
			}

			for (; ; )
			{
				if (cancelled) return;

				lock (syncLock)
				{
					long durationLast = Environment.TickCount - lastRequestTime;
					long durationTotal = Environment.TickCount - initialRequestTime;

					if (durationLast > delay || durationTotal > initialRequestTime)
					{
						action();
						return;
					}
				}

				Thread.Sleep(1 + delay / 100);
			}
		}

		public void CancelPendingRequests()
		{
			if (executor != null && executor.IsAlive)
			{
				cancelled = true;
				while (executor.IsAlive)
				{
					Thread.Sleep(delay / 200);
				}
				cancelled = false;
			}
		}
	}
}
