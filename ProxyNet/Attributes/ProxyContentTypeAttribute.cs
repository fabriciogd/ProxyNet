using System;

namespace ProxyNet.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ProxyContentTypeAttribute : Attribute
    {
        public ProxyContentTypeAttribute(string contentType)
        {
            _contentType = ContentType;
        }

        public string ContentType { get { return _contentType; } }

        private readonly string _contentType;
    }
}
