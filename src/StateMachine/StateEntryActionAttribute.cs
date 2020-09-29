using System;

namespace Library.StateMachine
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class StateEntryActionAttribute : Attribute
    {
    }
}
