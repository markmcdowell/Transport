using System;
using System.Threading.Tasks;

namespace Transport.Pipes
{
    internal interface IPipe : IDisposable
    {
        string Name { get; }

        event EventHandler<MessageEventArgs> MessageReceived;

        void Connect();

        Task Send(byte[] data);

        void Receive();
    }
}
