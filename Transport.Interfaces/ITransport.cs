using System;

namespace Transport.Interfaces
{
    public interface ITransport<T>
    {
        IObservable<T> Observe(string topic);

        IObserver<T> Publish(string topic);
    }
}
