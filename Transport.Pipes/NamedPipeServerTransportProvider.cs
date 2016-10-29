using System.ComponentModel.Composition;
using Transport.Interfaces;

namespace Transport.Pipes
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", PipeConstants.Pipes.Server)]
    internal sealed class NamedPipeServerTransportProvider : ITransportProvider
    {
        private readonly IPipeProvider _pipeProvider;

        [ImportingConstructor]
        public NamedPipeServerTransportProvider([Import(PipeConstants.Pipes.Server)]IPipeProvider pipeProvider)
        {
            _pipeProvider = pipeProvider;
        }

        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            return new PipeTransport<T>(transportDetails.Adapter, _pipeProvider);
        }
    }
}