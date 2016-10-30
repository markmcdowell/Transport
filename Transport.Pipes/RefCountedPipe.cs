using System;
using System.Threading.Tasks;

namespace Transport.Pipes
{
    internal sealed class RefCountedPipe : IPipe
    {
        private readonly IPipe _pipe;
        private readonly object _gate = new object();
        private int _count;

        public RefCountedPipe(IPipe pipe)
        {
            _pipe = pipe;
        }

        public void Dispose()
        {
            lock (_gate)
            {
                --_count;
                if (_count != 0)
                    return;
            }

            _pipe.Dispose();
        }

        public string Name => _pipe.Name;

        public event EventHandler<MessageEventArgs> MessageReceived
        {
            add { _pipe.MessageReceived += value; }
            remove { _pipe.MessageReceived -= value; }
        }

        public void Connect()
        {
            _pipe.Connect();
        }

        public Task Send(byte[] data)
        {
            lock (_gate)
                ++_count;

            return _pipe.Send(data);
        }

        public void Receive()
        {
            lock (_gate)
                ++_count;

            _pipe.Receive();
        }
    }
}