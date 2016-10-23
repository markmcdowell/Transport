namespace Transport.Interfaces
{
    public interface IAdapter<TFrom, TTo>
    {
        TTo Adapt(TFrom from);

        TFrom Adapt(TTo to);
    }
}