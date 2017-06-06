namespace ProxyNet.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ProxyHeaderAttribute : Attribute
    {
        public ProxyHeaderAttribute(string key, string value)
        {
            _key = key;
            _value = value;
        }

        public string Key { get { return _key; } }

        private readonly string _key;

        public string Value { get { return _value; } }

        private readonly string _value;
    }
}
