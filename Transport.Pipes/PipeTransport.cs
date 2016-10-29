using System;
using System.Reactive;
using System.Reactive.Linq;
using Transport.Interfaces;

namespace Transport.Pipes
{
    internal sealed class PipeTransport<T> : ITransport<T>
    {
        private readonly IAdapter<T, byte[]> _adapter;
        private readonly IPipeProvider _pipeProvider;

        public PipeTransport(IAdapter<T, byte[]> adapter, IPipeProvider pipeProvider)
        {
            _adapter = adapter;
            _pipeProvider = pipeProvider;
        }

        public IObservable<T> Observe(string topic)
        {
            var pipe = _pipeProvider.GetOrCreate(topic);

            return pipe.Receive().Select(data => _adapter.Adapt(data));
        }

        public IObserver<T> Publish(string topic)
        {
            var pipe = _pipeProvider.GetOrCreate(topic);

            return Observer.Create<T>(data =>
            {
                var bytes = _adapter.Adapt(data);

                pipe.Send(bytes);
            }, ex => pipe.Dispose(), () => pipe.Dispose());
        }
    }
}