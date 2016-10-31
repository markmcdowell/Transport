using System;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Subjects;

namespace Transport.Pipes
{
    internal sealed class NamedPipeClient : IPipe
    {
        private readonly ISubject<byte[]> _messages = new Subject<byte[]>();
        private readonly NamedPipeClientStream _pipe;
        private readonly byte[] _buffer = new byte[1024 * 16];
        private readonly TimeSpan _timeout = TimeSpan.FromMinutes(1);

        public NamedPipeClient(string name)
        {
            Name = name;
            _pipe = new NamedPipeClientStream(".", name, PipeDirection.InOut, PipeOptions.Asynchronous);            
        }

        public void Dispose()
        {
            _pipe.Dispose();
        }

        public string Name { get; }

        public void Connect()
        {
            if (_pipe.IsConnected)
                return;

            _pipe.Connect((int)_timeout.TotalMilliseconds);
            _pipe.ReadMode = PipeTransmissionMode.Message;
        }

        public void Send(byte[] data)
        {
            Connect();

            _pipe.BeginWrite(data, 0, data.Length, OnWriteFinished, null);
        }

        public IObservable<byte[]> Receive()
        {
            Connect();

            _pipe.BeginRead(_buffer, 0, _buffer.Length, OnReadFinished, null);

            return _messages;
        }

        private void OnWriteFinished(IAsyncResult result)
        {
            _pipe.EndWrite(result);
        }

        private void OnReadFinished(IAsyncResult result)
        {
            var readLength = _pipe.EndRead(result);

            if (_pipe.IsMessageComplete)
            {
                var message = _buffer.Take(readLength).ToArray();
                _messages.OnNext(message);
            }

            if (_pipe.IsConnected)
            {
                _pipe.BeginRead(_buffer, 0, _buffer.Length, OnReadFinished, null);
            }
        }
    }
}