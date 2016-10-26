using System.ComponentModel.Composition;
using Transport.Interfaces;

namespace Transport.InProcess
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", InProcessConstants.Names.Transport)]
    internal sealed class InProcessTransportProvider : ITransportProvider
    {
        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            return new PassThroughTransport<T>();
        }
    }
}