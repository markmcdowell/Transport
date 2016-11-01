using System;

namespace Transport.Interfaces
{
    /// <summary>
    /// Defines a transport.
    /// </summary>
    /// <typeparam name="T">The type to send over the transport.</typeparam>
    public interface ITransport<T>
    {
        /// <summary>
        /// Observe a data stream on the given topic.
        /// </summary>
        /// <param name="topic">The topic to subscribe to.</param>
        /// <returns>An <see cref="IObservable{T}"/>.</returns>
        IObservable<T> Observe(string topic);

        /// <summary>
        /// Publish data to the given topic.
        /// </summary>
        /// <param name="topic">The topic to publish to.</param>
        /// <returns>An <see cref="IObserver{T}"/>.</returns>
        IObserver<T> Publish(string topic);
    }
}
