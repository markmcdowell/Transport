using Newtonsoft.Json;
using Transport.Interfaces;

namespace Transport.Core.Adapters
{
    public sealed class ObjectToJsonAdapter<T> : IAdapter<T, string>
    {
        public string Adapt(T from)
        {
            return JsonConvert.SerializeObject(from);
        }

        public T Adapt(string to)
        {
            return JsonConvert.DeserializeObject<T>(to);
        }
    }
}