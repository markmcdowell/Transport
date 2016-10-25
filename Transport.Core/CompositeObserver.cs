using System;
using System.Collections.Generic;

namespace Transport.Core
{
    internal sealed class CompositeObserver<T> : IObserver<T>
    {
        private readonly IEnumerable<IObserver<T>> _observers;

        public CompositeObserver(IEnumerable<IObserver<T>> observers)
        {
            if (observers == null)
                throw new ArgumentNullException(nameof(observers));

            _observers = observers;
        }

        public void OnNext(T value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in _observers)
            {
                observer.OnError(error);
            }
        }

        public void OnCompleted()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }
    }
}