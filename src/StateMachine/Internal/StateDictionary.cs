using System;
using System.Collections.Generic;
using System.Reflection;

namespace Library.StateMachine {
    internal class StateDictionary<TState, TInput> : IStateDictionary<TState, TInput>
        where TState : IComparable
        where TInput : IComparable {

        #region Fields

        private readonly IStateMachineContext<TState, TInput> _context;
        private readonly IDictionary<TState, IState<TState, TInput>> _dictionary;
        private readonly object _syncRoot = new object();

        #endregion

        #region Ctors

        internal StateDictionary(IStateMachineContext<TState, TInput> context) {
            _context = context;
            _dictionary = new Dictionary<TState, IState<TState, TInput>>();
        }

        #endregion

        #region Properties

        public IState<TState, TInput> this[TState id] {
            get {
                lock (_syncRoot) {
                    if (!_dictionary.ContainsKey(id)) {
                        var state = CreateState(id);
                        _dictionary.Add(id, state);
                    }
                    return _dictionary[id];
                }
            }
        }

        #endregion

        #region Private Methods

        private IState<TState, TInput> CreateState(TState id) {
            var state = _context.Factory.CreateState(id);
            RegisterStateEntryActions(state);
            RegisterStateExitActions(state);
            return state;
        }

        private void RegisterStateEntryActions(IState<TState, TInput> state) {
            var actions = GetEntryOrExitActions(state, state.GetType(), typeof(StateEntryActionAttribute));
            foreach (var action in actions) {
                var actionToInvoke = action;
                state.AddEntryAction(_context.Factory
                    .CreateActionHolder(param => actionToInvoke.Invoke(state, new object[] { param })));
            }
        }

        private void RegisterStateExitActions(IState<TState, TInput> state) {
            var actions = GetEntryOrExitActions(state, state.GetType(), typeof(StateExitActionAttribute));
            foreach (var action in actions) {
                var actionToInvoke = action;
                state.AddExitAction(_context.Factory
                    .CreateActionHolder(param => actionToInvoke.Invoke(state, new object[] { param })));
            }
        }

        private static IEnumerable<MethodInfo> GetEntryOrExitActions(IState<TState, TInput> state, Type stateType, Type attributeType) {
            var result = new List<MethodInfo>();
            foreach (var method in stateType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (Attribute.IsDefined(method, attributeType, true)) {
                    if (method.GetParameters().Length != 1) {
                        if (method.DeclaringType != null) {
                            throw new ArgumentException(
                                  string.Format(
                                        "State has entry / exit action method with wrong signature. Expected signature is void <MethodName>(object <MethodParam>). Method: {0}.{1}",
                                        method.DeclaringType.FullName, method.Name));
                        }
                        throw new ArgumentNullException(string.Format("method.DeclaringType"));
                    }
                    result.Add(method);
                }
            }
            return result;
        }

        #endregion
    }
}
