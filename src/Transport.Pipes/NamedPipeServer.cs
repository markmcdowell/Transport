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
            if (_pipe.IsConnected)
                return;

            _pipe.BeginWaitForConnection(OnConnection, null);
        }

        public void Send(byte[] data)
        {
            _pipe.BeginWrite(data, 0, data.Length, OnWriteFinished, null);
        }

        public IObservable<byte[]> Receive()
        {
            Connect();

            return _messages;
        }

        private void OnConnection(IAsyncResult result)
        {
            try
            {
                _pipe.EndWaitForConnection(result);

                if (!_pipe.IsConnected)
                    return;

                var pipeState = new PipeState();
                _pipe.BeginRead(pipeState.Buffer, 0, pipeState.Buffer.Length, OnReadFinished, pipeState);
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
            var pipeState = (IPipeState)result.AsyncState;
            var readLength = _pipe.EndRead(result);

            if (_disposable.IsDisposed)
                return;

            var intermediateMessage = pipeState.Buffer.Take(readLength);
            pipeState.Message.AddRange(intermediateMessage);

            if (_pipe.IsMessageComplete)
            {
                var message = pipeState.Message.ToArray();
                _messages.OnNext(message);
                pipeState.Message.Clear();
            }

            if (_pipe.IsConnected)
                _pipe.BeginRead(pipeState.Buffer, 0, pipeState.Buffer.Length, OnReadFinished, pipeState);
            else
                _pipe.BeginWaitForConnection(OnConnection, pipeState);
        }
    }
}