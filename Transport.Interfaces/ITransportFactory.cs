using System;

namespace Transport.Interfaces
{
    public interface ITransportFactory
    {
        ITransport<T> Create<T>(string name, Func<ITransportConfiguration<T>, ITransportConfiguration<T>> configuration = null);
    }
}