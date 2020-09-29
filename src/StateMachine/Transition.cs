using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.StateMachine
{
    public class Transition<TState, TInput> : ITransition<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        private readonly IList<ITransitionGuard<TState, TInput>> guards;

        public Transition(IState<TState, TInput> source, IState<TState, TInput> target, TInput inputSignal)
        {
            Source = source;
            Target = target;
            InputSignal = inputSignal;
            guards = new List<ITransitionGuard<TState, TInput>>();
        }

        public IState<TState, TInput> Source { get; }
        public IState<TState, TInput> Target { get; }
        public TInput InputSignal { get; }

        public void AddGuard(ITransitionGuard<TState, TInput> guard)
        {
            if (ReferenceEquals(guard, null))
            {
                throw new ArgumentNullException("guard");
            }
            guards.Add(guard);
        }

        public void RemoveGuard(ITransitionGuard<TState, TInput> guard)
        {
            if (ReferenceEquals(guard, null))
            {
                throw new ArgumentNullException("guard");
            }
            guards.Remove(guard);
        }

        public void Fire(IStateMachineContext<TState, TInput> context, object args)
        {
            FireImpl(context, args);
        }

        public bool CanFire(IStateMachineContext<TState, TInput> context)
        {
            return CanFireImpl(context);
        }

        protected virtual void FireImpl(IStateMachineContext<TState, TInput> context, object args)
        {
        }

        protected virtual bool CanFireImpl(IStateMachineContext<TState, TInput> context)
        {
            return guards.All(guard => guard.Execute(context));
        }
    }
}
