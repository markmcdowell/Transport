using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using Transport.Interfaces;

namespace Transport.InProcess
{
    internal sealed class PassThroughTransport<T> : ITransport<T>, IDisposable
    {
        private readonly ConcurrentDictionary<string, ISubject<T>> _streams = new ConcurrentDictionary<string, ISubject<T>>();
        private readonly Func<string, ISubject<T>> _streamFactory = t => new Subject<T>();

        public IObservable<T> Observe(string topic)
        {
            return _streams.GetOrAdd(topic, _streamFactory);
        }

        public IObserver<T> Publish(string topic)
        {
            return _streams.GetOrAdd(topic, _streamFactory);
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