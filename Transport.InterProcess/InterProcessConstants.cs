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
            public const string NamedPipeClient = "NamedPipeClient";

            public const string NamedPipeServer = "NamedPipeServer";
        }
    }
}