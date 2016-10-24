using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Transport.Interfaces;

namespace Transport.InterProcess
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", InterProcessConstants.Names.Transport)]
    internal sealed class InterProcessTransportProvider : ITransportProvider
    {
        private readonly CompositionContainer _compositionContainer;

        [ImportingConstructor]
        public InterProcessTransportProvider(CompositionContainer compositionContainer)
        {
            _compositionContainer = compositionContainer;
        }

        public ITransport<T> Create<T>(Func<ITransportConfiguration, ITransportConfiguration> configuration)
        {
            return _compositionContainer.GetExportedValue<ITransport<T>>(InterProcessConstants.Transports.NamedPipeClient);
        }
    }
}