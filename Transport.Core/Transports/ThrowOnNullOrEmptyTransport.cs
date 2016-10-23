﻿using System;
using Transport.Interfaces;

namespace Transport.Core.Transports
{
    public sealed class ThrowOnNullOrEmptyTransport<T> : ITransport<T>
    {
        private readonly ITransport<T> _transportImplementation;

        public ThrowOnNullOrEmptyTransport(ITransport<T> transportImplementation)
        {
            _transportImplementation = transportImplementation;
        }

        public void Dispose()
        {
            _transportImplementation.Dispose();
        }

        public IObservable<T> Observe(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentNullException(nameof(topic));

            return _transportImplementation.Observe(topic);
        }

        public IObserver<T> Publish(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentNullException(nameof(topic));

            return _transportImplementation.Publish(topic);
        }
    }
}