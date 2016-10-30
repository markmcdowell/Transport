using System;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;

namespace Transport.Pipes
{
    internal sealed class NamedPipeClient : IPipe
    {
        private readonly NamedPipeClientStream _pipe;
        private readonly byte[] _buffer = new byte[1024 * 16];

        public NamedPipeClient(string name)
        {
            Name = name;
            _pipe = new NamedPipeClientStream(".", name, PipeDirection.InOut, PipeOptions.Asynchronous);            
        }

        public event EventHandler<MessageEventArgs> MessageReceived;

        public void Dispose()
        {
            _pipe.Dispose();
        }

        public string Name { get; }

        public void Connect()
        {
            if (_pipe.IsConnected)
                return;

            _pipe.Connect();
            _pipe.ReadMode = PipeTransmissionMode.Message;
        }

        public Task Send(byte[] data)
        {
            Connect();

            return Task.Factory.FromAsync((callback, state) => _pipe.BeginWrite(data, 0, data.Length, callback, state),
                                          result => _pipe.EndWrite(result), null);
        }

        public void Receive()
        {
            Connect();

            _pipe.BeginRead(_buffer, 0, _buffer.Length, EndRead, null);
        }

        private void EndRead(IAsyncResult result)
        {
            var readLength = _pipe.EndRead(result);

            if (_pipe.IsMessageComplete)
            {
                var message = _buffer.Take(readLength).ToArray();
                MessageReceived?.Invoke(this, new MessageEventArgs(message));
            }

            if (_pipe.IsConnected)
            {
                _pipe.BeginRead(_buffer, 0, _buffer.Length, EndRead, null);
            }
        }
    }
}