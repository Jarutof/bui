using bui.Operations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bui
{
    public class OperationsManager 
    {
        public event Action<Operation> OnOperationStarted;
        public event Action<Operation> OnOperationFinished;
        public event Action<OperationProgressEventArgs> OnOperationProgressChange;
        public event Action<int> OnCountChanged;
        private static readonly object lockObj = new object();
        private List<Func<Operation>> operations = new List<Func<Operation>>();
        public void Add(Func<Operation> getOperation)
        {
            if (operations.Count == 0)
            {
                lock (lockObj)
                {
                    operations.Add(getOperation);
                    OnCountChanged?.Invoke(operations.Count);
                }
                StartOperation();
            }
            else
            {
                lock (lockObj)
                {
                    operations.Add(getOperation);
                }
            }
        }

        private void StartOperation()
        {
            Task.Run(async ()=>
            {
                while(operations.Count > 0)
                {
                    Operation operation = operations[0]();
                    operation.OnStarted += () => OnOperationStarted?.Invoke(operation);
                    operation.Progress += e => OnOperationProgressChange?.Invoke(e);
                    operation.OnFinished += res =>
                    {
                        OnOperationFinished?.Invoke(operation);
                    };
                    await operation.StartAsync();
                    lock (lockObj)
                    {
                        operations.RemoveAt(0);
                        OnCountChanged?.Invoke(operations.Count);
                    }
                }
            });
        }
    }
}
