using System;
using System.Reactive.Disposables;

namespace Transport.Pipes
{
    internal static class PipeExtensions
    {
        public static IPipe OnDipose(this IPipe pipe, Action<string> onDispose)
        {
            if (pipe == null)
                throw new ArgumentNullException(nameof(pipe));
            if (onDispose == null)
                throw new ArgumentNullException(nameof(onDispose));

            var key = GetPipeKey(pipe.Name, pipe.PipeType);

            var disposable = Disposable.Create(() => onDispose(key));

            return new DisposeDelegatingPipe(pipe, disposable);
        }      

        public static IPipe RefCount(this IPipe pipe)
        {
            if (pipe == null)
                throw new ArgumentNullException(nameof(pipe));

            return new RefCountedPipe(pipe);
        }

        public static string GetPipeKey(this string name, PipeType pipeType)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return $"{name}/{pipeType}";
        }

        public static string GetPipeName(this string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var index = key.IndexOf("/", StringComparison.Ordinal);
            return key.Substring(0, index + 1);
        }
    }
}