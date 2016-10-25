using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Transport.Interfaces;

namespace Transport.Core.Transports
{
    public sealed class CompositeTransport<T> : ITransport<T>
    {
        private readonly IEnumerable<ITransport<T>> _transports;
        private readonly CompositeDisposable _disposable;

        public CompositeTransport(IEnumerable<ITransport<T>> transports)
        {
            _transports = transports.ToList();
            _disposable = new CompositeDisposable(_transports);
        }

        public void Dispose()
        {
            _disposable.Dispose();
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