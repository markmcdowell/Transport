using System;
using System.Threading.Tasks;

namespace Transport.Pipes
{
    internal interface IPipe : IDisposable
    {
        string Name { get; }

        void Connect();

        void Send(byte[] data);

        Task<byte[]> Receive();
    }
}
