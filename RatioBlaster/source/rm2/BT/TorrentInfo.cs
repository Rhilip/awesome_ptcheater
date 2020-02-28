namespace rm2
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TorrentInfo
    {
        private string _tracker;
        private string _hash;
        private long _uploadRate;
        private long _downloadRate;
        private int _interval;
        private long _uploaded;
        private long _downloaded;
        private long _left;
        private long _totalsize;
        private string _filename;
        private string _key;
        private string _port;
        private Random random;
        private string _numberOfPeers;
        private string _peerID;
        private Uri _trackerUri;
        public TorrentInfo(long uploaded, long downloaded)
        {
            this._uploaded = uploaded;
            this._downloaded = downloaded;
            this._tracker = "";
            this._hash = "";
            this._left = 0x2710L;
            this._totalsize = 0x2710L;
            this._filename = "";
            this._uploadRate = 0xcc00L;
            this._downloadRate = 0x2800L;
            this._interval = 0x708;
            this.random = new Random();
            this._key = this.random.Next(0x3e8).ToString();
            this._port = this.random.Next(0x401, 0xffff).ToString();
            this._numberOfPeers = "100";
            this._peerID = "";
            this._trackerUri = null;
        }

        public string tracker
        {
            get
            {
                return this._tracker;
            }
            set
            {
                this._tracker = value;
            }
        }
        public string hash
        {
            get
            {
                return this._hash;
            }
            set
            {
                this._hash = value;
            }
        }
        public long uploadRate
        {
            get
            {
                return this._uploadRate;
            }
            set
            {
                this._uploadRate = value;
            }
        }
        public long downloadRate
        {
            get
            {
                return this._downloadRate;
            }
            set
            {
                this._downloadRate = value;
            }
        }
        public int interval
        {
            get
            {
                return this._interval;
            }
            set
            {
                this._interval = value;
            }
        }
        public long uploaded
        {
            get
            {
                return this._uploaded;
            }
            set
            {
                this._uploaded = value;
            }
        }
        public long downloaded
        {
            get
            {
                return this._downloaded;
            }
            set
            {
                this._downloaded = value;
            }
        }
        public long left
        {
            get
            {
                return this._left;
            }
            set
            {
                this._left = value;
            }
        }
        public long totalsize
        {
            get
            {
                return this._totalsize;
            }
            set
            {
                this._totalsize = value;
            }
        }
        public string filename
        {
            get
            {
                return this._filename;
            }
            set
            {
                this._filename = value;
            }
        }
        public string key
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
        public string port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
            }
        }
        public string numberOfPeers
        {
            get
            {
                return this._numberOfPeers;
            }
            set
            {
                this._numberOfPeers = value;
            }
        }
        public string peerID
        {
            get
            {
                return this._peerID;
            }
            set
            {
                this._peerID = value;
            }
        }
        public Uri trackerUri
        {
            get
            {
                return this._trackerUri;
            }
            set
            {
                this._trackerUri = value;
            }
        }
    }
}

