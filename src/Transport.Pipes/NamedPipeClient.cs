using System;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace Transport.Pipes
{
    internal sealed class NamedPipeClient : IPipe
    {
        private readonly ISubject<byte[]> _messages = new Subject<byte[]>();
        private readonly NamedPipeClientStream _pipe;
        private readonly TimeSpan _timeout = TimeSpan.FromMinutes(1);
        private readonly BooleanDisposable _disposable = new BooleanDisposable();

        public NamedPipeClient(string name)
        {
            Name = name;
            _pipe = new NamedPipeClientStream(".", name, PipeDirection.InOut, PipeOptions.Asynchronous);            
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _pipe.Dispose();
        }

        public string Name { get; }

        public PipeType PipeType => PipeType.Client;

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

            var pipeState = new PipeState();
            _pipe.BeginRead(pipeState.Buffer, 0, pipeState.Buffer.Length, OnReadFinished, pipeState);

            return _messages;
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

            var message = pipeState.Buffer.Take(readLength);
            pipeState.Message.AddRange(message);

            if (_pipe.IsMessageComplete)
            {
                var completeMessage = pipeState.Message.ToArray();
                _messages.OnNext(completeMessage);
                pipeState.Message.Clear();
            }

            if (_pipe.IsConnected)
            {
                _pipe.BeginRead(pipeState.Buffer, 0, pipeState.Buffer.Length, OnReadFinished, null);
            }
        }
    }
}