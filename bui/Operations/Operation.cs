    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

namespace bui.Operations
{
    public class OperationProgressEventArgs
    {
        public float Value { get; private set; }
        public string Message { get; private set; }
        public OperationProgressEventArgs(float value, string message)
        {
            Value = Value;
            Message = message;
        }
    }
    public abstract class Operation
    {
        public event Action<OperationProgressEventArgs> Progress;
        public event Action OnStarted;
        public event Action<bool> OnFinished;
        protected CancellationTokenSource Source = new CancellationTokenSource();
        protected TaskCompletionSource<bool> tcs;


        public bool IsStarted { get; set; }
        public string Name { get; protected set; }
        public abstract Task<bool> ProcessAsync(CancellationToken token);
        protected virtual void OnProgress(float value, string message) => Progress?.Invoke(new OperationProgressEventArgs(value, message));
        protected virtual void OnProgress(OperationProgressEventArgs e) => Progress?.Invoke(e);
        public void Start()
        {
            tcs = new TaskCompletionSource<bool>();

            Task.Run(async () =>
            {
                IsStarted = true;
                bool result = false;
                OnStarted?.Invoke();
                try
                {
                    result = await ProcessAsync(Source.Token);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                IsStarted = false;
                OnFinished?.Invoke(result);

                tcs?.TrySetResult(result);
            }, Source.Token);
        }
        public async Task<bool> StartAsync()
        {
            Start();
            return await tcs.Task;
        }
        public async Task StopAsync()
        {
            Source.Cancel();
            await tcs.Task;
        }

    }
}
