using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Transport.Interfaces;
using Transport.Reactive;

namespace Transport.Core.Transports
{
    internal sealed class CompositeTransport<T> : ITransport<T>
    {
        private readonly IEnumerable<ITransport<T>> _transports;

        public CompositeTransport(IEnumerable<ITransport<T>> transports)
        {
            if (transports == null)
                throw new ArgumentNullException(nameof(transports));

            _transports = transports.ToList();
        }

        public IObservable<T> Observe(string topic)
        {
            return _transports.Select(t => t.Observe(topic)).Merge();
        }

        public IObserver<T> Publish(string topic)
        {
            return _transports.Select(t => t.Publish(topic)).Merge();
        }
    }
}