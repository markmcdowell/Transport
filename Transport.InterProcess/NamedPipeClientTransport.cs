using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO.Pipes;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Transport.Interfaces;

namespace Transport.InterProcess
{
    [Export(InterProcessConstants.Transports.NamedPipeClient, typeof(ITransport<>))]
    internal sealed class NamedPipeClientTransport<T> : ITransport<T>, IPartImportsSatisfiedNotification
    {
        private readonly IAdapter<T, byte[]> _adapter;
        private readonly NamedPipeClientStream _namedPipeClientStream;

        public NamedPipeClientTransport(IAdapter<T, byte[]> adapter)
        {
            _adapter = adapter;
            _namedPipeClientStream = new NamedPipeClientStream("Transport")
            {
                ReadMode = PipeTransmissionMode.Message
            };
        }

        public void Dispose()
        {
            _namedPipeClientStream.Dispose();
        }

        public IObservable<T> Observe(string topic)
        {
            return Observable.Create<T>(o =>
            {
                var message = new List<byte>();
                var messageBuffer = new byte[5];
                do
                {
                    _namedPipeClientStream.Read(messageBuffer, 0, messageBuffer.Length);
                    message.AddRange(messageBuffer);
                    messageBuffer = new byte[messageBuffer.Length];
                }
                while (!_namedPipeClientStream.IsMessageComplete);

                var bytes = _adapter.Adapt(message.ToArray());

                o.OnNext(bytes);
                
                return Disposable.Empty;
            });
        }

        public IObserver<T> Publish(string topic)
        {
            return Observer.Create<T>(data =>
            {
                var bytes = _adapter.Adapt(data);

                _namedPipeClientStream.Write(bytes, 0, bytes.Length);
            });
        }

        public void OnImportsSatisfied()
        {
            _namedPipeClientStream.Connect();
        }
    }
}