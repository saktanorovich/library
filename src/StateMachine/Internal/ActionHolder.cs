using System;

namespace Library.StateMachine {
    internal class ActionHolder<TState, TInput> : IActionHolder<TState, TInput>
        where TState : IComparable
        where TInput : IComparable {

        #region Fields

        private readonly Action<IStateMachineContext<TState, TInput>> _action;

        #endregion

        #region Ctors

        internal ActionHolder(Action<IStateMachineContext<TState, TInput>> action) {
            _action = action;
        }

        #endregion

        #region Public Methods

        public void Execute(IStateMachineContext<TState, TInput> context) {
            _action(context);
        }

        #endregion
    }
}
