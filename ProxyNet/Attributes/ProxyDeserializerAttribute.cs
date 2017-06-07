namespace ProxyNet.Attributes
{
    using Serializer;
    using System;


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ProxyDeserializerAttribute : Attribute
    {
        public ProxyDeserializerAttribute(IProxyResponseDeserializer deserializer)
        {
            this._deserializer = deserializer;
        }

        public IProxyResponseDeserializer Deserializer { get { return this._deserializer; } }

        private readonly IProxyResponseDeserializer _deserializer;
    }
}