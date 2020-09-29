using System;

namespace Library.StateMachine {
    internal class StateMachineContext<TState, TInput> : IStateMachineContext<TState, TInput>
        where TState : IComparable
        where TInput : IComparable {

        #region Ctors

        internal StateMachineContext() {
            Factory = new StateMachineFactory<TState, TInput>();
        }

        #endregion

        #region Properties

        public IStateMachineFactory<TState, TInput> Factory { get; private set; }

        #endregion

        #region Methods

        public void HandleException(Exception e) {
        }

        #endregion
    }
}
