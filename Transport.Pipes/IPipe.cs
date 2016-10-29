using System;

namespace Transport.Pipes
{
    internal interface IPipe : IDisposable
    {
        string Name { get; }

        void Connect();

        void Send(byte[] data);

        IObservable<byte[]> Receive();
    }
}
