namespace ProxyNet.Attributes
{
    using Enum;
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class ProxyMethodAttribute : Attribute
    {
        public ProxyMethodAttribute(Method method)
        {
            _method = method;
        }

        public Method Method { get { return _method; } }

        private readonly Method _method;
    }
}
