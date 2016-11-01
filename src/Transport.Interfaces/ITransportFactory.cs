using System;

namespace Transport.Interfaces
{
    /// <summary>
    /// Defines the transport factory.
    /// </summary>
    public interface ITransportFactory
    {
        /// <summary>
        /// Create a <see cref="ITransport{T}"/> from the given name and configuration.
        /// </summary>
        /// <typeparam name="T">The type of data being sent over the transport.</typeparam>
        /// <param name="name">The name of the transport to create.</param>
        /// <param name="configuration">The configuration to set on the transport.</param>
        /// <returns><see cref="ITransport{T}"/></returns>
        ITransport<T> Create<T>(string name, Func<ITransportConfiguration<T>, ITransportConfiguration<T>> configuration = null);
    }
}