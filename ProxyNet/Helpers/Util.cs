namespace ProxyNet.Helpers
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class Util
    {
        public static void CopyStream(Stream src, Stream dst)
        {
            byte[] buff = new byte[4096];

            while (true)
            {
                int read = src.Read(buff, 0, 4096);
                if (read == 0)
                    break;
                dst.Write(buff, 0, read);
            }
        }

        public static string Format(string url, MethodInfo mi, object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException("request");

            var parameters = mi.GetParameters().Select((a, i) => new { Name = a.Name, Value = arguments[i] });

            if (parameters.Count() == 0)
                return url;

            var dic = parameters.ToDictionary(a => a.Name, a => a.Value);

            url = url.Format(dic);

            return url;
        }

        public static object[] RemoveUnusedArguments(string url, MethodInfo mi, object[] arguments)
        {
            var matches = Regex.Matches(url, @"\{(.+?)\}");

            List<string> words = (from Match matche in matches select matche.Groups[1].Value).ToList();

            var parameters = mi.GetParameters().Select((a, i) => new { Name = a.Name, Index = i });

            var indexes = parameters.Where(a => words.Contains(a.Name)).Select(a => a.Index);

            return arguments.Where((a, i) => !indexes.Contains(i)).ToArray();
        }
    }
}
