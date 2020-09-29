using System;
using System.Threading;

namespace Library
{
    public static class EventUtils
    {
        public static void Raise(this EventHandler eventHandler, object sender, EventArgs args)
        {
            var handler = Interlocked.CompareExchange(ref eventHandler, null, null);
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs args)
            where TEventArgs : EventArgs
        {
            var handler = Interlocked.CompareExchange(ref eventHandler, null, null);
            if (handler != null)
            {
                handler(sender, args);
            }
        }
    }
}