using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO.Pipes;
using System.Reactive.Linq;

namespace Transport.Pipes
{
    [Export(PipeConstants.Pipes.Server, typeof(IPipe))]
    internal sealed class NamedPipeServer : IPipe
    {
        private readonly NamedPipeServerStream _pipe;

        public NamedPipeServer(string name)
        {
            _pipe = new NamedPipeServerStream(name, PipeDirection.InOut)
            {
                ReadMode = PipeTransmissionMode.Message
            };
        }

        public void Dispose()
        {
            _pipe.Dispose();
        }

        public void Send(byte[] data)
        {
            _pipe.Write(data, 0, data.Length);
        }

        public IObservable<byte[]> Receive()
        {
            return Observable.Create<byte[]>(o =>
            {
                var message = new List<byte>();
                var messageBuffer = new byte[5];
                do
                {
                    _pipe.Read(messageBuffer, 0, messageBuffer.Length);
                    message.AddRange(messageBuffer);
                    messageBuffer = new byte[messageBuffer.Length];
                }
                while (!_pipe.IsMessageComplete);

                var bytes = message.ToArray();

                o.OnNext(bytes);

                return _pipe;
            });
        }
    }
}