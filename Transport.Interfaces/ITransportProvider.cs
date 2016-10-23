using System;

namespace Transport.Interfaces
{
    public interface ITransportProvider
    {
        ITransport<T> Create<T>(Func<ITransportConfiguration, ITransportConfiguration> configuration);
    }
}