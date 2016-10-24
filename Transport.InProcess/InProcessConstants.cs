namespace Transport.InProcess
{
    internal static class InProcessConstants
    {
        public static class Names
        {
            public const string Transport = "InProcess";
        }

        public static class Transports
        {
            public const string PassThrough = Names.Transport + ".PassThrough";
        }
    }
}