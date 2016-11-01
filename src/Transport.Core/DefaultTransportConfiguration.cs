using Transport.Core.Adapters;
using Transport.Interfaces;

namespace Transport.Core
{
    internal sealed class DefaultTransportConfiguration<T> : ITransportConfiguration<T>, ITransportDetails<T>
    {
        private IAdapter<T, byte[]> _adapter;
        private string _jsonConfiguration;

        public DefaultTransportConfiguration()
        {
            var objectToString = new ObjectToJsonAdapter<T>();
            var stringToBytes = new StringToBytesAdapter();
            _adapter = new IntermediateAdapter<T, string, byte[]>(objectToString, stringToBytes);
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

        string ITransportDetails<T>.JsonConfiguration => _jsonConfiguration;

        IAdapter<T, byte[]> ITransportDetails<T>.Adapter => _adapter;
    }
}