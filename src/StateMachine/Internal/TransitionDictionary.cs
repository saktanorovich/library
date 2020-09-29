using System;
using System.Collections.Generic;

namespace Library.StateMachine {
    internal class TransitionDictionary<TState, TInput> : ITransitionDictionary<TState, TInput>
        where TState : IComparable
        where TInput : IComparable {

        #region Fields

        private readonly IDictionary<TInput, ITransition<TState, TInput>> _dictionary;

        #endregion

        #region Ctors

        internal TransitionDictionary() {
            _dictionary = new Dictionary<TInput, ITransition<TState, TInput>>();
        }

        #endregion

        #region Properties

        public int Count {
            get {
                return _dictionary.Count;
            }
        }

        public ITransition<TState, TInput> this[TInput inputSignal] {
            get {
                ITransition<TState, TInput> result;
                if (_dictionary.TryGetValue(inputSignal, out result)) {
                    return result;
                }
                return null;
            }
        }

        #endregion

        #region Public Methods

        public void Add(ITransition<TState, TInput> transition) {
            if (!_dictionary.ContainsKey(transition.InputSignal)) {
                _dictionary.Add(transition.InputSignal, transition);
            }
            else {
                throw new InvalidOperationException("Could not add transition twice.");
            }
        }

        #endregion
    }
}
