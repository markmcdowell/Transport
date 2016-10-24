using System;

namespace Transport.Interfaces
{
    public interface ITransportFactory
    {
        ITransport<T> Create<T>(string name, Func<ITransportConfiguration, ITransportConfiguration> configuration = null);
    }
}