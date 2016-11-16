using System;
using Transport.Interfaces;

namespace Transport.Pipes
{
    internal sealed class ThrowOnInvalidTopicTransport<T> : ITransport<T>
    {
        private readonly ITransport<T> _transport;

        public ThrowOnInvalidTopicTransport(ITransport<T> transport)
        {
            _transport = transport;
        }

        public IObservable<T> Observe(string topic)
        {
            if (topic.StartsWith(@"/"))
                throw new ArgumentOutOfRangeException(nameof(topic), @"Pipe topics cannot start with /");
            if (topic.Contains(@"\"))
                throw new ArgumentOutOfRangeException(nameof(topic), @"Pipe topics cannot contain a \");

            return _transport.Observe(topic);
        }

        public IObserver<T> Publish(string topic)
        {
            if (topic.StartsWith(@"/"))
                throw new ArgumentOutOfRangeException(nameof(topic), @"Pipe topics cannot start with /");
            if (topic.Contains(@"\"))
                throw new ArgumentOutOfRangeException(nameof(topic), @"Pipe topics cannot contain a \");

            return _transport.Publish(topic);
        }
    }
}