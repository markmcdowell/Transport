using System.ComponentModel.Composition;
using Transport.Interfaces;

namespace Transport.InterProcess
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", InterProcessConstants.Names.Transport)]
    internal sealed class InterProcessTransportProvider : ITransportProvider
    {
        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            return new NamedPipeClientTransport<T>(transportDetails.Adapter);
        }
    }
}