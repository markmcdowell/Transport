using Transport.Core.Adapters;
using Transport.Interfaces;

namespace Transport.Core
{
    internal sealed class DefaultTransportConfiguration<T> : ITransportConfiguration<T>, ITransportDetails<T>
    {
        private bool _instancePerCall;
        private IAdapter<T, byte[]> _adapter;
        private string _jsonConfiguration;

        public DefaultTransportConfiguration()
        {
            var objectToString = new ObjectToJsonAdapter<T>();
            var stringToBytes = new StringToBytesAdapter();
            _adapter = new IntermediateAdapter<T, string, byte[]>(objectToString, stringToBytes);
        }

        public ITransportConfiguration<T> InstancePerCall()
        {
            _instancePerCall = true;
            return this;
        }

        public ITransportConfiguration<T> Configuration(string jsonConfiguration)
        {
            _jsonConfiguration = jsonConfiguration;
            return this;
        }

        public ITransportConfiguration<T> Adapter(IAdapter<T, byte[]> adapter)
        {
            _adapter = adapter;
            return this;
        }

        bool ITransportDetails<T>.InstancePerCall => _instancePerCall;

        string ITransportDetails<T>.JsonConfiguration => _jsonConfiguration;

        IAdapter<T, byte[]> ITransportDetails<T>.Adapter => _adapter;
    }
}