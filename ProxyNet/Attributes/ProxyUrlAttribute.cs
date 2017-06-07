namespace ProxyNet.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class ProxyUrlAttribute : Attribute
    {
        public ProxyUrlAttribute(string url)
        {
            this._url = url;
        }

        public string Url { get { return this._url; } }

        private readonly string _url;
    }
}
