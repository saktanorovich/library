using System;

namespace Library.StateMachine
{
    public interface IStateBehaviour<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        IStateOnInputBehaviour<TState, TInput> On(TInput inputSignal);
    }

    public interface IStateEntryBehaviour<TState, TInput> : IStateBehaviour<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        IStateExitBehaviour<TState, TInput> ExecuteOnEntry(params Action<object>[] actions);
        IStateBehaviour<TState, TInput> ExecuteOnExit(params Action<object>[] actions);
    }

    public interface IStateExitBehaviour<TState, TInput> : IStateBehaviour<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        IStateBehaviour<TState, TInput> ExecuteOnExit(params Action<object>[] actions);
    }

    public interface IStateOnInputBehaviour<TState, TInput> : IStateBehaviour<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        IStateOnInputBehaviour<TState, TInput> Guard(Predicate<object> transitionGuard);
        IStateOnInputBehaviour<TState, TInput> Guard<TTransitionGuard>() where TTransitionGuard : ITransitionGuard<TState, TInput>, new();

        IStateBehaviour<TState, TInput> Goto(TState target);
    }
}
