namespace Transport.Interfaces
{
    /// <summary>
    /// Defines the <see cref="ITransport{T}"/> configuration.
    /// </summary>
    /// <typeparam name="T">The type of data being sent over the transport.</typeparam>
    public interface ITransportConfiguration<T>
    {
        /// <summary>
        /// Specify a json string defining the transport configuration.
        /// </summary>
        /// <param name="jsonConfiguration">The transport configuration.</param>
        /// <returns><see cref="ITransportConfiguration{T}"/>.</returns>
        ITransportConfiguration<T> Configuration(string jsonConfiguration);

        /// <summary>
        /// Specify a custom adapter to adapt the data with.
        /// </summary>
        /// <param name="adapter">The adapter to use.</param>
        /// <returns><see cref="ITransportConfiguration{T}"/>.</returns>
        ITransportConfiguration<T> Adapter(IAdapter<T, byte[]> adapter);
    }
}