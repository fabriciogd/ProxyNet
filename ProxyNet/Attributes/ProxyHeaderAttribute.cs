namespace ProxyNet.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ProxyHeaderAttribute : Attribute
    {
        public ProxyHeaderAttribute(string key, string value)
        {
            this._key = key;
            this._value = value;
        }

        public string Key { get { return this._key; } }

        private readonly string _key;

        public string Value { get { return this._value; } }

        private readonly string _value;
    }
}
