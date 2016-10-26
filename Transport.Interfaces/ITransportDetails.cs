namespace Transport.Interfaces
{
    public interface ITransportDetails<T>
    {
        bool InstancePerCall { get; }

        string JsonConfiguration { get; }

        IAdapter<T,byte[]> Adapter { get; }
    }
}