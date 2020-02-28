namespace BitTorrent
{
    using System;

    public class TorrentException : Exception
    {
        public TorrentException(string message) : base(message)
        {
        }
    }
}

