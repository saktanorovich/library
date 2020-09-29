using System;
using System.Collections.Generic;
using System.Threading;

namespace Library.StateMachine
{
    public class StateMachineExecutorConcurrent<TState, TInput> : StateMachineExecutor<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        private sealed class InputSignal
        {
            internal TInput Signal { get; }
            internal object Args { get; }

            internal InputSignal(TInput signal, object args)
            {
                Signal = signal;
                Args = args;
            }
        }

        private readonly Queue<InputSignal> queue = new Queue<InputSignal>(1024);
        private readonly Thread executingThread;
        private EventWaitHandle startWaitHandle = new ManualResetEvent(false);
        private EventWaitHandle waitHandle = new AutoResetEvent(false);
        private volatile bool isRunning;

        public StateMachineExecutorConcurrent(IStateMachine<TState, TInput> stateMachine)
            : base(stateMachine)
        {
            executingThread = new Thread(ExecutingThreadImpl)
            {
                IsBackground = true
            };
        }

        private void Enqueue(InputSignal inputSignal)
        {
            if (isRunning)
            {
                lock (queue)
                {
                    queue.Enqueue(inputSignal);
                }
                if (waitHandle != null)
                {
                    waitHandle.Set();
                }
            }
        }

        private void ExecutingThreadImpl()
        {
            isRunning = true;
            StateMachine.Start();
            if (startWaitHandle != null)
            {
                startWaitHandle.Set();
            }
            while (true)
            {
                InputSignal inputSignal = null;
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        inputSignal = queue.Dequeue();
                        if (ReferenceEquals(inputSignal, null))
                        {
                            break;
                        }
                    }
                }
                if (inputSignal != null)
                {
                    StateMachine.Handle(inputSignal.Signal, inputSignal.Args);
                }
                else
                {
                    if (waitHandle != null)
                    {
                        waitHandle.WaitOne();
                    }
                }
            }
            isRunning = false;
        }

        protected override void CoreStart()
        {
            if (!isRunning)
            {
                executingThread.Start();
            }
            if (startWaitHandle != null)
            {
                startWaitHandle.WaitOne();
                startWaitHandle.Reset();
            }
        }

        protected override void CoreStop()
        {
            Enqueue(null);
        }

        protected override void CoreHandle(TInput inputSignal, object args)
        {
            Enqueue(new InputSignal(inputSignal, args));
        }

        protected override void DisposeImpl()
        {
            Stop();
            if (waitHandle != null)
            {
                waitHandle.Dispose();
                waitHandle = null;
            }
            if (startWaitHandle != null)
            {
                startWaitHandle.Dispose();
                startWaitHandle = null;
            }
            executingThread.Join();
            base.DisposeImpl();
        }
    }
}
