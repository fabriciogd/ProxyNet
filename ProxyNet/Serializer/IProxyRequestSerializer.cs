namespace ProxyNet.Serializer
{
    using System.IO;

    public interface IProxyRequestSerializer
    {
        void Serialize(Stream stream, object[] parameters);
    }
}
