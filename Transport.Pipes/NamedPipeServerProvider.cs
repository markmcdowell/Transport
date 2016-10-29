using System.Collections.Concurrent;
using System.ComponentModel.Composition;

namespace Transport.Pipes
{
    [Export(PipeConstants.Pipes.Server, typeof(IPipeProvider))]
    internal sealed class NamedPipeServerProvider : IPipeProvider
    {
        private readonly ConcurrentDictionary<string, IPipe> _pipes = new ConcurrentDictionary<string, IPipe>();

        public IPipe GetOrCreate(string name)
        {
            return _pipes.GetOrAdd(name, n => new NamedPipeClient(n));
        }
    }
}