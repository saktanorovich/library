using System;

namespace Library.StateMachine
{
    /// <summary>
    /// Defines state machine interface.
    /// </summary>
    /// <typeparam name="TState">The type of the state id.</typeparam>
    /// <typeparam name="TInput">The type of the input signal.</typeparam>
    public interface IStateMachine<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        string Name { get; }

        event EventHandler<TransitionEventArgs<TState, TInput>> TransitionInitiated;
        event EventHandler<TransitionEventArgs<TState, TInput>> TransitionCompleted;

        void Initialize(TState initialState);
        IStateEntryBehaviour<TState, TInput> In(TState id);
        void Handle(TInput inputSignal);
        void Handle(TInput inputSignal, object args);
        void Start();
    }
}
