namespace Transport.Interfaces
{
    public interface ITransportProvider
    {
        ITransport<T> Create<T>(ITransportDetails<T> transportDetails);
    }
}