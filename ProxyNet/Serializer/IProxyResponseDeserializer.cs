namespace ProxyNet.Serializer
{
    using System;
    using System.IO;

    public interface IProxyResponseDeserializer
    {
        object Deserialize(Stream responseStream, Type returnType);
    }
}
