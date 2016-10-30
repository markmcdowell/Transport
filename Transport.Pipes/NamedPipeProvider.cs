using System;
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
            return _pipes.GetOrAdd(GetKey(name, pipeType), key =>
                         {
                             var pipeName = GetName(key);
                             switch (pipeType)
                             {
                                 case PipeType.Client:
                                     return new NamedPipeClient(pipeName);
                                 default:
                                     return new NamedPipeServer(pipeName);
                             }
                         })
                         .OnDipose(key =>
                         {
                             IPipe removed;
                             _pipes.TryRemove(key, out removed);
                         })
                         .RefCount();
        }

        private static string GetKey(string name, PipeType pipeType)
        {
            return $"{name}/{pipeType}";
        }

        private static string GetName(string key)
        {
            var index = key.IndexOf("/", StringComparison.Ordinal);
            return key.Substring(0, index + 1);
        }
    }
}
