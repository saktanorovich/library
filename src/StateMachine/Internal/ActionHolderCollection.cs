using System;
using System.Collections;
using System.Collections.Generic;

namespace Library.StateMachine {
    internal class ActionHolderCollection<TState, TInput> : IActionHolderCollection<TState, TInput>
        where TState : IComparable
        where TInput : IComparable {

        #region Fields

        private readonly IList<IActionHolder<TState, TInput>> _actionHolders;

        #endregion

        #region Ctors

        internal ActionHolderCollection() {
            _actionHolders = new List<IActionHolder<TState, TInput>>();
        }

        #endregion

        #region Properties

        public int Count {
            get {
                return _actionHolders.Count;
            }
        }

        #endregion

        #region Public Methos

        public void Add(IActionHolder<TState, TInput> actionHolder) {
            _actionHolders.Add(actionHolder);
        }

        public void Remove(IActionHolder<TState, TInput> actionHolder) {
            _actionHolders.Remove(actionHolder);
        }

        public IEnumerator<IActionHolder<TState, TInput>> GetEnumerator() {
            if (_actionHolders == null) {
            }
            else {
                foreach (var actionHolder in _actionHolders) {
                    yield return actionHolder;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
