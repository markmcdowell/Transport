namespace Transport.Interfaces
{
    /// <summary>
    /// Defines the transport metadata.
    /// </summary>
    public interface ITransportMetadata
    {
        /// <summary>
        /// Gets the name of the <see cref="ITransport{T}"/>
        /// </summary>
        string Name { get; }
    }
}