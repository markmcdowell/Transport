using System;

namespace Transport.Pipes
{
    internal interface IPipe : IDisposable
    {
        string Name { get; }

        PipeType PipeType { get; }

        void Connect();

        void Send(byte[] data);

        IObservable<byte[]> Receive();
    }
}
