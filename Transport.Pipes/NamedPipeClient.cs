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
            _pipe = new NamedPipeClientStream(".", name, PipeDirection.InOut, PipeOptions.Asynchronous)
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
            if (!_pipe.IsConnected)
                _pipe.Connect();
        }

        public async Task Send(byte[] data)
        {
            Connect();

            await Task.Factory.FromAsync((callback, state) => _pipe.BeginWrite(data, 0, data.Length, callback, state),
                                         result => _pipe.EndWrite(result), null);
        }

        public async Task<byte[]> Receive()
        {
            Connect();

            var length = await Task.Factory.FromAsync((callback, state) => _pipe.BeginRead(_buffer, 0, _buffer.Length, callback, state),
                                                      result => _pipe.EndRead(result), null);

            return _buffer.Take(length).ToArray();
        }        
    }
}