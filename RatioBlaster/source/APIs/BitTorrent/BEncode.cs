namespace BitTorrent
{
    using System;
    using System.IO;

    public class BEncode
    {
        private BEncode()
        {
        }

        public static BEncodeValue Parse(Stream d)
        {
            return Parse(d, (byte) d.ReadByte());
        }

        public static BEncodeValue Parse(Stream d, byte firstByte)
        {
            BEncodeValue value2;
            char ch = (char) firstByte;
            switch (ch)
            {
                case 'd':
                    value2 = new ValueDictionary();
                    break;

                case 'l':
                    value2 = new ValueList();
                    break;

                case 'i':
                    value2 = new ValueNumber();
                    break;

                default:
                    value2 = new ValueString();
                    break;
            }
            if (value2 is ValueString)
            {
                ((ValueString) value2).Parse(d, (byte) ch);
                return value2;
            }
            value2.Parse(d);
            return value2;
        }

        public static string String(BEncodeValue v)
        {
            if (v is ValueString)
            {
                return ((ValueString) v).String;
            }
            if (v is ValueNumber)
            {
                return ((ValueNumber) v).String;
            }
            return null;
        }
    }
}

