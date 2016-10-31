using System;
using Transport.Interfaces;

namespace Transport.Core.Transports
{
    internal sealed class TopicTranslatingTransport<T> : ITransport<T>
    {
        private readonly ITransport<T> _transport;
        private readonly ITopicTranslator _topicTranslator;

        public TopicTranslatingTransport(ITransport<T> transport, ITopicTranslator topicTranslator)
        {
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));
            if (topicTranslator == null)
                throw new ArgumentNullException(nameof(topicTranslator));

            _transport = transport;
            _topicTranslator = topicTranslator;
        }

        public IObservable<T> Observe(string topic)
        {
            var newTopic = _topicTranslator.Translate(topic);

            return _transport.Observe(newTopic);
        }

        public IObserver<T> Publish(string topic)
        {
            var newTopic = _topicTranslator.Translate(topic);

            return _transport.Publish(newTopic);
        }
    }
}