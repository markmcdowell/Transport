using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Transport.Interfaces;

namespace Transport.InProcess
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", InProcessConstants.Names.Transport)]
    internal sealed class InProcessTransportProvider : ITransportProvider
    {
        private readonly CompositionContainer _compositionContainer;

        [ImportingConstructor]
        public InProcessTransportProvider(CompositionContainer compositionContainer)
        {
            _compositionContainer = compositionContainer;
        }

        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            return _compositionContainer.GetExportedValue<ITransport<T>>(InProcessConstants.Transports.PassThrough);
        }
    }
}