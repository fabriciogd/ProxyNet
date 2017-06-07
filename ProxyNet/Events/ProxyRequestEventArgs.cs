namespace ProxyNet.Events
{
    using System;
    using System.IO;

    public class ProxyRequestEventArgs : EventArgs
    {
        public ProxyRequestEventArgs(Guid guid, long requestNum, string url, Stream requestStream)
        {
            this._guid = guid;
            this._requestNum = requestNum;
            this._url = url;
            this._requestStream = requestStream;
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

        public Stream RequestStream
        {
            get { return this._requestStream; }
        }

        private Stream _requestStream;
    }
}