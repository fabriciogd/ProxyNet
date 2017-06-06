namespace ProxyNet.Attributes
{
    using System;


    [AttributeUsage(AttributeTargets.Method)]
    public class ProxyParameterInfoAttribute : Attribute
    {
        public ProxyParameterInfoAttribute(string name)
        {
            _name = name;
        }

        public string Name { get { return _name; } }

        private readonly string _name;
    }
}
