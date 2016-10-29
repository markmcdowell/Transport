namespace Transport.Pipes
{
    internal interface IPipeProvider
    {
        IPipe GetOrCreate(string name, PipeType pipeType);
    }
}