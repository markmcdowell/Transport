using System.ComponentModel.Composition;
using System.Reactive.Subjects;
using Transport.Interfaces;

namespace Transport.InProcess
{
    [Export(typeof(ITransportProvider))]
    [ExportMetadata("Name", InProcessConstants.Transports.ReplayLast)]
    internal sealed class ReplayLastTransportProvider : ITransportProvider
    {
        public ITransport<T> Create<T>(ITransportDetails<T> transportDetails)
        {
            return new InProcessTransport<T>(topic => new ReplaySubject<T>(1));
        }
    }
}