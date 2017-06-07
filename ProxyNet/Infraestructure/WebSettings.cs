namespace ProxyNet.Infraestructure
{
    using System.Net;

    public class WebSettings
    {
        public string UserAgent
        {
            get { return this._userAgent; }
            set { this._userAgent = value; }
        }

        private string _userAgent = "Proxy.NET";

        public bool EnableCompression
        {
            get { return this._enableCompression; }
            set { this._enableCompression = value; }
        }

        private bool _enableCompression = false;

        public WebHeaderCollection Headers
        {
            get { return this._headers; }

        }
        private WebHeaderCollection _headers = new WebHeaderCollection();

        public ICredentials Credentials
        {
            get { return this._credentials; }
            set { this._credentials = value; }
        }

        private ICredentials _credentials = null;
    }
}
