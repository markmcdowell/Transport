using System.Collections.Generic;

namespace Transport.Pipes
{
    internal sealed class PipeState : IPipeState
    {
        public byte[] Buffer { get; } = new byte[1024 * 16];

        public List<byte> Message { get; } = new List<byte>();        
    }
}