using System;

namespace Transport.Pipes
{
    internal interface IPipe : IDisposable
    {
        void Send(byte[] data);

        IObservable<byte[]> Receive();
    }
}
