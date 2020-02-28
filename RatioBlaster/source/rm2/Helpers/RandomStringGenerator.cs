namespace rm2
{
    using System;
    using System.Text;

    public class RandomStringGenerator
    {
        private char[] characterArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private Random randNum = new Random();

        public string Generate(int stringLength)
        {
            return this.Generate(stringLength, false);
        }

        public string Generate(int stringLength, char[] characterArray)
        {
            StringBuilder builder = new StringBuilder();
            builder.Capacity = stringLength;
            for (int i = 0; i <= (stringLength - 1); i++)
            {
                builder.Append(characterArray[(int) ((characterArray.GetUpperBound(0) + 1) * this.randNum.NextDouble())]);
            }
            if (builder != null)
            {
                return builder.ToString();
            }
            return string.Empty;
        }

        public string Generate(int stringLength, bool randomness)
        {
            StringBuilder builder = new StringBuilder();
            builder.Capacity = stringLength;
            for (int i = 0; i <= (stringLength - 1); i++)
            {
                if (randomness)
                {
                    builder.Append((char) this.randNum.Next(0xff));
                }
                else
                {
                    builder.Append(this.GetRandomCharacter());
                }
            }
            if (builder != null)
            {
                return builder.ToString();
            }
            return string.Empty;
        }

        private char GetRandomCharacter()
        {
            return this.characterArray[(int) ((this.characterArray.GetUpperBound(0) + 1) * this.randNum.NextDouble())];
        }

        public string urlEncode(string Str, bool upperCase)
        {
            string str = "";
            string str2 = "";
            for (int i = 0; i < Str.Length; i++)
            {
                if (char.IsLetterOrDigit(Str[i]) && (Str[i] <= '\x007f'))
                {
                    str = str + Str[i];
                }
                else
                {
                    str = str + "%";
                    str2 = Convert.ToString((int) Str[i], 0x10);
                    if (upperCase)
                    {
                        str2 = str2.ToUpper();
                    }
                    if (str2.Length == 1)
                    {
                        str = str + "0" + str2;
                    }
                    else
                    {
                        str = str + str2;
                    }
                }
            }
            return str;
        }
    }
}

