using System;
using System.Collections.Generic;

namespace DataExchange
{
    public interface IExchangeable<T> where T: struct, Enum
    {
        event Action<object, ICommand<T>> OnDataReceive;
        event Action<object, ICommand<T>> OnError;
        void RemoveCommand(ICommand<T> cmd);
        void AddCommand(ICommand<T> cmd);
        void AddBaseCommand(Func<ICommand<T>> cmd);

        List<ICommand<T>> Commands { get; }
        List<Func<ICommand<T>>> BaseCommands { get; }
    }
}
