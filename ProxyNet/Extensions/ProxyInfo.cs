namespace ProxyNet.Extensions
{
    using Enum;
    using ProxyNet.Attributes;
    using Serializer;
    using System;
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

        public static IProxyRequestSerializer GetSerializer(this ProxyClient proxy)
        {
            var attr = Attribute.GetCustomAttribute(proxy.GetType(), typeof(ProxySerializerAttribute)) as ProxySerializerAttribute;
            var serializer = attr == null ? null : attr.Serializer;

            return serializer;
        }

        public static IProxyRequestSerializer GetSerializer(this MethodInfo methodInfo)
        {
            var attr = Attribute.GetCustomAttribute(methodInfo, typeof(ProxySerializerAttribute)) as ProxySerializerAttribute;
            var serializer = attr == null ? null : attr.Serializer;

            return serializer;
        }

        public static IProxyResponseDeserializer GetDeserializer(this ProxyClient proxy)
        {
            var attr = Attribute.GetCustomAttribute(proxy.GetType(), typeof(ProxyDeserializerAttribute)) as ProxyDeserializerAttribute;
            var deserializer = attr == null ? null : attr.Deserializer;

            return deserializer;
        }

        public static IProxyResponseDeserializer GetDeserializer(this MethodInfo methodInfo)
        {
            var attr = Attribute.GetCustomAttribute(methodInfo, typeof(ProxyDeserializerAttribute)) as ProxyDeserializerAttribute;
            var deserializer = attr == null ? null : attr.Deserializer;

            return deserializer;
        }

        public static bool IsSuccessStatusCode(this HttpWebResponse httpWebResponse)
        {
            return ((int)httpWebResponse.StatusCode >= 200) && ((int)httpWebResponse.StatusCode <= 299);
        }
    }
}
