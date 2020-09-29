using System;

namespace Library.StateMachine
{
    /// <summary>
    /// The basic unit that composes a state machine. A state machine can be
    /// in one state at any particular time.
    /// </summary>
    /// <typeparam name="TState">The type of the state id.</typeparam>
    /// <typeparam name="TInput">The type of the input signal.</typeparam>
    public interface IState<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        TState Id { get; }
        bool IsFinal { get; }
        ITransitionDictionary<TState, TInput> Transitions { get; }

        void AddEntryAction(IActionHolder<TState, TInput> actionHolder);
        void AddExitAction(IActionHolder<TState, TInput> actionHolder);

        /// <summary>
        /// An activity executed when entering the state.
        /// </summary>
        void Entry(IStateMachineContext<TState, TInput> context);

        /// <summary>
        /// An activity executed when exiting the state.
        /// </summary>
        void Exit(IStateMachineContext<TState, TInput> context);
    }
}
