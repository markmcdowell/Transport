using System;
using System.ComponentModel.Composition.Hosting;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Transport.Core;
using Transport.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Transport.Pipes.Tests
{
    public sealed class IntegrationTests
    {
        private readonly ITestOutputHelper _output;

        public IntegrationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldBeAbleToCreateTransport()
        {
            var inProcessCatalog = new AssemblyCatalog(typeof(PipeTransport<>).Assembly);
            var coreCatalog = new AssemblyCatalog(typeof(TransportExtensions).Assembly);
            var catalog = new AggregateCatalog(inProcessCatalog, coreCatalog);

            var container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);

            var transportFactory = container.GetExportedValue<ITransportFactory>();

            var manualResetEvent = new ManualResetEvent(false);

            var serverTransport = transportFactory.Create<string>(KnownTransports.Pipes.Server);
            serverTransport.Observe("topic/new")
                           .Subscribe(s =>
                           {
                               serverTransport.Publish("topic/new")
                                              .OnNext(s);
                           });

            var clientTransport = transportFactory.Create<string>(KnownTransports.Pipes.Client);
            clientTransport.Observe("topic/new")
                           .Subscribe(s =>
                           {
                               _output.WriteLine(s);
                               manualResetEvent.Set();
                           });

            clientTransport.Publish("topic/new")
                           .OnNext("hello!");

            manualResetEvent.WaitOne();
        }
    }
}
