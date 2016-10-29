using System.ComponentModel.Composition;
using System.Reactive.Subjects;
using Transport.Interfaces;

namespace Transport.InProcess
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", InProcessConstants.Transports.PassThrough)]
    internal sealed class PassThroughTransportProvider : ITransportProvider
    {
        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            return new InProcessTransport<T>(topic => new Subject<T>());
        }
    }
}