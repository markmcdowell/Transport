using System;
using Transport.Interfaces;

namespace Transport.Core.TopicTranslators
{
    internal sealed class DelegatingTopicTranslator : ITopicTranslator
    {
        private readonly Func<string, string> _translateFunction;

        public DelegatingTopicTranslator(Func<string, string> translateFunction)
        {
            if (translateFunction == null)
                throw new ArgumentNullException(nameof(translateFunction));

            _translateFunction = translateFunction;
        }

        public string Translate(string topic)
        {
            return _translateFunction(topic);
        }
    }
}