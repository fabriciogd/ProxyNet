namespace ProxyNet.Events
{
    using System;
    using System.IO;

    public class ProxyRequestEventArgs : EventArgs
    {
        private Stream _requestStream;

        public ProxyRequestEventArgs(Stream requestStream)
        {
            _requestStream = requestStream;
        }

        public Stream RequestStream
        {
            get { return _requestStream; }
        }
    }
}