namespace ProxyNet.Extensions
{
    using Enum;
    using ProxyNet.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;

    public static class ProxyInfo
    {
        public static string GetUrlAttribute(this ProxyClient proxy)
        {
            var attr = Attribute.GetCustomAttribute(proxy.GetType(), typeof(ProxyUrlAttribute)) as ProxyUrlAttribute;
            var url = attr == null ? null : attr.Url;

            return url;
        }

        public static string GetUrlAttribute(this MethodInfo methodInfo)
        {
            var attr = Attribute.GetCustomAttribute(methodInfo, typeof(ProxyUrlAttribute)) as ProxyUrlAttribute;
            var url = attr == null ? null : attr.Url;

            return url;
        }

        public static WebHeaderCollection GetHeaders(this MethodInfo methodInfo)
        {
            WebHeaderCollection headers = new WebHeaderCollection();

            ProxyHeaderAttribute[] attrs = (ProxyHeaderAttribute[])Attribute.GetCustomAttributes(methodInfo, typeof(ProxyHeaderAttribute));

            foreach (var attr in attrs)
            {
                headers.Add(attr.Key, attr.Value);
            }

            return headers;
        }

        public static Method GetMethod(this MethodInfo methodInfo)
        {
            var attr = Attribute.GetCustomAttribute(methodInfo, typeof(ProxyMethodAttribute)) as ProxyMethodAttribute;
            var method = attr == null ? Method.GET : attr.Method;

            return method;
        }

        public static string GetContentType(this MethodInfo methodInfo)
        {
            var attr = Attribute.GetCustomAttribute(methodInfo, typeof(ProxyContentTypeAttribute)) as ProxyContentTypeAttribute;
            var contentType = attr == null ? null : attr.ContentType;

            return contentType;
        }

        public static List<string> GetProxyParameters(this MethodInfo methodInfo)
        {
            ProxyParameterInfoAttribute[] attrs = (ProxyParameterInfoAttribute[])Attribute.GetCustomAttributes(methodInfo, typeof(ProxyParameterInfoAttribute));

            return attrs.Select(a => a.Name).ToList();
        }
    }
}
