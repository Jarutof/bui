using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace DataExchange
{
    public abstract class Exchanger<T> : IExchangeable<T> where T: struct, Enum
    {
        public event Action<object, ICommand<T>> OnDataReceive;
        public event Action<object, ICommand<T>> OnError;

        private CancellationTokenSource source;

        private TaskCompletionSource<bool> tcs;

        private bool isStarted;

        protected readonly static object lockObject = new object();
        public List<ICommand<T>> Commands { get; } = new List<ICommand<T>>();
        public List<Func<ICommand<T>>> BaseCommands { get; } = new List<Func<ICommand<T>>>();
        private int activeCommandId;

        public abstract void OnListen();
        public abstract Task OnRequestAsync();
        public abstract void OnStop();


        public void StartListen()
        {
            if (isStarted) return;
            source = new CancellationTokenSource();
            var token = source.Token;
            Task.Run(async () =>
            {
                isStarted = true;
                while (!token.IsCancellationRequested)
                {
                    OnListen();
                    await Task.Delay(15);
                }
                Console.WriteLine("StartListen finish");
                OnStop();
                tcs.SetResult(true);
            }, token);
        }

        public void StartRequest()
        {
            if (isStarted) return;
            source = new CancellationTokenSource();
            var token = source.Token;
            Task.Run(async () =>
            {
                isStarted = true;

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                       await OnRequestAsync();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"{ex.Message} : {ex.StackTrace}");
                    }
                }
                Console.WriteLine("StartRequest finish");
                OnStop();
                tcs.SetResult(false);
            }, token);
        }
        protected void ErrorEvent(ICommand<T> cmd) => OnError?.Invoke(this, cmd);
        protected void DataReceiveEvent(ICommand<T> cmd) => OnDataReceive?.Invoke(this, cmd);
        public async Task StopAsync()
        {
            if (isStarted)
            {
                Console.WriteLine("StopAsync");
                tcs = new TaskCompletionSource<bool>();
                source.Cancel();
                isStarted = await tcs.Task;
            }
        }

        public void AddCommand(ICommand<T> cmd)
        {
            lock (lockObject)
            {
                Commands.Add(cmd);
            }
        }

        public ICommand<T> GetNexCommand()
        {
            if (Commands.Count > 0)
            {
                var bc = Commands.Where(c => c.IsBroadcast).ToList();
                if (bc != null && bc.Count > 0) return bc[0];
                return Commands[0];
            }
            var cmd = BaseCommands[activeCommandId]();
            activeCommandId++;
            activeCommandId %= BaseCommands.Count;
            return cmd;
        }

        public void AddBaseCommand(Func<ICommand<T>> cmd)
        {
            BaseCommands.Add(cmd);
        }

        public void RemoveCommand(ICommand<T> cmd)
        {
            if (Commands.Contains(cmd))
            {
                lock (lockObject)
                {
                    Commands.Remove(cmd);
                }
            }
            cmd.Complete();
        }
    }


}
