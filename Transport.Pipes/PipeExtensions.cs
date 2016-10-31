using System;
using System.Reactive.Disposables;

namespace Transport.Pipes
{
    internal static class PipeExtensions
    {
        public static IPipe OnDipose(this IPipe pipe, Action<string> onDispose)
        {
            return new DisposeDelegatingPipe(pipe, Disposable.Create(() => onDispose(pipe.Name)));
        }      

        public static IPipe RefCount(this IPipe pipe)
        {
            return new RefCountedPipe(pipe);
        }

        public static string GetPipeKey(this string name, PipeType pipeType)
        {
            return $"{name}/{pipeType}";
        }

        public static string GetPipeName(this string key)
        {
            var index = key.IndexOf("/", StringComparison.Ordinal);
            return key.Substring(0, index + 1);
        }
    }
}