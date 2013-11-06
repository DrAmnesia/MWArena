using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace GW2Stuff
{
	public delegate void TaskQueueItemCompletionHandler(object source, TaskQueueItemCompletionArgs e);

	public class TaskQueueItemCompletionArgs: EventArgs
	{
		public Exception exception;
		public Func<Object, Object> func;
		public Object param;
		public Object result;

		public TaskQueueItemCompletionArgs(Exception exception, Func<Object, Object> func, Object param, Object result)
		{
			this.exception = exception;
			this.func = func;
			this.param = param;
			this.result = result;
		}
	}

	public class TaskQueueInvalidOperationException: System.Exception {}

	class TaskQueue
	{
		public event TaskQueueItemCompletionHandler taskQueueItemComplete;

		public TaskQueue()
		{
			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			workQueue = new BlockingCollection<QueueItem>();

			worker.DoWork +=
					(object sender, DoWorkEventArgs e) =>
					{
						try
						{
							while (true)
							{
								QueueItem queueItem = workQueue.Take();
								queueItem.run();
								worker.ReportProgress(0, queueItem);
							}
						}
						catch (Exception)
						{
							// Ignore exceptions at this point - failed to take from queue
						}
					};
			worker.ProgressChanged +=
					(object sender, ProgressChangedEventArgs e) =>
					{
						QueueItem queueItem = (QueueItem)(e.UserState);
						if (taskQueueItemComplete != null)
						{
							taskQueueItemComplete(
								this,
								new TaskQueueItemCompletionArgs(
									queueItem.exception,
									queueItem.func,
									queueItem.param,
									queueItem.result
								)
							);
						}
					};
			worker.RunWorkerAsync();
		}

		public void run(Object param, Func<Object, Object> func)
		{
			if (!workQueue.IsAddingCompleted)
			{
				workQueue.Add(new QueueItem(param, func));
			}
			else
			{
				throw new TaskQueueInvalidOperationException();
			}
		}

		public void complete()
		{
			workQueue.CompleteAdding();
		}


		/* Internals */

		private class QueueItem
		{
			public Exception exception;
			public Func<Object, Object> func;
			public Object param;
			public Object result;

			public QueueItem(Object param, Func<Object, Object> func)
			{
				this.exception = null;
				this.func = func;
				this.param = param;
				this.result = null;
			}

			public void run()
			{
				try
				{
					this.result = this.func(this.param);
				}
				catch (Exception ex)
				{
					this.exception = ex;
				}
			}
		}

		private BackgroundWorker worker;
		private BlockingCollection<QueueItem> workQueue;
	}
}
