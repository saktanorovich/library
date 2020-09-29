using System;

namespace Library.StateMachine
{
    public class StateMachine<TState, TInput> : IStateMachine<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        private readonly IStateDictionary<TState, TInput> states;
        private readonly IStateMachineContext<TState, TInput> context;
        private readonly object _syncRoot = new object();
        private IState<TState, TInput> currentState;
        private bool isInitialized;

        public StateMachine()
            : this(string.Empty)
        {
        }

        public StateMachine(string name)
            : this(name, new StateMachineContext<TState, TInput>())
        {
        }

        public StateMachine(IStateMachineContext<TState, TInput> context)
            : this(string.Empty, context)
        {
        }

        public StateMachine(string name, IStateMachineContext<TState, TInput> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Name = name;
            this.context = context;
            states = new StateDictionary<TState, TInput>(this.context);
        }

        public event EventHandler<TransitionEventArgs<TState, TInput>> TransitionInitiated;
        public event EventHandler<TransitionEventArgs<TState, TInput>> TransitionCompleted;

        public string Name { get; }

        public void Initialize(TState initialState)
        {
            lock (_syncRoot)
            {
                if (!isInitialized)
                {
                    isInitialized = true;
                    currentState = states[initialState];
                }
            }
        }

        public IStateEntryBehaviour<TState, TInput> In(TState id)
        {
            return new StateBehaviourBuilder<TState, TInput>(states[id], states, context);
        }

        public void Start()
        {
            EnsureMachineIsInitialized();
            lock (_syncRoot)
            {
                currentState.Entry(context);
            }
        }

        public void Handle(TInput inputSignal)
        {
            CoreHandleImpl(inputSignal, null);
        }

        public void Handle(TInput inputSignal, object args)
        {
            CoreHandleImpl(inputSignal, args);
        }

        protected virtual void CoreHandleImpl(TInput inputSignal, object args)
        {
            EnsureMachineIsInitialized();
            lock (_syncRoot)
            {
                var transition = currentState.Transitions[inputSignal];
                if (transition != null)
                {
                    if (transition.CanFire(context))
                    {
                        OnTransitionInitiated(transition, args);
                        currentState.Exit(context);
                        transition.Fire(context, args);
                        currentState = transition.Target;
                        currentState.Entry(context);
                        OnTransitionCompleted(transition, args);
                    }
                }
            }
        }

        protected virtual void OnTransitionInitiated(ITransition<TState, TInput> transition, object args)
        {
            TransitionInitiated.Raise(this, new TransitionEventArgs<TState, TInput>(transition, args));
        }

        protected virtual void OnTransitionCompleted(ITransition<TState, TInput> transition, object args)
        {
            TransitionCompleted.Raise(this, new TransitionEventArgs<TState, TInput>(transition, args));
        }

        private void EnsureMachineIsInitialized()
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("The state machine is not initialized.");
            }
        }
    }
}
