namespace Transport.Interfaces
{
    /// <summary>
    /// Defines an adapter.
    /// </summary>
    /// <typeparam name="TFrom">Adapt from type.</typeparam>
    /// <typeparam name="TTo">Adapt to type.</typeparam>
    public interface IAdapter<TFrom, TTo>
    {
        TTo Adapt(TFrom from);

        TFrom Adapt(TTo to);
    }
}