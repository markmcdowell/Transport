using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Transport.Interfaces;

namespace Transport.Pipes
{
    internal sealed class PipeTransport<T> : ITransport<T>
    {
        private readonly IAdapter<T, byte[]> _adapter;
        private readonly IPipeProvider _pipeProvider;
        private readonly PipeType _pipeType;

        public PipeTransport(IAdapter<T, byte[]> adapter, IPipeProvider pipeProvider, PipeType pipeType)
        {
            _adapter = adapter;
            _pipeProvider = pipeProvider;
            _pipeType = pipeType;
        }

        public IObservable<T> Observe(string topic)
        {
            var pipe = _pipeProvider.GetOrCreate(topic, _pipeType);

            return Observable.Create<T>(o =>
            {
                pipe.Receive();
                var subscription = Observable.FromEventPattern<MessageEventArgs>(h => pipe.MessageReceived += h, h => pipe.MessageReceived -= h)
                                             .Select(_ => _adapter.Adapt(_.EventArgs.Message))
                                             .Subscribe(o);

                return Disposable.Create(() =>
                {
                    subscription.Dispose();
                    pipe.Dispose();
                });
            });
        }

        public IObserver<T> Publish(string topic)
        {
            var pipe = _pipeProvider.GetOrCreate(topic, _pipeType);

            return Observer.Create<T>(data =>
            {
                var bytes = _adapter.Adapt(data);

                pipe.Send(bytes);
            }, ex => pipe.Dispose(), () => pipe.Dispose());
        }
    }
}