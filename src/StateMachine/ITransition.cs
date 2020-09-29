using System;

namespace Library.StateMachine
{
    /// <summary>
    /// A directed relationship between two states which represents the complete response of a state machine
    /// to an occurrence of an event of a particular type.
    /// </summary>
    /// <typeparam name="TState">The type of the state id.</typeparam>
    /// <typeparam name="TInput">The type of the input signal.</typeparam>
    public interface ITransition<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        IState<TState, TInput> Source { get; }
        IState<TState, TInput> Target { get; }
        TInput InputSignal { get; }

        void AddGuard(ITransitionGuard<TState, TInput> guard);
        void RemoveGuard(ITransitionGuard<TState, TInput> guard);
        void Fire(IStateMachineContext<TState, TInput> context, object args);
        bool CanFire(IStateMachineContext<TState, TInput> context);
    }
}
