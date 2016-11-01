namespace Transport.Interfaces
{
    /// <summary>
    /// Defines the transport provider.
    /// </summary>
    public interface ITransportProvider
    {
        /// <summary>
        /// Create a <see cref="ITransport{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type data being sent over the transport.</typeparam>
        /// <param name="transportDetails">The transport details.</param>
        /// <returns><see cref="ITransport{T}"/></returns>
        ITransport<T> Create<T>(ITransportDetails<T> transportDetails);
    }
}