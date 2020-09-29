using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.StateMachine {
    internal class StateBehaviourBuilder<TState, TInput> : IStateEntryBehaviour<TState, TInput>, IStateExitBehaviour<TState, TInput>, IStateOnInputBehaviour<TState, TInput>
        where TState : IComparable
        where TInput : IComparable {

        #region Nested Types

        private class TransitionInfo {
            private readonly IStateMachineFactory<TState, TInput> _factory;
            private readonly IList<ITransitionGuard<TState, TInput>> _guards;

            internal TransitionInfo(IStateMachineFactory<TState, TInput> stateMachineFactory) {
                _factory = stateMachineFactory;
                _guards = new List<ITransitionGuard<TState, TInput>>();
            }

            internal IState<TState, TInput> Source { get; set; }
            internal IState<TState, TInput> Target { get; set; }
            internal TInput InputSignal { get; set; }

            public void AddGuard(ITransitionGuard<TState, TInput> guard) {
                _guards.Add(guard);
            }

            public ITransition<TState, TInput> GetTransition() {
                var transition = _factory.CreateTransition(Source, Target, InputSignal);
                foreach (var guard in _guards) {
                    transition.AddGuard(guard);
                }
                return transition;
            }
        }

        #endregion

        #region Fields

        private readonly IState<TState, TInput> _source;
        private readonly IStateDictionary<TState, TInput> _states;
        private readonly IStateMachineContext<TState, TInput> _context;
        private readonly IList<TransitionInfo> _transitions;

        #endregion

        #region Ctors

        public StateBehaviourBuilder(IState<TState, TInput> source, IStateDictionary<TState, TInput> states, IStateMachineContext<TState, TInput> context) {
            _source = source;
            _states = states;
            _context = context;
            _transitions = new List<TransitionInfo>();
        }

        #endregion

        #region Public Methods

        public IStateExitBehaviour<TState, TInput> ExecuteOnEntry(params Action<object>[] actions) {
            foreach (var action in actions) {
                _source.AddEntryAction(_context.Factory.CreateActionHolder(action));
            }
            return this;
        }

        public IStateBehaviour<TState, TInput> ExecuteOnExit(params Action<object>[] actions) {
            foreach (var action in actions) {
                _source.AddExitAction(_context.Factory.CreateActionHolder(action));
            }
            return this;
        }

        public IStateOnInputBehaviour<TState, TInput> On(TInput inputSignal) {
            var found = false;
            foreach (var transition in _transitions) {
                if (transition.InputSignal.CompareTo(inputSignal) == 0) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                var transitionInfo = new TransitionInfo(_context.Factory) {
                    Source = _source,
                    InputSignal = inputSignal
                };
                _transitions.Add(transitionInfo);
            }
            return this;
        }

        public IStateOnInputBehaviour<TState, TInput> Guard(Predicate<object> guard) {
            if (ReferenceEquals(guard, null)) {
                throw new ArgumentNullException("guard");
            }
            if (_transitions.Count == 0) {
                throw new InvalidOperationException();
            }
            var lastTransition = _transitions.Last();
            lastTransition.AddGuard(
                _context.Factory.CreateTransitionGuard(guard));
            return this;
        }

        public IStateOnInputBehaviour<TState, TInput> Guard<TTransitionGuard>() where TTransitionGuard : ITransitionGuard<TState, TInput>, new() {
            if (_transitions.Count == 0) {
                throw new InvalidOperationException();
            }
            var lastTransition = _transitions.Last();
            lastTransition.AddGuard(new TTransitionGuard());
            return this;
        }

        public IStateBehaviour<TState, TInput> Goto(TState target) {
            foreach (var transitionInfo in _transitions) {
                transitionInfo.Target = _states[target];
                _source.Transitions.Add(transitionInfo.GetTransition());
            }
            _transitions.Clear();
            return this;
        }

        #endregion
    }
}
