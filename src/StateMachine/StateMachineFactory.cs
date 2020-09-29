using System;

namespace Library.StateMachine
{
    public class StateMachineFactory<TState, TInput> : IStateMachineFactory<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        public IState<TState, TInput> CreateState(TState id)
        {
            return CreateStateImpl(id);
        }

        public ITransition<TState, TInput> CreateTransition(IState<TState, TInput> source, IState<TState, TInput> target, TInput inputSignal)
        {
            return CreateTransitionImpl(source, target, inputSignal);
        }

        public IActionHolder<TState, TInput> CreateActionHolder(Action<IStateMachineContext<TState, TInput>> action)
        {
            return CreateActionHolderImpl(action);
        }

        public ITransitionGuard<TState, TInput> CreateTransitionGuard(Predicate<IStateMachineContext<TState, TInput>> guard)
        {
            return CoreCreateTransitionGuard(guard);
        }

        protected virtual IState<TState, TInput> CreateStateImpl(TState id)
        {
            return new State<TState, TInput>(id);
        }

        protected virtual ITransition<TState, TInput> CreateTransitionImpl(IState<TState, TInput> source, IState<TState, TInput> target, TInput inputSignal)
        {
            return new Transition<TState, TInput>(source, target, inputSignal);
        }

        protected virtual IActionHolder<TState, TInput> CreateActionHolderImpl(Action<IStateMachineContext<TState, TInput>> action)
        {
            return new ActionHolder<TState, TInput>(action);
        }

        protected virtual ITransitionGuard<TState, TInput> CoreCreateTransitionGuard(Predicate<IStateMachineContext<TState, TInput>> guard)
        {
            return new TransitionGuard<TState, TInput>(guard);
        }
    }
}
