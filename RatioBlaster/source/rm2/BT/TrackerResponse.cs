namespace rm2
{
    using BitTorrent;
    using System;
    using System.IO;
    using System.IO.Compression;

    internal class TrackerResponse
    {
        private string _body;
        private string _charset;
        private bool _chunkedEncoding;
        private string _contentEncoding;
        private ValueDictionary _dict;
        private string _headers;
        public bool doRedirect;
        public string RedirectionURL;
        public bool response_status_302;

        public TrackerResponse(MemoryStream responseStream)
        {
            string str2;
            this._headers = "";
            this._body = "";
            this._contentEncoding = "";
            this._charset = "";
            this.RedirectionURL = "";
            Stream stream = new MemoryStream();
            StreamReader streamReader = new StreamReader(responseStream);
            responseStream.Position = 0L;
            string str = this.getNewLineStr(streamReader);
            this._headers = "";
            do
            {
                str2 = streamReader.ReadLine();
                int index = str2.IndexOf("302 Found");
                if (index >= 0)
                {
                    this.response_status_302 = true;
                }
                else
                {
                    index = str2.IndexOf("Location: ");
                    if (index >= 0)
                    {
                        this.RedirectionURL = str2.Substring(index + 10);
                    }
                    else
                    {
                        index = str2.IndexOf("Content-Encoding: ");
                        if (index >= 0)
                        {
                            this._contentEncoding = str2.Substring(index + 0x12).ToLower();
                        }
                        else
                        {
                            index = str2.IndexOf("charset=");
                            if (index >= 0)
                            {
                                this._charset = str2.Substring(index + 8).ToLower();
                            }
                            else
                            {
                                index = str2.IndexOf("Transfer-Encoding: chunked");
                                if (index >= 0)
                                {
                                    this._chunkedEncoding = true;
                                }
                            }
                        }
                    }
                }
                this._headers = this._headers + str2 + str;
            }
            while (str2.Length != 0);
            responseStream.Position = this._headers.Length;
            if (this.response_status_302 && (this.RedirectionURL != ""))
            {
                this.doRedirect = true;
            }
            else
            {
                if (!this._chunkedEncoding)
                {
                    byte[] buffer2 = new byte[responseStream.Length - responseStream.Position];
                    responseStream.Read(buffer2, 0, buffer2.Length);
                    stream.Write(buffer2, 0, buffer2.Length);
                }
                else
                {
                    string str3 = "";
                    str3 = streamReader.ReadLine();
                    int count = Convert.ToInt32(str3.Split(new char[] { ' ' })[0], 0x10);
                    while (count > 0)
                    {
                        byte[] buffer = new byte[count];
                        responseStream.Position = (responseStream.Position + str3.Length) + str.Length;
                        responseStream.Read(buffer, 0, count);
                        stream.Write(buffer, 0, count);
                        streamReader.ReadLine();
                        str3 = streamReader.ReadLine();
                        try
                        {
                            count = Convert.ToInt32(str3.Split(new char[] { ' ' })[0], 0x10);
                            continue;
                        }
                        catch (Exception)
                        {
                            count = 0;
                            continue;
                        }
                    }
                }
                stream.Position = 0L;
                this._dict = this.parseBEncodeDict((MemoryStream) stream);
                stream.Position = 0L;
                StreamReader reader2 = new StreamReader(stream);
                this._body = reader2.ReadToEnd();
                stream.Dispose();
                reader2.Dispose();
                streamReader.Dispose();
            }
        }

        private string getNewLineStr(StreamReader streamReader)
        {
            char ch;
            long position = streamReader.BaseStream.Position;
            string str = "\r";
            do
            {
                ch = (char) streamReader.BaseStream.ReadByte();
            }
            while ((ch != '\r') && (ch != '\n'));
            if ((ch == '\r') && (((ushort) streamReader.BaseStream.ReadByte()) == 10))
            {
                str = "\r\n";
            }
            streamReader.BaseStream.Position = position;
            return str;
        }

        private ValueDictionary parseBEncodeDict(MemoryStream responseStream)
        {
            ValueDictionary dictionary = null;
            if ((this._contentEncoding == "gzip") || (this._contentEncoding == "x-gzip"))
            {
                GZipStream d = new GZipStream(responseStream, CompressionMode.Decompress);
                try
                {
                    dictionary = (ValueDictionary) BEncode.Parse(d);
                }
                catch (Exception)
                {
                }
                return dictionary;
            }
            try
            {
                dictionary = (ValueDictionary) BEncode.Parse(responseStream);
            }
            catch (Exception exception)
            {
                Console.Write(exception.StackTrace);
            }
            return dictionary;
        }

        private void saveArrayToFile(byte[] arr, string filename)
        {
            FileStream stream = File.OpenWrite(filename);
            stream.Write(arr, 0, arr.Length);
            stream.Close();
        }

        private void saveStreamToFile(MemoryStream ms, string filename)
        {
            FileStream stream = File.OpenWrite(filename);
            ms.WriteTo(stream);
            stream.Close();
        }

        public string Body
        {
            get
            {
                return this._body;
            }
        }

        public string Charset
        {
            get
            {
                return this._charset;
            }
        }

        public string ContentEncoding
        {
            get
            {
                return this._contentEncoding;
            }
        }

        public ValueDictionary Dict
        {
            get
            {
                return this._dict;
            }
        }

        public string Headers
        {
            get
            {
                return this._headers;
            }
        }
    }
}

