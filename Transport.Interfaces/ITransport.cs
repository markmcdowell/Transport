using System;

namespace Transport.Interfaces
{
    public interface ITransport<T> : IDisposable
    {
        IObservable<T> Observe(string topic);

        IObserver<T> Publish(string topic);
    }
}
