using System;

namespace Subtitler.Desktop.Helpers
{
	public class AsyncExecutor : IDisposable
	{
		public delegate void ExecutionFinishedHandler(object sender);
		public event ExecutionFinishedHandler OnExecutionComplete = delegate { };

		public AsyncExecutor() {}

		/// <summary>
		/// Execute task without parameters with TOut return value type
		/// </summary>
		public void ExecuteTask<TOut>
			(Func<TOut> task)
		{
		
			var loader = new Func<TOut>(() => task());

			AsyncCallback callback = asyncResult =>
			{
				TOut result = loader.EndInvoke(asyncResult);

				App.Current.Dispatcher.Invoke(
					() => OnExecutionComplete(result)
					);
				asyncResult.AsyncWaitHandle.Close();
			};

			loader.BeginInvoke(callback, null);
		}

		/// <summary>
		/// Execute task without TIn parameter and with TOut return value type
		/// </summary>
		public void ExecuteTask<TIn, TOut>
			(Func<TIn, TOut> task, TIn taskParameter)
		{

			var loader = new Func<TOut>(() => task(taskParameter));

			AsyncCallback callback = asyncResult =>
			{
				TOut result = loader.EndInvoke(asyncResult);

				App.Current.Dispatcher.Invoke(
					() => OnExecutionComplete(result)
					);
				asyncResult.AsyncWaitHandle.Close();
			};

			loader.BeginInvoke(callback, null);
		}


		public void Dispose()
		{
			//OnExecutionComplete = null;
			//todo ??
		}
	}


}
