using System;
using Transport.Interfaces;

namespace Transport.Core.Transports
{
    internal sealed class ThrowOnNullOrEmptyTransport<T> : ITransport<T>
    {
        private readonly ITransport<T> _transport;

        public ThrowOnNullOrEmptyTransport(ITransport<T> transport)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            _transport = transport;
        }

        public IObservable<T> Observe(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentNullException(nameof(topic));

            return _transport.Observe(topic);
        }

        public IObserver<T> Publish(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentNullException(nameof(topic));

            return _transport.Publish(topic);
        }
    }
}