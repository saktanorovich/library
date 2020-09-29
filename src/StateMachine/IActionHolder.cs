using System;

namespace Library.StateMachine
{
    public interface IActionHolder<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        void Execute(IStateMachineContext<TState, TInput> context);
    }
}
