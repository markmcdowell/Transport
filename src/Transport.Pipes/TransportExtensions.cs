using Transport.Interfaces;

namespace Transport.Pipes
{
    internal static class TransportExtensions
    {
        public static ITransport<T> ThrowOnInvalidTopic<T>(this ITransport<T> transport)
        {
            return new ThrowOnInvalidTopicTransport<T>(transport);
        }
    }
}