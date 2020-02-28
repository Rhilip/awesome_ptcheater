namespace BitTorrent
{
    using System;
    using System.IO;

    public class Piece
    {
        private byte[] hash = new byte[20];
        private int pieceNumber;
        private BitTorrent.Torrent torrent;

        public Piece(BitTorrent.Torrent parent, int pieceNumber)
        {
            this.pieceNumber = pieceNumber;
            this.torrent = parent;
            Buffer.BlockCopy(((ValueString) this.torrent.Info["pieces"]).Bytes, pieceNumber * 20, this.hash, 0, 20);
        }

        public byte[] Bytes
        {
            get
            {
                FileStream input = new FileStream(this.torrent.PhysicalFiles[0].Path, FileMode.Open);
                BinaryReader reader = new BinaryReader(input);
                byte[] buffer = reader.ReadBytes((int) input.Length);
                reader.Close();
                input.Close();
                return buffer;
            }
        }

        public byte[] Hash
        {
            get
            {
                return this.hash;
            }
        }

        public int PieceNumber
        {
            get
            {
                return this.pieceNumber;
            }
        }

        public BitTorrent.Torrent Torrent
        {
            get
            {
                return this.torrent;
            }
        }
    }
}

