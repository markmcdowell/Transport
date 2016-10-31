using System;
using System.Collections.Generic;

namespace Transport.Core
{
    internal static class ObserverExtensions
    {
        /// <summary>
        /// Merges all observers in the given enumerable sequence into a single observer.
        /// </summary>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><see cref="IObserver{T}"/></returns>
        public static IObserver<T> Merge<T>(this IEnumerable<IObserver<T>> observers)
        {
            if (observers == null)
                throw new ArgumentNullException(nameof(observers));

            return new CompositeObserver<T>(observers);
        }
    }
}