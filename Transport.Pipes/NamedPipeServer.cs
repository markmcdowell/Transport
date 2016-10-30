using System;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;

namespace Transport.Pipes
{
    internal sealed class NamedPipeServer : IPipe
    {
        private readonly byte[] _buffer = new byte[1024 * 32];
        private readonly NamedPipeServerStream _pipe;

        public NamedPipeServer(string name)
        {
            Name = name;
            _pipe = new NamedPipeServerStream(name, 
                                              PipeDirection.InOut, 
                                              NamedPipeServerStream.MaxAllowedServerInstances,
                                              PipeTransmissionMode.Message, 
                                              PipeOptions.Asynchronous);
        }

        public void Dispose()
        {
            _pipe.Dispose();
        }

        public string Name { get; }

        public event EventHandler<MessageEventArgs> MessageReceived;

        public void Connect()
        {
        }

        public async Task Send(byte[] data)
        {
            if (_pipe.IsConnected)
            {
                await Task.Factory.FromAsync((callback, state) => _pipe.BeginWrite(data, 0, data.Length, callback, state), result => _pipe.EndWrite(result), null);
            }
        }

        public void Receive()
        {
            if (!_pipe.IsConnected)
                _pipe.BeginWaitForConnection(OnConnection, null);
            else
                _pipe.BeginRead(_buffer, 0, _buffer.Length, EndRead, null);
        }

        private void OnConnection(IAsyncResult result)
        {
            _pipe.EndWaitForConnection(result);

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
            else
            {
                _pipe.BeginWaitForConnection(OnConnection, null);
            }
        }
    }
}