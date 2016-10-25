using System;
using System.Collections.Generic;

namespace Transport.Core
{
    internal static class ObserverExtensions
    {
        public static IObserver<T> Merge<T>(this IEnumerable<IObserver<T>> observers)
        {
            if (observers == null)
                throw new ArgumentNullException(nameof(observers));

            return new CompositeObserver<T>(observers);
        }
    }
}