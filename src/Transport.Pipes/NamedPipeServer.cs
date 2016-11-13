using System;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace Transport.Pipes
{
    internal sealed class NamedPipeServer : IPipe
    {
        private readonly ISubject<byte[]> _messages = new Subject<byte[]>();
        private readonly byte[] _buffer = new byte[1024 * 32];
        private readonly NamedPipeServerStream _pipe;
        private readonly BooleanDisposable _disposable = new BooleanDisposable();

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
            _disposable.Dispose();
            _pipe.Dispose();
        }

        public string Name { get; }

        public PipeType PipeType => PipeType.Server;

        public void Connect()
        {
        }

        public void Send(byte[] data)
        {
            if (_pipe.IsConnected)
            {
                _pipe.BeginWrite(data, 0, data.Length, OnWriteFinished, null);
            }
        }

        public IObservable<byte[]> Receive()
        {
            if (!_pipe.IsConnected)
                _pipe.BeginWaitForConnection(OnConnection, null);
            else
                _pipe.BeginRead(_buffer, 0, _buffer.Length, OnReadFinished, null);

            return _messages;
        }

        private void OnConnection(IAsyncResult result)
        {
            try
            {
                _pipe.EndWaitForConnection(result);

                if (_pipe.IsConnected)
                    _pipe.BeginRead(_buffer, 0, _buffer.Length, OnReadFinished, null);
            }
            catch (ObjectDisposedException)
            {
                if (!_disposable.IsDisposed)
                    throw;
            }
        }

        private void OnWriteFinished(IAsyncResult result)
        {            
            _pipe.EndWrite(result);
        }

        private void OnReadFinished(IAsyncResult result)
        {
            var readLength = _pipe.EndRead(result);

            if (_disposable.IsDisposed)
                return;

            if (_pipe.IsMessageComplete)
            {
                var message = _buffer.Take(readLength).ToArray();
                _messages.OnNext(message);
            }

            if (_pipe.IsConnected)
                _pipe.BeginRead(_buffer, 0, _buffer.Length, OnReadFinished, null);
            else
                _pipe.BeginWaitForConnection(OnConnection, null);
        }
    }
}