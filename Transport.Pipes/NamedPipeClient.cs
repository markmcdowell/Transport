using System;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
            _pipe = new NamedPipeClientStream(".", name, PipeDirection.InOut)
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
            _pipe.Connect();
        }

        public void Send(byte[] data)
        {
            _pipe.Write(data, 0, data.Length);
        }

        public IObservable<byte[]> Receive()
        {
            return Task.Factory
                       .FromAsync((callback, state) => _pipe.BeginRead(_buffer, 0, _buffer.Length, callback, state),
                                  result => _pipe.EndRead(result), this)
                       .ToObservable()
                       .Select(read => _buffer.Take(read).ToArray())
                       .Repeat();
        }        
    }
}