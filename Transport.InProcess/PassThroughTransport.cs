using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Reactive.Subjects;
using Transport.Interfaces;

namespace Transport.InProcess
{
    [Export(typeof(ITransport<>))]
    public sealed class PassThroughTransport<T> : ITransport<T>
    {
        private readonly ConcurrentDictionary<string, ISubject<T>> _streams = new ConcurrentDictionary<string, ISubject<T>>();

        public IObservable<T> Observe(string topic)
        {
            return _streams.GetOrAdd(topic, t => new Subject<T>());
        }

        public IObserver<T> Publish(string topic)
        {
            return _streams.GetOrAdd(topic, t => new Subject<T>());
        }

        public void Dispose()
        {
            foreach (var stream in _streams)
            {
                var disposable = stream.Value as IDisposable;
                disposable?.Dispose();
            }
            _streams.Clear();
        }
    }
}