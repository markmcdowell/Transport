namespace Transport.Interfaces
{
    /// <summary>
    /// Defines a topic translator.
    /// </summary>
    public interface ITopicTranslator
    {
        /// <summary>
        /// Translate the given topic.
        /// </summary>
        /// <param name="topic">The topic to translate.</param>
        /// <returns>The translated topic.</returns>
        string Translate(string topic);
    }
}