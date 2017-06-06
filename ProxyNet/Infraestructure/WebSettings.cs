using System.Net;

namespace ProxyNet.Infraestructure
{
    public class WebSettings
    {
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        private string _userAgent = "Proxy.NET";


        public bool EnableCompression
        {
            get { return _enableCompression; }
            set { _enableCompression = value; }
        }

        private bool _enableCompression = false;

        public WebHeaderCollection Headers
        {
            get { return _headers; }

        }
        private WebHeaderCollection _headers = new WebHeaderCollection();

        public ICredentials Credentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        private ICredentials _credentials = null;
    }
}
