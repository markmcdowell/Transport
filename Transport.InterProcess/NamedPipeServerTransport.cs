using System;
using System.ComponentModel.Composition;
using System.IO.Pipes;
using System.Reactive;
using System.Reactive.Linq;
using Transport.Interfaces;

namespace Transport.InterProcess
{
    [Export(InterProcessConstants.Transports.NamedPipeServer, typeof(ITransport<>))]
    internal sealed class NamedPipeServerTransport<T> : ITransport<T>
    {
        public void Dispose()
        {
        }

        public IObservable<T> Observe(string topic)
        {
            return Observable.Create<T>(o =>
            {
                var pipe = new NamedPipeServerStream(topic);
                pipe.WaitForConnection();

                return pipe;
            });
        }

        public IObserver<T> Publish(string topic)
        {
            return Observer.Create<T>(data =>
            {
                var pipe = new NamedPipeServerStream(topic);

            });
        }
    }
}