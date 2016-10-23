using System;
using System.ComponentModel.Composition;
using Transport.Interfaces;

namespace Transport.InProcess
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", "InProcess")]
    public sealed class PassThroughTransportProvider : ITransportProvider
    {
        

        public ITransport<T> Create<T>(Func<ITransportConfiguration, ITransportConfiguration> configuration)
        {
            return new PassThroughTransport<T>();
        }
    }
}