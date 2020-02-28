namespace BitTorrent
{
    using System;
    using System.IO;

    public class TorrentFile
    {
        private FileInfo fileInfo;

        public TorrentFile(long len, string apath)
        {
            this.fileInfo = new FileInfo(apath);
        }

        public long Length
        {
            get
            {
                return this.fileInfo.Length;
            }
        }

        public string Name
        {
            get
            {
                return this.fileInfo.Name;
            }
        }

        public string Path
        {
            get
            {
                return this.fileInfo.FullName;
            }
        }
    }
}

