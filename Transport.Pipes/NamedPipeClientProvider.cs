using System.Collections.Concurrent;
using System.ComponentModel.Composition;

namespace Transport.Pipes
{
    [Export(PipeConstants.Pipes.Client, typeof(IPipeProvider))]
    internal sealed class NamedPipeClientProvider : IPipeProvider
    {
        private readonly ConcurrentDictionary<string, IPipe> _pipes = new ConcurrentDictionary<string, IPipe>();

        public IPipe GetOrCreate(string name)
        {
            return _pipes.GetOrAdd(name, n => new NamedPipeClient(n));
        }
    }
}
