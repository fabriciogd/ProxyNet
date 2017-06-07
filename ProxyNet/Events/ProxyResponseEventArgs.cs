namespace ProxyNet.Events
{
    using System;
    using System.IO;

    public class ProxyResponseEventArgs : EventArgs
    {
        public ProxyResponseEventArgs(Guid guid, long requestNum, string url, Stream responseStream)
        {
            this._guid = guid;
            this._requestNum = requestNum;
            this._url = url;
            this._responseStream = responseStream;
        }

        public Guid ProxyID
        {
            get { return this._guid; }
        }

        private Guid _guid;

        public long RequestNum
        {
            get { return this._requestNum; }
        }

        private long _requestNum;

        public string Url
        {
            get { return this._url; }
        }

        private string _url;

        public Stream ResponseStream
        {
            get { return this._responseStream; }
        }

        private Stream _responseStream;
    }
}
