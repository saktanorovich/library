using System;

namespace Library.StateMachine
{
    public class TransitionGuard<TState, TInput> : ITransitionGuard<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        private readonly Predicate<IStateMachineContext<TState, TInput>> guard;

        public TransitionGuard(Predicate<IStateMachineContext<TState, TInput>> guard)
        {
            this.guard = guard;
        }

        public bool Execute(IStateMachineContext<TState, TInput> context)
        {
            return ExecuteImpl(context);
        }

        protected virtual bool ExecuteImpl(IStateMachineContext<TState, TInput> context)
        {
            return guard(context);
        }
    }
}
