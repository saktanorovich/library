using System;

namespace Library.StateMachine
{
    public abstract class StateMachineExecutor<TState, TInput> : DisposableObject
        where TState : IComparable
        where TInput : IComparable
    {
        protected readonly IStateMachine<TState, TInput> StateMachine;

        protected StateMachineExecutor(IStateMachine<TState, TInput> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public void Start()
        {
            CoreStart();
        }

        public void Stop()
        {
            CoreStop();
        }

        public void Handle(TInput inputSignal)
        {
            CoreHandle(inputSignal, null);
        }

        public void Handle(TInput inputSignal, object args)
        {
            CoreHandle(inputSignal, args);
        }

        protected abstract void CoreHandle(TInput inputSignal, object args);
        protected abstract void CoreStart();
        protected abstract void CoreStop();
    }
}
