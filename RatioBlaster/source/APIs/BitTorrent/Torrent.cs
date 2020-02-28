namespace BitTorrent
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Security.Cryptography;

    public class Torrent
    {
        private ulong _totalLength;
        private ValueDictionary data;
        private byte[] infohash;
        private string localTorrentFile;
        private Piece[] pieceArray;
        private long pieceLength;
        private int pieces;
        private Collection<TorrentFile> torrentFiles;

        public Torrent()
        {
            this.data = new ValueDictionary();
            this.localTorrentFile = string.Empty;
            this.torrentFiles = new Collection<TorrentFile>();
        }

        public Torrent(string localFilename)
        {
            this.torrentFiles = new Collection<TorrentFile>();
            this.OpenTorrent(localFilename);
        }

        private void LoadTorrent()
        {
            if (!this.data.Contains("announce"))
            {
                throw new IncompleteTorrentData("No tracker URL");
            }
            if (!this.data.Contains("info"))
            {
                throw new IncompleteTorrentData("No internal torrent information");
            }
            ValueDictionary dictionary = (ValueDictionary) this.data["info"];
            this.pieceLength = ((ValueNumber) dictionary["piece length"]).Integer;
            if (!dictionary.Contains("pieces"))
            {
                throw new IncompleteTorrentData("No piece hash data");
            }
            ValueString str = (ValueString) dictionary["pieces"];
            if ((str.Length % 20) != 0)
            {
                throw new IncompleteTorrentData("Missing or damaged piece hash codes");
            }
            this.ParsePieceHashes(str.Bytes);
            if (this.SingleFile)
            {
                this.ParseSingleFile();
            }
            else
            {
                this.ParseMultipleFiles();
            }
            this.infohash = this.InfoHash;
        }

        public bool OpenTorrent()
        {
            return this.OpenTorrent(this.localTorrentFile);
        }

        public bool OpenTorrent(string localFilename)
        {
            this.data = null;
            bool flag = false;
            this.localTorrentFile = localFilename;
            this.data = new ValueDictionary();
            FileStream input = null;
            BinaryReader reader = null;
            try
            {
                input = File.OpenRead(localFilename);
                reader = new BinaryReader(input);
                this.data = (ValueDictionary) BEncode.Parse(reader.BaseStream);
                this.LoadTorrent();
                flag = true;
                reader.Close();
                input.Close();
            }
            catch (IOException)
            {
                flag = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (input != null)
                {
                    input.Close();
                }
            }
            return flag;
        }

        private void ParseMultipleFiles()
        {
            ValueDictionary dictionary = (ValueDictionary) this.data["info"];
            ValueList list = (ValueList) dictionary["files"];
            this.torrentFiles = null;
            this.torrentFiles = new Collection<TorrentFile>();
            foreach (ValueDictionary dictionary2 in list)
            {
                ValueList list2 = (ValueList) dictionary2["path"];
                bool flag = true;
                string apath = "";
                foreach (ValueString str2 in list2)
                {
                    if (!flag)
                    {
                        apath = apath + "/";
                    }
                    flag = false;
                    apath = apath + str2.String;
                }
                this._totalLength += (ulong) ((ValueNumber) dictionary2["length"]).Integer;
                TorrentFile item = new TorrentFile(((ValueNumber) dictionary2["length"]).Integer, apath);
                this.torrentFiles.Add(item);
            }
        }

        private void ParsePieceHashes(byte[] hashdata)
        {
            int num = hashdata.Length / 20;
            this.pieces = 0;
            this.pieceArray = null;
            this.pieceArray = new Piece[num];
            while (this.pieces < num)
            {
                this.pieceArray[this.pieces] = new Piece(this, this.pieces);
                this.pieces++;
            }
        }

        private void ParseSingleFile()
        {
            ValueDictionary dictionary = (ValueDictionary) this.data["info"];
            this._totalLength = (ulong) ((ValueNumber) dictionary["length"]).Integer;
            TorrentFile item = new TorrentFile(((ValueNumber) dictionary["length"]).Integer, ((ValueString) dictionary["name"]).String);
            this.torrentFiles.Add(item);
        }

        public string Announce
        {
            get
            {
                return BEncode.String(this.data["announce"]);
            }
            set
            {
                this.data.SetStringValue("announce", value);
            }
        }

        public string Comment
        {
            get
            {
                return BEncode.String(this.data["comment"]);
            }
            set
            {
                this.data.SetStringValue("comment", value);
            }
        }

        public string CreatedBy
        {
            get
            {
                return BEncode.String(this.data["created by"]);
            }
            set
            {
                this.data.SetStringValue("created by", value);
            }
        }

        public ValueDictionary Data
        {
            get
            {
                return this.data;
            }
        }

        public ValueDictionary Info
        {
            get
            {
                return (ValueDictionary) this.data["info"];
            }
        }

        public byte[] InfoHash
        {
            get
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                return sha.ComputeHash(this.data["info"].Encode());
            }
        }

        public string Name
        {
            get
            {
                return BEncode.String(((ValueDictionary) this.data["info"])["name"]);
            }
            set
            {
                if (!this.data.Contains("info"))
                {
                    this.data.Add("info", new ValueDictionary());
                }
                ((ValueDictionary) this.data["info"]).SetStringValue("name", value);
            }
        }

        public Collection<TorrentFile> PhysicalFiles
        {
            get
            {
                return this.torrentFiles;
            }
        }

        public int Pieces
        {
            get
            {
                return this.pieceArray.Length;
            }
        }

        public bool SingleFile
        {
            get
            {
                return ((ValueDictionary) this.data["info"]).Contains("length");
            }
        }

        public ulong totalLength
        {
            get
            {
                return this._totalLength;
            }
        }
    }
}

