using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reactive;
using System.Reactive.Linq;
using Transport.Interfaces;

namespace Transport.InterProcess
{
    internal sealed class NamedPipeClientTransport<T> : ITransport<T>
    {
        private readonly IAdapter<T, byte[]> _adapter;

        public NamedPipeClientTransport(IAdapter<T, byte[]> adapter)
        {
            _adapter = adapter;
        }

        public IObservable<T> Observe(string topic)
        {
            return Observable.Create<T>(o =>
            {
                var pipe = new NamedPipeClientStream(".", topic, PipeDirection.In)
                {
                    ReadMode = PipeTransmissionMode.Message
                };

                var message = new List<byte>();
                var messageBuffer = new byte[5];
                do
                {
                    pipe.Read(messageBuffer, 0, messageBuffer.Length);
                    message.AddRange(messageBuffer);
                    messageBuffer = new byte[messageBuffer.Length];
                }
                while (!pipe.IsMessageComplete);

                var bytes = message.ToArray();

                var data = _adapter.Adapt(bytes);

                o.OnNext(data);
                
                return pipe;
            });
        }

        public IObserver<T> Publish(string topic)
        {
            return Observer.Create<T>(data =>
            {
                using (var pipe = new NamedPipeClientStream(".", topic, PipeDirection.Out))
                {
                    var bytes = _adapter.Adapt(data);
                    pipe.Write(bytes, 0, bytes.Length);
                }
            });
        }
    }
}