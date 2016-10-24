using Transport.Core.Transports;
using Transport.Interfaces;

namespace Transport.Core
{
    public static class TransportExtensions
    {
        public static ITransport<T> TranslateTopic<T>(this ITransport<T> transport, ITopicTranslator topicTranslator)
        {
            return new TopicTranslatingTransport<T>(transport, topicTranslator);
        }

        public static ITransport<T> ThrowOnNullOrEmptyTopic<T>(this ITransport<T> transport)
        {
            return new ThrowOnNullOrEmptyTransport<T>(transport);
        }
    }
}