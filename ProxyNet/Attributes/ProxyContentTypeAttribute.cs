namespace ProxyNet.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class ProxyContentTypeAttribute : Attribute
    {
        public ProxyContentTypeAttribute(string contentType)
        {
            this._contentType = ContentType;
        }

        public string ContentType { get { return this._contentType; } }

        private readonly string _contentType;
    }
}
