namespace ProxyNet.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public static class UrlExtensions
    {
        public static string Format(this string url, MethodInfo mi, object[] arguments, string separator = ",")
        {
            if (arguments == null)
                throw new ArgumentNullException("request");

            var parameters = mi.GetProxyParameters();

            if (parameters.Count == 0)
                return url + "/" + arguments[0];

            var properties = parameters
                   .Select((p, i) => new { Name = p, Value = arguments[i] })
                   .ToDictionary(x => x.Name, x => x.Value);

            var matches = Regex.Matches(url, @"\{(.+?)\}");

            List<string> words = (from Match matche in matches select matche.Groups[1].Value).ToList();

            url = url.Format(properties);

            properties = properties.Keys.Except(words).ToDictionary(a => a, a => properties[a]);

            var propertyNames = properties
                   .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                   .Select(x => x.Key)
                   .ToList();

            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            return url + "?" + string.Join("&", properties
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));
        }
    }
}
