using Transport.Interfaces;

namespace Transport.Core
{
    public sealed class DefaultTransportConfiguration : ITransportConfiguration
    {
        private bool _singleInstance;

        public ITransportConfiguration InstancePerCall()
        {
            _singleInstance = true;
            return this;
        }        
    }
}