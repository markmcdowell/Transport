using System.ComponentModel.Composition;
using Transport.Interfaces;

namespace Transport.Pipes
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", PipeConstants.Transports.Server)]
    internal sealed class NamedPipeServerTransportProvider : ITransportProvider
    {
        private readonly IPipeProvider _pipeProvider;

        [ImportingConstructor]
        public NamedPipeServerTransportProvider([Import]IPipeProvider pipeProvider)
        {
            _pipeProvider = pipeProvider;
        }

        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            var transport = new PipeTransport<T>(transportDetails.Adapter, _pipeProvider, PipeType.Server);

            return transport.ThrowOnInvalidTopic();
        }
    }
}