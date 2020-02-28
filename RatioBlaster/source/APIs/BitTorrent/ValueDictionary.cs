namespace BitTorrent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;

    public class ValueDictionary : BEncodeValue
    {
        private Dictionary<string, BEncodeValue> dict = new Dictionary<string, BEncodeValue>();

        public void Add(string key, BEncodeValue value)
        {
            this.dict.Add(key, value);
        }

        public bool Contains(string key)
        {
            return this.dict.ContainsKey(key);
        }

        public byte[] Encode()
        {
            Collection<byte> collection = new Collection<byte>();
            collection.Add(100);
            ArrayList list = new ArrayList();
            foreach (string str in this.dict.Keys)
            {
                list.Add(str);
            }
            foreach (string str2 in list)
            {
                ValueString str3 = new ValueString(str2);
                foreach (byte num in str3.Encode())
                {
                    collection.Add(num);
                }
                foreach (byte num2 in this.dict[str2].Encode())
                {
                    collection.Add(num2);
                }
            }
            collection.Add(0x65);
            byte[] array = new byte[collection.Count];
            collection.CopyTo(array, 0);
            return array;
        }

        public void Parse(Stream s)
        {
            for (byte i = (byte) s.ReadByte(); i != 0x65; i = (byte) s.ReadByte())
            {
                if (!char.IsNumber((char) i))
                {
                    return;
                }
                ValueString str = new ValueString();
                str.Parse(s, i);
                BEncodeValue value2 = BEncode.Parse(s);
                if (str.String != null)
                {
                    if (this.dict.ContainsKey(str.String))
                    {
                        this.dict[str.String] = value2;
                    }
                    else
                    {
                        this.dict.Add(str.String, value2);
                    }
                }
            }
        }

        public void Remove(string key)
        {
            this.dict.Remove(key);
        }

        public void SetStringValue(string key, string value)
        {
            if (this.Contains(value))
            {
                ((ValueString) this[key]).String = value;
            }
            else
            {
                this[key] = new ValueString(value);
            }
        }

        public BEncodeValue this[string key]
        {
            get
            {
                if (!this.dict.ContainsKey(key))
                {
                    this.dict.Add(key, new ValueString(""));
                }
                return this.dict[key];
            }
            set
            {
                if (this.dict.ContainsKey(key))
                {
                    this.dict.Remove(key);
                }
                this.dict.Add(key, value);
            }
        }

        public ICollection Keys
        {
            get
            {
                return this.dict.Keys;
            }
        }

        public ICollection Values
        {
            get
            {
                return this.dict.Values;
            }
        }
    }
}

