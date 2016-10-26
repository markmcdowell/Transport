namespace Transport.InterProcess
{
    internal static class InterProcessConstants
    {
        public static class Names
        {
            public const string Transport = "InterProcess";
        }

        public static class Transports
        {
            public const string NamedPipeClient = "NamedPipe.Client";

            public const string NamedPipeServer = "NamedPipe.Server";
        }
    }
}