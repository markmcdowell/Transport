using Transport.Interfaces;

namespace Transport.Core
{
    public sealed class DefaultTransportConfiguration : ITransportConfiguration
    {
        private bool _instancePerCall;

        public ITransportConfiguration InstancePerCall()
        {
            _instancePerCall = true;
            return this;
        }        
    }
}