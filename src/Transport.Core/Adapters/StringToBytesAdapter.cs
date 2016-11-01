using System.Text;
using Transport.Interfaces;

namespace Transport.Core.Adapters
{
    public sealed class StringToBytesAdapter : IAdapter<string, byte[]>
    {
        public byte[] Adapt(string from)
        {
            return Encoding.UTF8.GetBytes(from);
        }

        public string Adapt(byte[] to)
        {
            return Encoding.UTF8.GetString(to);
        }
    }
}