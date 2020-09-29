using System;

namespace Library.StateMachine
{
    public class TransitionEventArgs<TState, TInput> : EventArgs
        where TState : IComparable
        where TInput : IComparable
    {

        public TransitionEventArgs(ITransition<TState, TInput> transition, object args)
        {
            Transition = transition;
            Args = args;
        }

        public ITransition<TState, TInput> Transition { get; }
        public object Args { get; }
    }
}
