namespace BitTorrent
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;

    public class ValueList : BEncodeValue, IEnumerable, IEnumerator
    {
        private int Position = -1;
        private Collection<BEncodeValue> values = new Collection<BEncodeValue>();

        public void Add(BEncodeValue value)
        {
            this.values.Add(value);
        }

        public byte[] Encode()
        {
            Collection<byte> collection = new Collection<byte>();
            collection.Add(0x6c);
            foreach (BEncodeValue value2 in this.values)
            {
                foreach (byte num in value2.Encode())
                {
                    collection.Add(num);
                }
            }
            collection.Add(0x65);
            byte[] buffer = new byte[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                buffer[i] = collection[i];
            }
            return buffer;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (this.Position < (this.values.Count - 1))
            {
                this.Position++;
                return true;
            }
            return false;
        }

        public void Parse(Stream s)
        {
            for (byte i = (byte) s.ReadByte(); i != 0x65; i = (byte) s.ReadByte())
            {
                BEncodeValue item = BEncode.Parse(s, i);
                this.values.Add(item);
            }
        }

        public void Reset()
        {
            this.Position = -1;
        }

        public object Current
        {
            get
            {
                return this.values[this.Position];
            }
        }

        public BEncodeValue this[int index]
        {
            get
            {
                return this.values[index];
            }
            set
            {
                this.values[index] = value;
            }
        }

        public Collection<BEncodeValue> Values
        {
            get
            {
                return this.values;
            }
            set
            {
                this.values.Clear();
                foreach (BEncodeValue value2 in value)
                {
                    value.Add(value2);
                }
            }
        }
    }
}

