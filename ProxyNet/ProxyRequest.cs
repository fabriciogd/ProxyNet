namespace ProxyNet
{
    using System;
    using System.Reflection;

    public class ProxyRequest
    {
        public ProxyRequest(string methodName, object[] parameters, MethodInfo methodInfo)
        {
            Method = methodName;
            Args = parameters;
            Mi = methodInfo;

            if (Mi != null)
                ReturnType = Mi.ReturnType;
        }

        public String Method = null;
        public Object[] Args = null;
        public MethodInfo Mi = null;
        public Type ReturnType;
    }
}
