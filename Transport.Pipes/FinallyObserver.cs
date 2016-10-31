using System;

namespace Transport.Pipes
{
    internal sealed class FinallyObserver<T> : IObserver<T>
    {
        private readonly IObserver<T> _observer;
        private readonly Action _finallyAction;

        public FinallyObserver(IObserver<T> observer, Action finallyAction)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            _observer = observer;
            _finallyAction = finallyAction;
        }

        public void OnNext(T value)
        {
            _observer.OnNext(value);
        }

        public void OnError(Exception error)
        {
            _observer.OnError(error);
            _finallyAction();
        }

        public void OnCompleted()
        {
            _observer.OnCompleted();
            _finallyAction();
        }
    }
}