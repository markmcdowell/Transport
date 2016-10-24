using System;
using Transport.Interfaces;

namespace Transport.Core.Transports
{
    internal sealed class TopicTranslatingTransport<T> : ITransport<T>
    {
        private readonly ITransport<T> _transportImplementation;
        private readonly ITopicTranslator _topicTranslator;

        public TopicTranslatingTransport(ITransport<T> transportImplementation, ITopicTranslator topicTranslator)
        {
            _transportImplementation = transportImplementation;
            _topicTranslator = topicTranslator;
        }

        public void Dispose()
        {
            _transportImplementation.Dispose();
        }

        public IObservable<T> Observe(string topic)
        {
            var newTopic = _topicTranslator.Translate(topic);

            return _transportImplementation.Observe(newTopic);
        }

        public IObserver<T> Publish(string topic)
        {
            var newTopic = _topicTranslator.Translate(topic);

            return _transportImplementation.Publish(newTopic);
        }
    }
}