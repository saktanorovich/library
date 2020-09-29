using System;

namespace Library.StateMachine
{
    public interface IStateDictionary<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        IState<TState, TInput> this[TState id] { get; }
    }
}
