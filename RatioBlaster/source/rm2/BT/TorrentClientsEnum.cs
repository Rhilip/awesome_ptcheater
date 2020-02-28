namespace rm2
{
    using ProcessMemoryReaderLib;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using rm2.UI;

    internal class TorrentClientsEnum
    {
        private int _clientsNum;
        private TorrerntSettingsUI _mainForm;
        private TorrentClient[] _torrentClients;
        private TorrentClient[] externalClients;
        private TorrentClient[] internalClients;
        private Random random;
        private RandomStringGenerator stringGenerator;
        private rm2.TorrentSession currentRMIns;
        
        public TorrentClientsEnum(TorrerntSettingsUI MainForm)
        {
            rm2.Program.currentRM = currentRMIns;

            int num;
            this._clientsNum = 5;
            this.random = new Random();
            this.stringGenerator = new RandomStringGenerator();
            this._mainForm = MainForm;
            this.internalClients = this.EnumerateInternalClients();
            this.externalClients = this.EnumerateExternalClients();
            this._torrentClients = new TorrentClient[this.externalClients.Length + this.internalClients.Length];
            int num2 = 0;
            for (num = 0; num < this.externalClients.Length; num++)
            {
                this._torrentClients[num] = this.externalClients[num];
                num2++;
            }
            for (num = 0; num < this.internalClients.Length; num++)
            {
                this._torrentClients[num2 + num] = this.internalClients[num];
            }
        }

        private TorrentClient[] EnumerateExternalClients()
        {
            //first of all let's see whether the directory exits or not :P
            
            //path
            string clientFilesPath;
            
            // Specify the directories you want to manipulate.
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\Clients");

            if (di.Exists == true)//ok the dir is there so it...
            {
                clientFilesPath = Application.StartupPath + "\\Clients";
            }
            else//its not there so set the path to this... because we can't just pass a non exisiting path... but a exsisting path with no files should be ok
            {
                clientFilesPath = Application.StartupPath;
            }

                //changing the path where client files should put in... . defult is \\Clients
                string[] files = Directory.GetFiles(clientFilesPath, "*.client");
                this._clientsNum = files.GetUpperBound(0);
                this.externalClients = new TorrentClient[this._clientsNum + 1];
                int index = 0;
                foreach (string str2 in files)
                {
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        document.Load(str2);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Corrupted client file [" + str2 + "], please remove or fix it", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        try
                        {
                            Process.GetCurrentProcess().Kill();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    XmlNode xmlNode = document.SelectSingleNode("//client");
                    this.externalClients[index] = new TorrentClient(this.getAttribute(xmlNode, "name") + " (author " + this.getAttribute(xmlNode, "author") + ", ver. " + this.getAttribute(xmlNode, "version") + ")");
                    this.externalClients[index].ProcessName = this.getAttribute(xmlNode, "processname");
                    XmlNode node2 = xmlNode.SelectSingleNode("query");
                    this.externalClients[index].Query = node2.InnerText;
                    XmlNode node3 = xmlNode.SelectSingleNode("headers");
                    this.externalClients[index].Headers = node3.InnerText.Replace("_nl_", "\r\n");
                    XmlNode node4 = xmlNode.SelectSingleNode("peer_id");
                    this.externalClients[index].PeerIDPrefix = this.getAttribute(node4, "prefix");
                    this.externalClients[index].PeerID = this.parseValueFromNode(node4);
                    XmlNode node5 = xmlNode.SelectSingleNode("key");
                    this.externalClients[index].Key = this.parseValueFromNode(node5);
                    XmlNode node6 = xmlNode.SelectSingleNode("protocol");
                    this.externalClients[index].HttpProtocol = this.getAttribute(node6, "value", "HTTP/1.1");
                    XmlNode node7 = xmlNode.SelectSingleNode("hash");
                    string str3 = this.getAttribute(node7, "upperCase", "false");
                    this.externalClients[index].HashUpperCase = str3 == "true";
                    index++;
                }
                return this.externalClients;

        }

        private TorrentClient[] EnumerateInternalClients()
        {
            this.internalClients = new TorrentClient[3];
            //this.internalClients[0] = new TorrentClient("Azureus 2.4.0.2");
            //this.internalClients[0].Query = "info_hash={infohash}&peer_id={peerid}&port={port}&uploaded={uploaded}&downloaded={downloaded}&left={left}{event}&numwant={numwant}&no_peer_id=1&compact=1&key={key}";
            //this.internalClients[0].Headers = "User-Agent: Azureus 2.4.0.2;Windows XP;Java 1.5.0_04\r\nConnection: close\r\nAccept-Encoding: gzip\r\nHost: {host}\r\nAccept: text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2\r\nContent-type: application/x-www-form-urlencoded\r\n";
            //this.internalClients[0].PeerIDPrefix = "-AZ2402-";
            //this.internalClients[0].PeerID = this.internalClients[0].PeerIDPrefix + this.GenerateIdString("alphanumeric", 12, false);
            //this.internalClients[0].Key = this.GenerateIdString("alphanumeric", 8, false);
            //this.internalClients[0].HttpProtocol = "HTTP/1.1";
            //this.internalClients[0].HashUpperCase = true;
            //this.internalClients[0].ProcessName = "azureus";

            this.internalClients[0] = new TorrentClient("Azureus 2.5.0.2 by yeehaa ver 1.0");
            this.internalClients[0].Query = "info_hash={infohash}&amp;peer_id={peerid}&amp;requirecrypto=1&amp;port={port}&amp;azudp={port}&amp;uploaded={uploaded}&amp;downloaded={downloaded}&amp;left={left}{event}&amp;numwant={numwant}&amp;no_peer_id=1&amp;compact=1&amp;key={key}&amp;azver=3";
            this.internalClients[0].Headers = "User-Agent: Azureus 2.5.0.2_nl_Connection: close_nl_Accept-Encoding: gzip_nl_Host: {host}_nl_Accept: text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2_nl_";
            this.internalClients[0].PeerIDPrefix = "-AZ2502-";
            this.internalClients[0].PeerID = this.internalClients[0].PeerIDPrefix + this.GenerateIdString("alphanumeric", 12, false);
            this.internalClients[0].Key = this.GenerateIdString("alphanumeric", 8, false);
            this.internalClients[0].HttpProtocol = "HTTP/1.1";
            this.internalClients[0].HashUpperCase = true;
            this.internalClients[0].ProcessName = "azureus";

            //<client name="Azureus 2.5.0.2" author="yeehaa" version="1.0" processname="azureus">
            //  <query>info_hash={infohash}&amp;peer_id={peerid}&amp;requirecrypto=1&amp;port={port}&amp;azudp={port}&amp;uploaded={uploaded}&amp;downloaded={downloaded}&amp;left={left}{event}&amp;numwant={numwant}&amp;no_peer_id=1&amp;compact=1&amp;key={key}&amp;azver=3</query> 
            //  <headers>User-Agent: Azureus 2.5.0.2_nl_Connection: close_nl_Accept-Encoding: gzip_nl_Host: {host}_nl_Accept: text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2_nl_</headers>
            //  <peer_id prefix="-AZ2502-" type="alphanumeric" length="12" urlencoding="false" upperCase="false" value=""/>
            //  <key type="alphanumeric" length="8" urlencoding="false" upperCase="false" value=""/>
            //  <protocol value="HTTP/1.1"/>
            //  <hash upperCase="true"/>
            //</client>

            this.internalClients[1] = new TorrentClient("uTorrent 1.6.1  (build 490)");
            this.internalClients[1].Query = "info_hash={infohash}&peer_id={peerid}&port={port}&uploaded={uploaded}&downloaded={downloaded}&left={left}&key={key}{event}&numwant={numwant}&compact=1&no_peer_id=1";
            this.internalClients[1].Headers = "Host: {host}\r\nUser-Agent: uTorrent/1610\r\nAccept-Encoding: gzip\r\n";
            this.internalClients[1].PeerIDPrefix = "-UT1610-%ea%81";
            this.internalClients[1].PeerID = this.internalClients[1].PeerIDPrefix + this.GenerateIdString("random", 10, true);
            this.internalClients[1].Key = this.GenerateIdString("hex", 8, false).ToUpper();
            this.internalClients[1].HttpProtocol = "HTTP/1.1";
            this.internalClients[1].HashUpperCase = false;
            this.internalClients[1].ProcessName = "utorrent";

            //<client name="uTorrent 1.7.5 build (4602)" author="uk10" version="1.0" processname="utorrent">
            //  <query>info_hash={infohash}&amp;peer_id={peerid}&amp;port={port}&amp;uploaded={uploaded}&amp;downloaded={downloaded}&amp;left={left}&amp;key={key}{event}&amp;numwant={numwant}&amp;compact=1&amp;no_peer_id=1</query> 
            //  <headers>Host: {host}_nl_User-Agent: uTorrent/1750_nl_Accept-Encoding: gzip_nl_</headers>
            //  <peer_id prefix="-UT1750-%fa%91" type="random" length="10" urlencoding="true" upperCase="false" value=""/>
            //  <key type="hex" length="8" urlencoding="false" upperCase="true" value=""/>
            //  <protocol value="HTTP/1.1"/>
            //  <hash upperCase="false"/>
            //</client>

            this.internalClients[2] = new TorrentClient("uTorrent 1.7.5 build (4602) by uk10 ver 1.0");
            this.internalClients[2].Query = "info_hash={infohash}&amp;peer_id={peerid}&amp;port={port}&amp;uploaded={uploaded}&amp;downloaded={downloaded}&amp;left={left}&amp;key={key}{event}&amp;numwant={numwant}&amp;compact=1&amp;no_peer_id=1";
            this.internalClients[2].Headers = "Host: {host}_nl_User-Agent: uTorrent/1750_nl_Accept-Encoding: gzip_nl_";
            this.internalClients[2].PeerIDPrefix = "-UT1750-%fa%91";
            this.internalClients[2].PeerID = this.internalClients[1].PeerIDPrefix + this.GenerateIdString("random", 10, true);
            this.internalClients[2].Key = this.GenerateIdString("hex", 8, false).ToUpper();
            this.internalClients[2].HttpProtocol = "HTTP/1.1";
            this.internalClients[2].HashUpperCase = false;
            this.internalClients[2].ProcessName = "utorrent";

            //this.internalClients[2] = new TorrentClient("BitComet 0.70");
            //this.internalClients[2].Query = "info_hash={infohash}&peer_id={peerid}&port={port}&uploaded={uploaded}&downloaded={downloaded}&left={left}&numwant=200&compact=1&no_peer_id=1&key={key}{event}";
            //this.internalClients[2].Headers = "Connection: close\r\nHost: {host}\r\nUser-Agent: BitTorrent/3.4.2\r\nAccept-Encoding: gzip, deflate\r\nCache-Control: no-cache\r\n";
            //this.internalClients[2].PeerIDPrefix = "%2DBC0070%2D";
            //this.internalClients[2].PeerID = this.internalClients[2].PeerIDPrefix + this.GenerateIdString("random", 12, true, true);
            //this.internalClients[2].Key = this.GenerateIdString("numeric", 5, false);
            //this.internalClients[2].HttpProtocol = "HTTP/1.0";
            //this.internalClients[2].HashUpperCase = true;
            //this.internalClients[2].ProcessName = "bitcomet";
            return this.internalClients;
        }

        private string GenerateIdString(string keyType, int keyLength, bool urlencoding)
        {
            return this.GenerateIdString(keyType, keyLength, urlencoding, false);
        }

        private string GenerateIdString(string keyType, int keyLength, bool urlencoding, bool upperCase)
        {
            string str = "";
            switch (keyType)
            {
                case "alphanumeric":
                    str = this.stringGenerator.Generate(keyLength);
                    break;

                case "numeric":
                    str = this.stringGenerator.Generate(keyLength, "0123456789".ToCharArray());
                    break;

                case "random":
                    str = this.stringGenerator.Generate(keyLength, true);
                    break;

                case "hex":
                    str = this.stringGenerator.Generate(keyLength, "0123456789abcdef".ToCharArray());
                    break;

                default:
                    str = this.stringGenerator.Generate(keyLength);
                    break;
            }
            if (urlencoding)
            {
                return this.stringGenerator.urlEncode(str, upperCase);
            }
            if (upperCase)
            {
                str = str.ToUpper();
            }
            return str;
        }

        private string getAttribute(XmlNode xmlNode, string attribName)
        {
            return this.getAttribute(xmlNode, attribName, "");
        }

        private string getAttribute(XmlNode xmlNode, string attribName, string defValue)
        {
            if (xmlNode == null)
            {
                return defValue;
            }
            XmlNode namedItem = xmlNode.Attributes.GetNamedItem(attribName);
            if (namedItem == null)
            {
                return defValue;
            }
            return namedItem.Value.ToString();
        }

        private string parseValueFromNode(XmlNode xmlNode)
        {
            string str = this.getAttribute(xmlNode, "value");
            if (str == "")
            {
                string str2 = this.getAttribute(xmlNode, "prefix");
                string str3 = this.getAttribute(xmlNode, "suffix");
                string keyType = this.getAttribute(xmlNode, "type", "alphanumeric");
                int keyLength = int.Parse(this.getAttribute(xmlNode, "length", "12"));
                bool urlencoding = this.getAttribute(xmlNode, "urlencoding", "true") == "true";
                bool upperCase = this.getAttribute(xmlNode, "upperCase", "false") == "true";
                str = str2 + this.GenerateIdString(keyType, keyLength, urlencoding, upperCase) + str3;
            }
            return str;
        }

        private void ReadIDsFromMemory(TorrentClient[] internalClients)
        {
            currentRMIns.AddLogLine("Looking for uTorrent 1.6  (build 474) process...");
            Process[] processesByName = Process.GetProcessesByName("utorrent");
            if (processesByName.Length == 0)
            {
                currentRMIns.AddLogLine("No uTorrent process found :(");
            }
            else
            {
                ProcessMemoryReader pReader = new ProcessMemoryReader();
                pReader.ReadProcess = processesByName[0];
                currentRMIns.AddLogLine("uTorrent process found! Checking version and data offsets :)");
                pReader.OpenProcess();
                string str = "";
                string prefix = "-UT1600-";
                str = this.ReadMemoryIdString(pReader, "utorrent_peerid", prefix, 0x448a38);
                if (str == "")
                {
                    currentRMIns.AddLogLine("Incompatible uTorrent version or wrong data offsets :(");
                }
                else
                {
                    internalClients[1].PeerID = "-UT1600-" + this.stringGenerator.urlEncode(str, false);
                    this.ReadMemoryIdString(pReader, "utorrent_key", "", 0x448a34);
                    internalClients[1].Key = this.ReadMemoryIdString(pReader, "utorrent_key", "", 0x448a34);
                    currentRMIns.AddLogLine("Success!!!");
                    currentRMIns.AddLogLine("Updated " + internalClients[1].Name);
                    currentRMIns.AddLogLine("---> peerID=" + internalClients[1].PeerID);
                    currentRMIns.AddLogLine("---> key=" + internalClients[1].Key);
                    currentRMIns.AddLogLine("");
                    pReader.CloseHandle();
                }
            }
        }

        private string ReadMemoryIdString(ProcessMemoryReader pReader, string stringType, string prefix, int memOffset)
        {
            string str = "";
            string str2 = "";
            try
            {
                byte[] buffer;
                int num;
                switch (stringType.ToLower())
                {
                    case "utorrent_key":
                        buffer = pReader.ReadProcessMemory((IntPtr) memOffset, 4, out num);
                        return string.Format("{0:X}{1:X}{2:X}{3:X}", new object[] { buffer[3], buffer[2], buffer[1], buffer[0] });

                    case "utorrent_peerid":
                        buffer = pReader.ReadProcessMemory((IntPtr) memOffset, 20, out num);
                        str = Encoding.GetEncoding(0x6faf).GetString(buffer);
                        str2 = str.Replace(prefix, "");
                        if (str2.Length < str.Length)
                        {
                            return str2;
                        }
                        return "";
                }
                str = "";
            }
            catch (Exception exception)
            {
                currentRMIns.AddLogLine("ClientsEnum : " + exception.ToString());
            }
            return str;
        }

        public TorrentClient[] TorrentClients
        {
            get
            {
                return this._torrentClients;
            }
        }
    }
}

