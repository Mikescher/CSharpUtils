using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MSHC.Util.Threads
{
	public class SingleThreadPool
	{
		private readonly object syncLock = new object();

		private readonly Action<int> setCount;
		private readonly Action<Exception> errorCallback;

		private readonly ConcurrentStack<Action> queue = new ConcurrentStack<Action>(); 
		private Thread executor;

		public SingleThreadPool(Action<int> onChange, Action<Exception> onError)
		{
			executor = new Thread(Run);

			setCount = onChange;
			errorCallback = onError;
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

		public void QueueAction(Action a)
		{
			lock (syncLock)
			{
				queue.Push(a);
				if (setCount != null) setCount(queue.Count);
				if (executor == null || !executor.IsAlive) Start();
			}
		}

		private void Run()
		{
			for (;;)
			{
				Action action;

				lock (syncLock)
				{
					bool exec = queue.TryPop(out action);
					if (!exec)
					{
						if (setCount != null) setCount(queue.Count);
						return;
					}
				}

				try
				{
					action.Invoke();
					if (setCount != null) setCount(queue.Count);
				}
				catch (Exception e)
				{
					if (errorCallback != null) errorCallback(e);
					return;
				}
			}
		}
	}
}
