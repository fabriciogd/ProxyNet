namespace ProxyNet
{
    using Enum;
    using Events;
    using Extensions;
    using Helpers;
    using Infraestructure;
    using Serializer;
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;

    public class ProxyClient : IProxy
    {

        WebSettings webSettings = new WebSettings();

        public bool EnableCompression
        {
            get { return webSettings.EnableCompression; }
            set { webSettings.EnableCompression = value; }
        }

        public string UserAgent
        {
            get { return webSettings.UserAgent; }
            set { webSettings.UserAgent = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return webSettings.Headers; }
        }
        public ICredentials Credentials
        {
            get { return webSettings.Credentials; }
            set { webSettings.Credentials = value; }
        }

        public event ProxyResponseEventHandler ResponseEvent;

        public event ProxyRequestEventHandler RequestEvent;

        public object Invoke(MethodBase mb, params object[] parameters)
        {
            return Invoke(this, mb as MethodInfo, parameters);
        }

        public object Invoke(ProxyClient client, MethodInfo mi, params object[] parameters)
        {
            object response = null;

            var url = this.GetEffectiveUrl(client, mi);
            var headers = this.GetHeaders(mi);

            this.MaybeFormatUrl(url, mi, parameters);

            WebRequest webRequest = this.GetWebRequest(url);

            this.SetRequestProperties(webRequest);
            this.SetRequestHeaders(webRequest, headers);
            this.MakeProxyRequest(webRequest, mi);

            if (mi.GetMethod() != Method.GET)
            {
                Stream requestStream = null;
                Stream serializedStream = null;

                if (!LogRequest)
                {
                    serializedStream = requestStream = webRequest.GetRequestStream();
                }
                else
                {
                    serializedStream = new MemoryStream(2000);
                }

                try
                {
                    var serializer = new ProxyRequestSerializer();
                    serializer.Serializer(serializedStream, parameters);

                    if (LogRequest)
                    {
                        requestStream = webRequest.GetRequestStream();
                        serializedStream.Position = 0;

                        Util.CopyStream(serializedStream, requestStream);

                        requestStream.Flush();
                        serializedStream.Position = 0;

                        OnRequest(new ProxyRequestEventArgs(serializedStream));
                    }

                }
                finally
                {
                    if (requestStream != null)
                        requestStream.Close();
                }
            }

            HttpWebResponse webResponse = this.GetWebResponse(webRequest) as HttpWebResponse;

            Stream responseStream = null;
            Stream deserializedStream = null;

            try
            {
                responseStream = webResponse.GetResponseStream();
                responseStream = this.MaybeDecompressStream((HttpWebResponse)webResponse, responseStream);

                if (!LogResponse)
                {
                    deserializedStream = responseStream;
                }
                else
                {
                    deserializedStream = new MemoryStream(2000);

                    Util.CopyStream(responseStream, deserializedStream);

                    deserializedStream.Flush();
                    deserializedStream.Position = 0;
                }

                if (LogResponse)
                {
                    OnResponse(new ProxyResponseEventArgs(deserializedStream));
                    deserializedStream.Position = 0;
                }

                response = this.ReadResponse(webResponse, deserializedStream, mi);
            }
            finally
            {
                if (webRequest != null)
                    webRequest = null;
            }

            return response;
        }

        private void MaybeFormatUrl(string url, MethodInfo mi, object[] parameters)
        {
            if (mi.GetMethod() == Method.GET && parameters.Length > 0)
                url = url.Format(mi, parameters);
        }

        public object ReadResponse(WebResponse webResponse, Stream responseStream, MethodInfo mi)
        {
            HttpWebResponse httpResp = (HttpWebResponse)webResponse;

            if (httpResp.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception(httpResp.StatusDescription);
            }

            var deserializer = new ProxyResponseDeserializer();
            return deserializer.Deserialize(responseStream, mi.ReturnType);
        }

        protected Stream MaybeDecompressStream(HttpWebResponse webResponse, Stream respStream)
        {
            Stream decodedStream;

            string contentEncoding = webResponse.ContentEncoding.ToLower();

            string coen = webResponse.Headers["Content-Encoding"];

            if (contentEncoding.Contains("gzip"))
            {
                decodedStream = new System.IO.Compression.GZipStream(respStream, System.IO.Compression.CompressionMode.Decompress);
            }
            else if (contentEncoding.Contains("deflate"))
            {
                decodedStream = new System.IO.Compression.DeflateStream(respStream, System.IO.Compression.CompressionMode.Decompress);
            }
            else
                decodedStream = respStream;

            return decodedStream;
        }

        private WebResponse GetWebResponse(WebRequest webRequest)
        {
            WebResponse webResponse = null;

            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw;

                webResponse = ex.Response;
            }

            return webResponse;
        }

        private string GetEffectiveUrl(ProxyClient client, MethodInfo mi)
        {
            var baseUrl = client.GetUrlAttribute();

            if (string.IsNullOrEmpty(baseUrl))
                throw new System.Exception("Proxy URL attribute not set.");

            var path = mi.GetUrlAttribute();

            return baseUrl + path;
        }

        private WebHeaderCollection GetHeaders(MethodInfo mi)
        {
            var headers = mi.GetHeaders();

            foreach (string key in this.Headers)
            {
                headers.Add(key, this.Headers[key]);
            }

            return headers;
        }

        private WebRequest GetWebRequest(string url)
        {
            Uri uri = new Uri(url);

            WebRequest req = WebRequest.Create(uri);

            return req;
        }

        private void SetRequestProperties(WebRequest webRequest)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)webRequest;

            httpWebRequest.UserAgent = this.UserAgent;

            if (this.EnableCompression)
                httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            httpWebRequest.Credentials = this.Credentials;
            httpWebRequest.UseDefaultCredentials = false;

        }

        private void MakeProxyRequest(WebRequest webRequest, MethodInfo mi)
        {
            webRequest.Method = mi.GetMethod().ToString();
            webRequest.ContentType = mi.GetContentType();
        }

        private void SetRequestHeaders(WebRequest webRequest, WebHeaderCollection headers)
        {
            foreach (string key in headers)
            {
                webRequest.Headers.Add(key, headers[key]);
            }
        }

        internal bool LogResponse
        {
            get { return ResponseEvent != null; }
        }

        internal bool LogRequest
        {
            get { return RequestEvent != null; }
        }

        protected virtual void OnResponse(ProxyResponseEventArgs e)
        {
            ResponseEvent?.Invoke(this, e);
        }

        protected virtual void OnRequest(ProxyRequestEventArgs e)
        {
            RequestEvent?.Invoke(this, e);
        }
    }
}
