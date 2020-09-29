using System;

namespace Library.StateMachine
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class StateExitActionAttribute : Attribute
    {
    }
}
