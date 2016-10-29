using System.Collections.Concurrent;
using System.ComponentModel.Composition;

namespace Transport.Pipes
{
    [Export(typeof(IPipeProvider))]
    internal sealed class NamedPipeProvider : IPipeProvider
    {
        private readonly ConcurrentDictionary<string, IPipe> _pipes = new ConcurrentDictionary<string, IPipe>();

        public IPipe GetOrCreate(string name, PipeType pipeType)
        {
            return _pipes.GetOrAdd(name, n =>
                         {
                             switch (pipeType)
                             {
                                 case PipeType.Client:
                                     return new NamedPipeClient(n);
                                 default:
                                     return new NamedPipeServer(n);
                             }
                         })
                         .OnDipose(n =>
                         {
                             IPipe removed;
                             _pipes.TryRemove(n, out removed);
                         })
                         .RefCount();
        }
    }
}
