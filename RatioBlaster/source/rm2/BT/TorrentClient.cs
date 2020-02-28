namespace rm2
{
    using System;

    public class TorrentClient
    {
        private bool _HashUpperCase;
        private string _Headers;
        private string _HttpProtocol;
        private string _key;
        private string _Name;
        private string _PeerID;
        private string _PeerIDPrefix;
        private string _ProcessName = "";
        private string _Query;

        public TorrentClient(string Name)
        {
            this._Name = Name;
        }

        public bool HashUpperCase
        {
            get
            {
                return this._HashUpperCase;
            }
            set
            {
                this._HashUpperCase = value;
            }
        }

        public string Headers
        {
            get
            {
                return this._Headers;
            }
            set
            {
                this._Headers = value;
            }
        }

        public string HttpProtocol
        {
            get
            {
                return this._HttpProtocol;
            }
            set
            {
                this._HttpProtocol = value;
            }
        }

        public string Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        public string PeerID
        {
            get
            {
                return this._PeerID;
            }
            set
            {
                this._PeerID = value;
            }
        }

        public string PeerIDPrefix
        {
            get
            {
                return this._PeerIDPrefix;
            }
            set
            {
                this._PeerIDPrefix = value;
            }
        }

        public string ProcessName
        {
            get
            {
                return this._ProcessName;
            }
            set
            {
                this._ProcessName = value;
            }
        }

        public string Query
        {
            get
            {
                return this._Query;
            }
            set
            {
                this._Query = value;
            }
        }
    }
}

