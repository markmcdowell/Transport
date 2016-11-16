using System.Collections.Generic;

namespace Transport.Pipes
{
    internal interface IPipeState
    {
        byte[] Buffer { get; }

        List<byte> Message { get; }
    }
}