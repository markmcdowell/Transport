using Transport.Interfaces;

namespace Transport.Core.Adapters
{
    public sealed class PassThroughAdapter<T> : IAdapter<T, T>
    {
        public T Adapt(T from) => from;

        T IAdapter<T, T>.Adapt(T to) => to;
    }
}