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
    }
}