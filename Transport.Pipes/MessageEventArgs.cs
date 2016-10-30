using System;

namespace Transport.Pipes
{
    internal sealed class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(byte[] message)
        {
            Message = message;
        }

        public byte[] Message { get; }
    }
}