namespace BitTorrent
{
    using System;

    public class IncompleteTorrentData : TorrentException
    {
        public IncompleteTorrentData(string message) : base(message)
        {
        }
    }
}

