namespace Transport.Interfaces
{
    /// <summary>
    /// Defines the <see cref="ITransport{T}"/> details.
    /// </summary>
    /// <typeparam name="T">The type of data being sent over the transport.</typeparam>
    public interface ITransportDetails<T>
    {
        string JsonConfiguration { get; }

        IAdapter<T,byte[]> Adapter { get; }
    }
}