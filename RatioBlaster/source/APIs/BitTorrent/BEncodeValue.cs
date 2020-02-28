namespace BitTorrent
{
    using System;
    using System.IO;

    public interface BEncodeValue
    {
        byte[] Encode();
        void Parse(Stream p);
    }
}

