using System;

namespace Library.StateMachine
{
    public interface IStateMachineContext<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        IStateMachineFactory<TState, TInput> Factory { get; }

        void HandleException(Exception e);
    }
}
