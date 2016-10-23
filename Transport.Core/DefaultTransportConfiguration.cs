using Transport.Interfaces;

namespace Transport.Core
{
    public sealed class DefaultTransportConfiguration : ITransportConfiguration
    {
        private bool _singleInstance;

        public ITransportConfiguration SharedInstance()
        {
            _singleInstance = true;
            return this;
        }        
    }
}