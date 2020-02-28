namespace BitTorrent
{
    using System;
    using System.IO;
    using System.Text;

    public class ValueString : BEncodeValue
    {
        private byte[] data;
        private string v;

        public ValueString()
        {
        }

        public ValueString(string StringValue)
        {
            this.String = StringValue;
        }

        public byte[] Encode()
        {
            string s = this.v.Length.ToString() + ":";
            byte[] bytes = Encoding.GetEncoding(0x6faf).GetBytes(s);
            byte[] buffer2 = new byte[s.Length + this.data.Length];
            for (int i = 0; i < s.Length; i++)
            {
                buffer2[i] = bytes[i];
            }
            for (int j = 0; j < this.data.Length; j++)
            {
                buffer2[j + s.Length] = this.data[j];
            }
            return buffer2;
        }

        public void Parse(Stream s)
        {
            throw new TorrentException("Parse method not supported, the first byte must be passed into the string parse routine.");
        }

        public void Parse(Stream s, byte firstByte)
        {
            string str = ((char) firstByte).ToString();
            if (char.IsNumber(str[0]))
            {
                for (char ch = (char) s.ReadByte(); ch != ':'; ch = (char) s.ReadByte())
                {
                    str = str + ch.ToString();
                }
                try
                {
                    int count = int.Parse(str);
                    this.data = new byte[count];
                    s.Read(this.data, 0, count);
                    this.v = Encoding.GetEncoding(0x6faf).GetString(this.data);
                }
                catch (Exception)
                {
                    this.v = null;
                }
            }
        }

        public byte[] Bytes
        {
            get
            {
                return this.data;
            }
        }

        public int Length
        {
            get
            {
                return this.v.Length;
            }
        }

        public string String
        {
            get
            {
                return this.v;
            }
            set
            {
                this.v = value;
                this.data = Encoding.GetEncoding(0x6faf).GetBytes(this.v);
            }
        }
    }
}

