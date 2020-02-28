namespace BitTorrent
{
    using System;
    using System.IO;
    using System.Text;

    public class ValueNumber : BEncodeValue
    {
        private byte[] data;
        private string v;

        public ValueNumber()
        {
        }

        public ValueNumber(long number)
        {
            this.v = number.ToString();
            this.String = this.v;
        }

        public byte[] Encode()
        {
            byte[] buffer = new byte[this.data.Length + 2];
            buffer[0] = 0x69;
            for (int i = 0; i < this.data.Length; i++)
            {
                buffer[i + 1] = this.data[i];
            }
            buffer[this.data.Length + 1] = 0x65;
            return buffer;
        }

        public void Parse(Stream s)
        {
            string str = string.Empty;
            for (char ch = (char) s.ReadByte(); ch != 'e'; ch = (char) s.ReadByte())
            {
                str = str + ch.ToString();
            }
            try
            {
                this.String = long.Parse(str).ToString();
            }
            catch (Exception)
            {
                this.String = "";
            }
        }

        public long Integer
        {
            get
            {
                return long.Parse(this.v);
            }
            set
            {
                this.String = value.ToString();
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

