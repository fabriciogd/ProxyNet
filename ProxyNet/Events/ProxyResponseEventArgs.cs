namespace ProxyNet.Events
{
    using System;
    using System.IO;

    public class ProxyResponseEventArgs : EventArgs
    {
        private Stream _responseStream;

        public ProxyResponseEventArgs(Stream responseStream)
        {
            _responseStream = responseStream;
        }

        public Stream ResponseStream
        {
            get { return _responseStream; }
        }
    }
}
