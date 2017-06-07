namespace ProxyNet
{
    using System;
    using System.Reflection;

    public class ProxyRequest
    {
        public ProxyRequest(string method, MethodInfo methodInfo, object[] parameters, string url, Guid id)
        {
            this._method = method;
            this._methodInfo = methodInfo;
            this._parameters = parameters;
            this._url = url;
            this._id = id;
        }

        public string Method
        {
            get { return this._method; }
        }

        private string _method;

        public MethodInfo MethodInfo
        {
            get { return this._methodInfo; }
        }

        private MethodInfo _methodInfo;

        public object[] Parameters
        {
            get { return this._parameters; }
        }

        private object[] _parameters;

        public string Url
        {
            get { return this._url; }
        }

        private string _url;

        public Guid Id
        {
            get { return this._id; }
        }

        private Guid _id;

        static int _created;

        public int Number = System.Threading.Interlocked.Increment(ref _created);

    }
}
