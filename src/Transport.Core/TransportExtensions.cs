using System;
using System.Collections.Generic;
using Transport.Core.TopicTranslators;
using Transport.Core.Transports;
using Transport.Interfaces;

namespace Transport.Core
{
    public static class TransportExtensions
    {
        public static ITransport<T> Merge<T>(this IEnumerable<ITransport<T>> transports)
        {
            if (transports == null)
                throw new ArgumentNullException(nameof(transports));

            return new CompositeTransport<T>(transports);
        }

        public static ITransport<T> Merge<T>(this ITransport<T> first, ITransport<T> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            return Merge(new[] {first, second});
        }

        public static ITransport<T> TranslateTopic<T>(this ITransport<T> transport, Func<string, string> translate)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            return transport.TranslateTopic(new DelegatingTopicTranslator(translate));
        }

        public static ITransport<T> TranslateTopic<T>(this ITransport<T> transport, ITopicTranslator topicTranslator)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            return new TopicTranslatingTransport<T>(transport, topicTranslator);
        }

        public static ITransport<T> ThrowOnNullOrEmptyTopic<T>(this ITransport<T> transport)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));

            return new ThrowOnNullOrEmptyTransport<T>(transport);
        }
    }
}