namespace ProxyNet
{
    using Runtime;
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public static class Proxy
    {
        static Dictionary<Type, Type> _types = new Dictionary<Type, Type>();

        public static T Create<T>() where T : IProxy
        {
            return (T)Create(typeof(T));
        }

        public static object Create(Type itf)
        {
            Type proxyType;

            lock (typeof(Proxy))
            {
                if (!_types.ContainsKey(itf))
                {
                    var guid = Guid.NewGuid();

                    var assemblyName = "Proxy" + guid;
                    var moduleName = string.Format("Proxy{0}.dll", guid);
                    var typeName = string.Format("Proxy{0}", guid);

                    var type = ProxyFactory.BuildType(itf, assemblyName, moduleName, typeName, AssemblyBuilderAccess.Run);

                    _types.Add(itf, type);
                }

                proxyType = _types[itf];
            }

            return Activator.CreateInstance(proxyType);
        }
    }
}
