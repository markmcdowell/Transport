using System;

namespace Transport.Pipes
{
    internal static class ObserverExtensions
    {
        /// <summary>
        /// Invokes a specified action after the observer terminates gracefully or expectionally.
        /// </summary>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <param name="observer">The source.</param>
        /// <param name="finallyAction">Action to invoke after the observer terminates.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><see cref="IObserver{T}"/></returns>
        public static IObserver<T> Finally<T>(this IObserver<T> observer, Action finallyAction)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return new FinallyObserver<T>(observer, finallyAction);
        }
    }
}