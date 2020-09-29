using System;
using System.Collections.Generic;

namespace Library.StateMachine
{
    public interface IActionHolderCollection<TState, TInput> : IEnumerable<IActionHolder<TState, TInput>>
        where TState : IComparable
        where TInput : IComparable
    {
        int Count { get; }

        void Add(IActionHolder<TState, TInput> actionHolder);
        void Remove(IActionHolder<TState, TInput> actionHolder);
    }
}
