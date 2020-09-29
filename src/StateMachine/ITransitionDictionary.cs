using System;

namespace Library.StateMachine
{
    public interface ITransitionDictionary<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        int Count { get; }
        ITransition<TState, TInput> this[TInput inputSignal] { get; }

        void Add(ITransition<TState, TInput> transition);
    }
}
