namespace Transport.Interfaces
{
    public interface ITransportConfiguration<T>
    {
        ITransportConfiguration<T> InstancePerCall();

        ITransportConfiguration<T> Configuration(string jsonConfiguration);

        ITransportConfiguration<T> Adapter(IAdapter<T, byte[]> adapter);
    }
}