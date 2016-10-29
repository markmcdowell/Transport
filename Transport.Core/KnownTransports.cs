namespace Transport.Core
{
    public static class KnownTransports
    {
        public static class InProcess
        {
            public const string PassThrough = "InProcess.PassThrough";

            public const string ReplayLast = "InProcess.ReplayLast";
        }

        public static class Pipes
        {
            public const string Client = "NamedPipe.Client";

            public const string Server = "NamedPipe.Server";
        }
    }
}