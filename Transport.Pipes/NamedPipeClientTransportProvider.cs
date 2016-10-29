using System.ComponentModel.Composition;
using Transport.Interfaces;

namespace Transport.Pipes
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", PipeConstants.Pipes.Client)]
    internal sealed class NamedPipeClientTransportProvider : ITransportProvider
    {
        private readonly IPipeProvider _pipeProvider;

        [ImportingConstructor]
        public NamedPipeClientTransportProvider([Import(PipeConstants.Pipes.Client)]IPipeProvider pipeProvider)
        {
            _pipeProvider = pipeProvider;
        }

        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            return new PipeTransport<T>(transportDetails.Adapter, _pipeProvider);
        }
    }
}