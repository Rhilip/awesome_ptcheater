namespace rm2
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using rm2.UI;
    using RatioBlaster.rm2.Controllers;

    public class ApplicationSettings
    {
        //new stuff

        private bool _checkAutomaticMemReader;
        //-------

        private bool _checkLogEnabled;
        private bool _checkNewVersion;
        private bool _checkRandomDownload;
        private bool _checkRandomUpload;
        private bool _checkRequestScrap;
        private bool _checkShowTrayBaloon;
        private bool _checkTCPListen;
        private int _comboProxyTypeIndex;
        private string _configPath;
        private string _customKey;
        private string _customPeerID;
        private string _customPeersNum;
        private string _customPort;
        private string _downloadRate;
        private string _finishedPercent;
        private bool _ignoreFailureReason;
        private string _interval;
        private TorrerntSettingsUI _mainForm;
        private string _RandomDownloadFrom;
        private string _RandomDownloadTo;
        private string _RandomUploadFrom;
        private string _RandomUploadTo;
        private int _stopProcessActionBox;
        private int _stopProcessUnitsBox;
        private string _stopProcessValue;
        private string _textProxyHost;
        private string _textProxyPass;
        private string _textProxyPort;
        private string _textProxyUser;
        private decimal _textStopMinLeecher;
        private int _TorrentClientsIndex;
        private string _torrentFilePath;
        private string _torrentHash;
        private string _torrentsConfigDir;
        private bool _updateAnnounceParamsOnStart;
        private string _uploadRate;

        public ApplicationSettings()
        {
            this._configPath = Application.StartupPath + @"\rm2.config";
            this._torrentsConfigDir = Application.StartupPath + @"\Torrents Config\";
        }

        public ApplicationSettings(TorrerntSettingsUI MainForm)
        {
            this._configPath = Application.StartupPath + @"\rm2.config";
            this._torrentsConfigDir = Application.StartupPath + @"\Torrents Config\";
            this._mainForm = MainForm;
            if (!Directory.Exists(this._torrentsConfigDir))
            {
                Directory.CreateDirectory(this._torrentsConfigDir);
            }
        }

        private void applySettingsToForm(ApplicationSettings myAppSettings)
        {
            try
            {
                //new stuff
                this._mainForm.checkAutoMemReader.Checked = myAppSettings.checkAutoMemReader;
                //-----------

                this._mainForm.uploadRate.Text = myAppSettings.uploadRate;
                this._mainForm.downloadRate.Text = myAppSettings.downloadRate;
                this._mainForm.interval.Text = myAppSettings.interval;
                this._mainForm.TorrentClientsBox.SelectedIndex = myAppSettings.TorrentClientsIndex;
                
                this._mainForm.checkRequestScrap.Checked = myAppSettings.checkRequestScrap;
                //this._mainForm.checkShowTrayBaloon.Checked = myAppSettings.checkShowTrayBaloon;
                this._mainForm.checkTCPListen.Checked = myAppSettings.checkTCPListen;
                //this._mainForm.checkNewVersion.Checked = myAppSettings.checkNewVersion;
                this._mainForm.customPort.Text = myAppSettings.customPort;
                this._mainForm.customKey.Text = myAppSettings.customKey;
                this._mainForm.customPeersNum.Text = myAppSettings.customPeersNum;
                this._mainForm.customPeerID.Text = myAppSettings.customPeerID;
                this._mainForm.textProxyHost.Text = myAppSettings.textProxyHost;
                this._mainForm.textProxyPort.Text = myAppSettings.textProxyPort;
                this._mainForm.textProxyPass.Text = myAppSettings.textProxyPass;
                this._mainForm.textProxyUser.Text = myAppSettings.textProxyUser;
                this._mainForm.comboProxyType.SelectedIndex = myAppSettings.comboProxyTypeIndex;
                this._mainForm.checkRandomUpload.Checked = myAppSettings.checkRandomUpload;
                this._mainForm.checkRandomDownload.Checked = myAppSettings.checkRandomDownload;
                this._mainForm.RandomUploadFrom.Text = myAppSettings.RandomUploadFrom;
                this._mainForm.RandomUploadTo.Text = myAppSettings.RandomUploadTo;
                this._mainForm.RandomDownloadFrom.Text = myAppSettings.RandomDownloadFrom;
                this._mainForm.RandomDownloadTo.Text = myAppSettings.RandomDownloadTo;
                this._mainForm.fileSize.Text = this.GetValueDef(myAppSettings.finishedPercent, "100");
                this._mainForm.updateAnnounceParamsOnStart.Checked = myAppSettings.updateAnnounceParamsOnStart;
                this._mainForm.textStopMinLeecher.Value = myAppSettings.textStopMinLeecher;
                this._mainForm.checkIgnoreFailureReason.Checked = myAppSettings.ignoreFailureReason;
                
                //---------------------
                //these can yeild exceptions if no value was selected while it was saved last time
                if (myAppSettings.stopProcessActionBox > 0)
                {
                    this._mainForm.stopProcessActionBox.SelectedIndex = myAppSettings.stopProcessActionBox;

                    if (myAppSettings.stopProcessUnitsBox >= 0)
                        this._mainForm.stopProcessUnitsBox.SelectedIndex = myAppSettings.stopProcessUnitsBox;
                }
                //---------------------

                this._mainForm.stopProcessValue.Text = myAppSettings.stopProcessValue;
            }
            catch (Exception exception)
            {
                AddLogLine("Error loading config: " + exception.ToString());
                //MessageBox.Show("Error loading config: " + exception.ToString());
               
            }
        }

        public void AddLogLine(string logLine)
        {
            try
            {
                DateTime now = DateTime.Now;
                //this.strLogArray[intLogCount] = (("[" + string.Format("{0:hh:mm:ss}", now) + "]") + " " + logLine + "\r\n");
                //intLogCount++;
                //Logger.appErrLog = Logger.appErrLog + (("[" + string.Format("{0:hh:mm:ss}", now) + "]") + " " + logLine + "\r\n");
                Logger.Log(logLine, this);
            }
            catch (Exception)
            {
            }
        }

        public string getTorrentConfigPath(string torrentHash)
        {
            return (this._torrentsConfigDir + "torrent_" + torrentHash + ".config");
        }

        public ApplicationSettings getTorrentSettings(string torrentHash)
        {
            XmlSerializer serializer = null;
            FileStream stream = null;
            ApplicationSettings settings = null;
            serializer = new XmlSerializer(typeof(ApplicationSettings));
            string fileName = this.getTorrentConfigPath(torrentHash);
            FileInfo info = new FileInfo(fileName);
            try
            {
                if (info.Exists)
                {
                    stream = info.OpenRead();
                    settings = (ApplicationSettings) serializer.Deserialize(stream);
                    //this._mainForm.AddLogLine("Loaded settings from torrent config: " + fileName);
                }
            }
            catch (Exception exception)
            {
                //Logger.appErrLog = Logger.appErrLog + "/n" + exception.ToString();
                LogException(exception);
                //this._mainForm.AddLogLine("Error loading Torrent config: " + exception.ToString());
                settings = null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return settings;
        }

        private string GetValueDef(string val, string defVal)
        {
            if (val.Length > 0)
            {
                return val;
            }
            return defVal;
        }

        public bool LoadAppSettings()
        {
            XmlSerializer serializer = null;
            FileStream stream = null;
            bool flag = false;
            try
            {
                serializer = new XmlSerializer(typeof(ApplicationSettings));
                FileInfo info = new FileInfo(this._configPath);
                if (!info.Exists)
                {
                    return flag;
                }
                stream = info.OpenRead();
                ApplicationSettings settings = null;
                ApplicationSettings myAppSettings = (ApplicationSettings) serializer.Deserialize(stream);
                string torrentHash = myAppSettings.torrentHash;
                if ((torrentHash != null) && (torrentHash.Length > 0))
                {
                    settings = this.getTorrentSettings(torrentHash);
                    if (settings != null)
                    {
                        myAppSettings = settings;
                    }
                }
                this.applySettingsToForm(myAppSettings);
                this._mainForm.loadTorrentFileInfo(myAppSettings.torrentFilePath, false);
                flag = true;
            }
            catch (Exception exception)
            {
                //Logger.appErrLog = Logger.appErrLog + "/n" + exception.ToString();
                LogException(exception);
                //this._mainForm.AddLogLine("Error loading config: " + exception.ToString());
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return flag;
        }

        public void LoadTorrentSettings(string torrentHash)
        {
            ApplicationSettings myAppSettings = null;
            myAppSettings = this.getTorrentSettings(torrentHash);
            if (myAppSettings != null)
            {
                this.applySettingsToForm(myAppSettings);
            }
        }

        public void SaveAppSettings()
        {
            StreamWriter writer = null;
            XmlSerializer serializer = null;
            try
            {
                this.checkAutoMemReader = this._mainForm.checkAutoMemReader.Checked;
                this.uploadRate = this._mainForm.uploadRate.Text;
                this.downloadRate = this._mainForm.downloadRate.Text;
                this.interval = this._mainForm.interval.Text;
                this.TorrentClientsIndex = this._mainForm.TorrentClientsBox.SelectedIndex;
                
                this.checkRequestScrap = this._mainForm.checkRequestScrap.Checked;
                //this.checkShowTrayBaloon = this._mainForm.checkShowTrayBaloon.Checked;
                this.checkTCPListen = this._mainForm.checkTCPListen.Checked;
                //this.checkNewVersion = this._mainForm.checkNewVersion.Checked;
                this.customPort = this._mainForm.customPort.Text;
                this.customKey = this._mainForm.customKey.Text;
                this.customPeersNum = this._mainForm.customPeersNum.Text;
                this.customPeerID = this._mainForm.customPeerID.Text;
                this.textProxyHost = this._mainForm.textProxyHost.Text;
                this.textProxyPort = this._mainForm.textProxyPort.Text;
                this.textProxyPass = this._mainForm.textProxyPass.Text;
                this.textProxyUser = this._mainForm.textProxyUser.Text;
                this.comboProxyTypeIndex = this._mainForm.comboProxyType.SelectedIndex;
                this.checkRandomUpload = this._mainForm.checkRandomUpload.Checked;
                this.checkRandomDownload = this._mainForm.checkRandomDownload.Checked;
                this.RandomUploadFrom = this._mainForm.RandomUploadFrom.Text;
                this.RandomUploadTo = this._mainForm.RandomUploadTo.Text;
                this.RandomDownloadFrom = this._mainForm.RandomDownloadFrom.Text;
                this.RandomDownloadTo = this._mainForm.RandomDownloadTo.Text;
                this.finishedPercent = this._mainForm.fileSize.Text;
                this.updateAnnounceParamsOnStart = this._mainForm.updateAnnounceParamsOnStart.Checked;
                this.textStopMinLeecher = this._mainForm.textStopMinLeecher.Value;
                this.ignoreFailureReason = this._mainForm.checkIgnoreFailureReason.Checked;
                this.torrentFilePath = this._mainForm.torrentFile.Text;
                this.torrentHash = this._mainForm.shaHash.Text.Trim();
                this.stopProcessActionBox = this._mainForm.stopProcessActionBox.SelectedIndex;
                this.stopProcessUnitsBox = this._mainForm.stopProcessUnitsBox.SelectedIndex;
                this.stopProcessValue = this._mainForm.stopProcessValue.Text;
                serializer = new XmlSerializer(typeof(ApplicationSettings));
                writer = new StreamWriter(this._configPath, false);
                serializer.Serialize((TextWriter) writer, this);
                if (this.torrentHash.Length == 40)
                {
                    writer = new StreamWriter(this.getTorrentConfigPath(this.torrentHash), false);
                    serializer.Serialize((TextWriter) writer, this);
                }
            }
            catch (Exception exception)
            {
                //Logger.appErrLog = Logger.appErrLog + "/n" + exception.ToString();
                LogException(exception);
                //this._mainForm.AddLogLine("Error saving config: " + exception.ToString());
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void LogException(Exception exception)
        {
            Logger.Log("Exception in " + this.GetType().ToString() + "/n" + exception.ToString(), this);
        }

        public bool checkLogEnabled
        {
            get
            {
                return this._checkLogEnabled;
            }
            set
            {
                this._checkLogEnabled = value;
            }
        }

        public bool checkNewVersion
        {
            get
            {
                return this._checkNewVersion;
            }
            set
            {
                this._checkNewVersion = value;
            }
        }

        public bool checkRandomDownload
        {
            get
            {
                return this._checkRandomDownload;
            }
            set
            {
                this._checkRandomDownload = value;
            }
        }

        public bool checkRandomUpload
        {
            get
            {
                return this._checkRandomUpload;
            }
            set
            {
                this._checkRandomUpload = value;
            }
        }

        public bool checkRequestScrap
        {
            get
            {
                return this._checkRequestScrap;
            }
            set
            {
                this._checkRequestScrap = value;
            }
        }

        public bool checkShowTrayBaloon
        {
            get
            {
                return this._checkShowTrayBaloon;
            }
            set
            {
                this._checkShowTrayBaloon = value;
            }
        }

        public bool checkTCPListen
        {
            get
            {
                return this._checkTCPListen;
            }
            set
            {
                this._checkTCPListen = value;
            }
        }

        public int comboProxyTypeIndex
        {
            get
            {
                return this._comboProxyTypeIndex;
            }
            set
            {
                this._comboProxyTypeIndex = value;
            }
        }

        public string customKey
        {
            get
            {
                return this._customKey;
            }
            set
            {
                this._customKey = value;
            }
        }

        public string customPeerID
        {
            get
            {
                return this._customPeerID;
            }
            set
            {
                this._customPeerID = value;
            }
        }

        public string customPeersNum
        {
            get
            {
                return this._customPeersNum;
            }
            set
            {
                this._customPeersNum = value;
            }
        }

        public string customPort
        {
            get
            {
                return this._customPort;
            }
            set
            {
                this._customPort = value;
            }
        }

        public string downloadRate
        {
            get
            {
                return this._downloadRate;
            }
            set
            {
                this._downloadRate = value;
            }
        }

        public string finishedPercent
        {
            get
            {
                return this._finishedPercent;
            }
            set
            {
                this._finishedPercent = value;
            }
        }

        public bool ignoreFailureReason
        {
            get
            {
                return this._ignoreFailureReason;
            }
            set
            {
                this._ignoreFailureReason = value;
            }
        }

        public string interval
        {
            get
            {
                return this._interval;
            }
            set
            {
                this._interval = value;
            }
        }

        public string RandomDownloadFrom
        {
            get
            {
                return this._RandomDownloadFrom;
            }
            set
            {
                this._RandomDownloadFrom = value;
            }
        }

        public string RandomDownloadTo
        {
            get
            {
                return this._RandomDownloadTo;
            }
            set
            {
                this._RandomDownloadTo = value;
            }
        }

        public string RandomUploadFrom
        {
            get
            {
                return this._RandomUploadFrom;
            }
            set
            {
                this._RandomUploadFrom = value;
            }
        }

        public string RandomUploadTo
        {
            get
            {
                return this._RandomUploadTo;
            }
            set
            {
                this._RandomUploadTo = value;
            }
        }

        public int stopProcessActionBox
        {
            get
            {
                return this._stopProcessActionBox;
            }
            set
            {
                this._stopProcessActionBox = value;
            }
        }

        public int stopProcessUnitsBox
        {
            get
            {
                return this._stopProcessUnitsBox;
            }
            set
            {
                this._stopProcessUnitsBox = value;
            }
        }

        public string stopProcessValue
        {
            get
            {
                return this._stopProcessValue;
            }
            set
            {
                this._stopProcessValue = value;
            }
        }

        public string textProxyHost
        {
            get
            {
                return this._textProxyHost;
            }
            set
            {
                this._textProxyHost = value;
            }
        }

        public string textProxyPass
        {
            get
            {
                return this._textProxyPass;
            }
            set
            {
                this._textProxyPass = value;
            }
        }

        public string textProxyPort
        {
            get
            {
                return this._textProxyPort;
            }
            set
            {
                this._textProxyPort = value;
            }
        }

        public string textProxyUser
        {
            get
            {
                return this._textProxyUser;
            }
            set
            {
                this._textProxyUser = value;
            }
        }

        public decimal textStopMinLeecher
        {
            get
            {
                return this._textStopMinLeecher;
            }
            set
            {
                this._textStopMinLeecher = value;
            }
        }

        public int TorrentClientsIndex
        {
            get
            {
                return this._TorrentClientsIndex;
            }
            set
            {
                this._TorrentClientsIndex = value;
            }
        }

        public string torrentFilePath
        {
            get
            {
                return this._torrentFilePath;
            }
            set
            {
                this._torrentFilePath = value;
            }
        }

        public string torrentHash
        {
            get
            {
                return this._torrentHash;
            }
            set
            {
                this._torrentHash = value;
            }
        }

        public bool updateAnnounceParamsOnStart
        {
            get
            {
                return this._updateAnnounceParamsOnStart;
            }
            set
            {
                this._updateAnnounceParamsOnStart = value;
            }
        }

        public string uploadRate
        {
            get
            {
                return this._uploadRate;
            }
            set
            {
                this._uploadRate = value;
            }
        }

        public bool checkAutoMemReader
        {
            get
            {
                return this._checkAutomaticMemReader;
            }
            set 
            {
                this._checkAutomaticMemReader = value;
            }

        }


    }
}

