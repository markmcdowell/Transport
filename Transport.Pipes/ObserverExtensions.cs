using System;

namespace Transport.Pipes
{
    internal static class ObserverExtensions
    {
        public static IObserver<T> Finally<T>(this IObserver<T> observer, Action finallyAction)
        {
            return new FinallyObserver<T>(observer, finallyAction);
        }
    }
}