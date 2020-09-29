using System;

namespace Library.StateMachine
{
    public interface IStateMachineFactory<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {

        /// <summary>
        /// Creates state.
        /// </summary>
        IState<TState, TInput> CreateState(TState id);

        /// <summary>
        /// Creates transition.
        /// </summary>
        ITransition<TState, TInput> CreateTransition(IState<TState, TInput> source, IState<TState, TInput> target, TInput inputSignal);

        /// <summary>
        /// Creates action holder.
        /// </summary>
        IActionHolder<TState, TInput> CreateActionHolder(Action<IStateMachineContext<TState, TInput>> action);

        /// <summary>
        /// Creates transition guard.
        /// </summary>
        ITransitionGuard<TState, TInput> CreateTransitionGuard(Predicate<IStateMachineContext<TState, TInput>> guard);
    }
}
