namespace ProxyNet
{
    using Events;
    using System.Net;

    public interface IProxy
    {
        string UserAgent { get; set; }

        bool EnableCompression { get; set; }

        WebHeaderCollection Headers { get; }

        ICredentials Credentials { get; set; }

        event ProxyResponseEventHandler ResponseEvent;

        event ProxyRequestEventHandler RequestEvent;
    }

    public delegate void ProxyResponseEventHandler(object sender, ProxyResponseEventArgs args);

    public delegate void ProxyRequestEventHandler(object sender, ProxyRequestEventArgs args);
}
