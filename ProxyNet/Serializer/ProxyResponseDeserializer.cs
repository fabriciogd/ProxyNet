namespace ProxyNet.Serializer
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public class ProxyResponseDeserializer : IProxyResponseDeserializer
    {
        public object Deserialize(Stream responseStream, Type returnType)
        {
            using (var reader = new StreamReader(responseStream))
            {
                using (var jr = new JsonTextReader(reader))
                {
                    dynamic d = new JsonSerializer().Deserialize(jr, returnType);
                    return d;
                }
            }
        }
    }
}
