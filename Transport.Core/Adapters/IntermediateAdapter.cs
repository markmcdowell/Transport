using Transport.Interfaces;

namespace Transport.Core.Adapters
{
    public sealed class IntermediateAdapter<TFrom, TIntermediate, TTo> : IAdapter<TFrom, TTo>
    {
        private readonly IAdapter<TFrom, TIntermediate> _firstAdapter;
        private readonly IAdapter<TIntermediate, TTo> _secondAdapter;

        public IntermediateAdapter(IAdapter<TFrom, TIntermediate> firstAdapter, IAdapter<TIntermediate, TTo> secondAdapter)
        {
            _firstAdapter = firstAdapter;
            _secondAdapter = secondAdapter;
        }

        public TTo Adapt(TFrom from)
        {
            var intermediate = _firstAdapter.Adapt(from);

            return _secondAdapter.Adapt(intermediate);
        }

        public TFrom Adapt(TTo to)
        {
            var intermediate = _secondAdapter.Adapt(to);

            return _firstAdapter.Adapt(intermediate);
        }
    }
}