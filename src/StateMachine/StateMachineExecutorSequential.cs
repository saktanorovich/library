using System;

namespace Library.StateMachine
{
    public class StateMachineExecutorSequential<TState, TInput> : StateMachineExecutor<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        private volatile bool isRunning;

        public StateMachineExecutorSequential(IStateMachine<TState, TInput> stateMachine)
            : base(stateMachine)
        {
        }

        protected override void CoreHandle(TInput inputSignal, object args)
        {
            StateMachine.Handle(inputSignal, args);
        }

        protected override void CoreStart()
        {
            if (!isRunning)
            {
                isRunning = true;
                StateMachine.Start();
            }
        }

        protected override void CoreStop()
        {
            isRunning = false;
        }
    }
}
