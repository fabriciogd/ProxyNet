namespace ProxyNet.Attributes
{
    using Serializer;
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ProxySerializerAttribute : Attribute
    {
        public ProxySerializerAttribute(IProxyRequestSerializer serializer)
        {
            this._serializer = serializer;
        }

        public IProxyRequestSerializer Serializer { get { return this._serializer; } }

        private readonly IProxyRequestSerializer _serializer;
    }
}
