using System;

namespace Library.StateMachine
{
    public class State<TState, TInput> : IState<TState, TInput>
        where TState : IComparable
        where TInput : IComparable
    {
        private readonly IActionHolderCollection<TState, TInput> entryActions;
        private readonly IActionHolderCollection<TState, TInput> exitActions;
        private readonly ITransitionDictionary<TState, TInput> transitions;

        public State(TState id)
        {
            Id = id;
            entryActions = new ActionHolderCollection<TState, TInput>();
            exitActions = new ActionHolderCollection<TState, TInput>();
            transitions = new TransitionDictionary<TState, TInput>();
        }

        public TState Id { get; }

        public bool IsFinal => (transitions.Count == 0);

        public ITransitionDictionary<TState, TInput> Transitions => transitions;

        public void AddEntryAction(IActionHolder<TState, TInput> actionHolder)
        {
            entryActions.Add(actionHolder);
        }

        public void AddExitAction(IActionHolder<TState, TInput> actionHolder)
        {
            exitActions.Add(actionHolder);
        }

        public void Entry(IStateMachineContext<TState, TInput> context)
        {
            CoreEntryImpl(context);
        }

        public void Exit(IStateMachineContext<TState, TInput> context)
        {
            CoreExitImpl(context);
        }

        protected virtual void CoreEntryImpl(IStateMachineContext<TState, TInput> context)
        {
            foreach (var entryAction in entryActions)
            {
                try
                {
                    entryAction.Execute(context);
                }
                catch (Exception e)
                {
                    context.HandleException(e);
                }
            }
        }

        protected virtual void CoreExitImpl(IStateMachineContext<TState, TInput> context)
        {
            foreach (var exitAction in exitActions)
            {
                try
                {
                    exitAction.Execute(context);
                }
                catch (Exception e)
                {
                    context.HandleException(e);
                }
            }
        }
    }
}
