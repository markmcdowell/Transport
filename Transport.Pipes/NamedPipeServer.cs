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
            _pipe = new NamedPipeServerStream(name, PipeDirection.InOut)
            {
                ReadMode = PipeTransmissionMode.Message
            };
        }

        public void Dispose()
        {
            _pipe.Dispose();
        }

        public string Name { get; }

        public void Connect()
        {
        }

        public void Send(byte[] data)
        {
            if (_pipe.IsConnected)
                _pipe.Write(data, 0, data.Length);
        }

        public async Task<byte[]> Receive()
        {
            if (!_pipe.IsConnected)
                await Task.Factory
                          .FromAsync((callback, state) => _pipe.BeginWaitForConnection(callback, state),
                                      result => _pipe.EndWaitForConnection(result), null);

            var read = await Task.Factory
                                 .FromAsync((callback, state) => _pipe.BeginRead(_buffer, 0, _buffer.Length, callback, state),
                                                         result => _pipe.EndRead(result), null);

            return _buffer.Take(read).ToArray();
        }
    }
}