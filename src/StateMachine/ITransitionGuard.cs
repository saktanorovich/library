using System;

namespace Library.StateMachine
{
    public interface ITransitionGuard<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        bool Execute(IStateMachineContext<TState, TInput> context);
    }
}
