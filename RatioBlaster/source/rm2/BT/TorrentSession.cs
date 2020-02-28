//using System;
//using System.Text;

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//stuff that RM form uses
using BitTorrent;
using BytesRoad.Net.Sockets;

namespace rm2
{
    [Serializable]
    public class TorrentSession
    {
        #region Enums
        public enum SizeUnits
        {
            KB,
            MB,
            GB
        }

        public enum TimeUnits
        {
            Seconds,
            Minutes,
            Hours
        }
        #endregion

        #region MemberVariables
        //-------------------
        //stuff declared by sp, for this RM class...
        public string strBigLog;

        //have the RM object been properly initillized? values given...?
        public bool isInitillized = false;

        //this is to keep the rm collection's index number for this RM instence...
        public int rmIndex;

        //should we overide the tracker's requested interval and chooes to keep our own?
        public bool boolIntervalOveride = false;

        //flags on Start Stop...
        public bool Started;
        //Boolean Stopped;

        //updateScrapStats()
        public String strScrapStatSeeders;
        public String strScrapStatLeechs;

        //public void updateCounters(TorrentInfo torrentInfo)
        public String strUploadCount;
        public String strDownloadCount;
        public String strFileSizeText;

        //timer value
        public String strTimerValue;

        public String[] strLogArray = new String[] { "something" };
        int intLogCount = 0;

        //String strSelTorClnt;



        //randomiseSpeeds()
        public bool boolRandomDownload = true;
        public bool boolRandomUpload = true;
        public String strRandomUploadFrom;
        public String strRandomUploadTo;
        public String strRandomDownloadFrom;
        public String strRandomDownloadTo;

        //isStopProcessCondition()
        public String strStopProcessValue = "1000";


        //-----------------------
        public bool boolIgnoreFailureReason = false;

        public bool updateProcessStarted;

        public bool boolRequestScrap;
        public bool boolStopMinLeecher;
        public String strStopMinLeecherVal;
        //--------------------

        //-------------------

        //getStopProcessUnitsMultiplyer()
        public int intStopProcessActionBoxSelectedIndex;
        public int intStopProcessUnitsBoxSelectedIndex;
        //------------------------

        //OpenTcpListener()
        public bool boolTCPListen = true;
        //
        //declaring needed variables/classes here
        //private IContainer components;
        public rm2.TorrentClient currentClient;
        public rm2.ProxyInfo currentProxy;
        public rm2.TorrentInfo currentTorrent;
        public Torrent currentTorrentFile;

        private bool haveInitialPeers;

        public TcpListener localListen;

        private Random random = new Random((int)DateTime.Now.Ticks);//check this out... might be a security risk!

        public bool scrapStatsUpdated;
        public bool seedMode;
        public System.Windows.Forms.Timer serverUpdateTimer;

        //changed this to private because the UI needs to change this sometimes....
        public bool stopParamsUpdateInProgress;

        public int temporaryIntervalCounter;

        //private TorrentClientsEnum TorrentClientsObj;

        public int totalRunningTimeCounter;

        //private VersionChecker versionChecker;

        public string strTotalRunningTime; 
        #endregion

        #region Constructors
        public TorrentSession(int parRmIndex)
        {
            try
            {
                //set the rm's index number in the rm collection
                rmIndex = parRmIndex;

                this.InitializeComponent(); // this will initialize the timer..
            }
            catch (Exception e)
            {
                manageError(e);
            }

        }

        private void InitializeComponent()
        {
            this.serverUpdateTimer = new System.Windows.Forms.Timer();

            // 
            // serverUpdateTimer
            // 
            this.serverUpdateTimer.Interval = 1000;
            this.serverUpdateTimer.Tick += new System.EventHandler(this.serverUpdateTimer_Tick);
            System.Windows.Forms.Timer g = new System.Windows.Forms.Timer();
            //

        }
        #endregion

        #region Util Functions

        private long RoundByDenominator(long value, long denominator)
        {
            return (denominator * (value / denominator));
        }

        public string ConvertToTime(int seconds)
        {
            if (seconds < 0xe10)
            {
                int num = seconds / 60;
                int num2 = seconds % 60;
                return (num.ToString("00") + ":" + num2.ToString("00"));
            }
            string[] strArray = new string[] { (seconds / 0xe10).ToString("00"), ":", ((seconds % 0xe10) / 60).ToString("00"), ":", (seconds % 60).ToString("00") };
            return string.Concat(strArray);
        }

        public string FormatFileSize(ulong fileSize)
        {
            if (fileSize < 0L)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }
            if (fileSize >= 0x40000000L)
            {
                return string.Format("{0:########0.00} GB", ((double)fileSize) / 1073741824);
            }
            if (fileSize >= 0x100000L)
            {
                return string.Format("{0:####0.00} MB", ((double)fileSize) / 1048576);
            }
            if (fileSize >= 0x400L)
            {
                return string.Format("{0:####0.00} KB", ((double)fileSize) / 1024);
            }
            return string.Format("{0} bytes", fileSize);
        }

        public string HashUrlEncode(string decoded, bool upperCase)
        {
            StringBuilder builder = new StringBuilder();
            RandomStringGenerator generator = new RandomStringGenerator();
            try
            {
                for (int i = 0; i < decoded.Length; i += 2)
                {
                    char ch = (char)Convert.ToUInt16(decoded.Substring(i, 2), 0x10);
                    builder.Append(ch);
                }
            }
            catch (Exception exception)
            {
                this.AddLogLine(exception.ToString());
            }
            return generator.urlEncode(builder.ToString(), upperCase);
        }

        private float parseValidFloat(string str, float defVal)
        {
            try
            {
                return float.Parse(str);
            }
            catch (Exception)
            {
                return defVal;
            }
        }

        private int ParseValidInt(string str, int defVal)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {
                return defVal;
            }
        }

        private long parseValidInt64(string str, long defVal)
        {
            try
            {
                return long.Parse(str);
            }
            catch (Exception)
            {
                return defVal;
            }
        }

        #endregion

        #region Public Methods
        public void AddLogLine(string logLine)
        {
            /*
            if (this.logWindow.InvokeRequired)
            {
                SetTextCallback method = new SetTextCallback(this.AddLogLine);
                base.Invoke(method, new object[] { logLine });
            }
            else if (this.checkLogEnabled.Checked)
            {
                try
                {
                    DateTime now = DateTime.Now;
                    this.logWindow.AppendText(("[" + string.Format("{0:hh:mm:ss}", now) + "]") + " " + logLine + "\r\n");
                    this.logWindow.ScrollToCaret();
                }
                catch (Exception)
                {
                }
            }
             */

            /* SP edit: i am gonna use a string to store this log. so it will be in a local variable. and i will
             * write a method so that someone outside can call it and get the log...
             */

            try
            {
                DateTime now = DateTime.Now;
                //this.strLogArray[intLogCount] = (("[" + string.Format("{0:hh:mm:ss}", now) + "]") + " " + logLine + "\r\n");
                //intLogCount++;
                this.strBigLog = strBigLog + (("[" + string.Format("{0:hh:mm:ss}", now) + "]") + " " + logLine + "\r\n");
            }
            catch (Exception)
            {
            }
        }

        public void ClearLog()
        {
            try{
            
                this.strLogArray = new String[]{"something"}; //this should clear the log
               
            }
            catch(Exception e)
            {
                manageError(e);
            }
        }
        
        public void completedProcess()
        {
            try{
                
                if (this.sendEventToTracker(this.currentTorrent, "&event=completed"))
                {
                    this.requestScrapeFromTracker(this.currentTorrent);
                }
                
            }
            catch(Exception e)
            {
                manageError(e);
            }

        }

        public void continueProcess()
        {

            if (this.sendEventToTracker(this.currentTorrent, ""))
            {
                this.requestScrapeFromTracker(this.currentTorrent);
            }
        }

        public string getUrlString(TorrentInfo torrentInfo, string eventType)
        {
            string newValue = "0";
            if (torrentInfo.uploaded > 0L)
            {
                torrentInfo.uploaded = this.RoundByDenominator(torrentInfo.uploaded, 0x4000L);
                newValue = Convert.ToString(torrentInfo.uploaded);
            }
            string str2 = "0";
            if (torrentInfo.downloaded > 0L)
            {
                torrentInfo.downloaded = this.RoundByDenominator(torrentInfo.downloaded, 0x10L);
                str2 = Convert.ToString(torrentInfo.downloaded);
            }
            if (torrentInfo.left > 0L)
            {
                torrentInfo.left = torrentInfo.totalsize - torrentInfo.downloaded;
            }
            string str3 = torrentInfo.left.ToString();
            string str4 = torrentInfo.key.ToString();
            string str5 = torrentInfo.port.ToString();
            string peerID = torrentInfo.peerID;
            string tracker = "";
            tracker = torrentInfo.tracker;
            if (tracker.Contains("?"))
            {
                tracker = tracker + "&";
            }
            else
            {
                tracker = tracker + "?";
            }
            tracker = (tracker + this.currentClient.Query).Replace("{infohash}", this.HashUrlEncode(torrentInfo.hash, this.currentClient.HashUpperCase)).Replace("{peerid}", peerID).Replace("{port}", str5).Replace("{uploaded}", newValue).Replace("{downloaded}", str2).Replace("{left}", str3).Replace("{event}", eventType);
            if ((torrentInfo.numberOfPeers == "0") && !eventType.ToLower().Contains("stopped"))
            {
                torrentInfo.numberOfPeers = "100";
            }
            return tracker.Replace("{numwant}", torrentInfo.numberOfPeers).Replace("{key}", str4).Replace("{localip}", Program.localIP);
        }

        public string getScrapeUrlString(TorrentInfo torrentInfo)
        {
            string tracker = "";
            tracker = torrentInfo.tracker;
            int num = tracker.LastIndexOf("/");
            if (tracker.Substring(num + 1, 8).ToLower() != "announce")
            {
                return "";
            }
            tracker = tracker.Substring(0, num + 1) + "scrape" + tracker.Substring(num + 9);
            string str2 = this.HashUrlEncode(torrentInfo.hash, this.currentClient.HashUpperCase);
            if (tracker.Contains("?"))
            {
                tracker = tracker + "&";
            }
            else
            {
                tracker = tracker + "?";
            }
            return (tracker + "info_hash=" + str2);
        }

        public Boolean manualUpdate()
        {
            try
            {
                if (this.updateProcessStarted)
                {
                    this.OpenTcpListener();
                    this.temporaryIntervalCounter = this.currentTorrent.interval;

                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public void setDownSpeed(string downSpeed)
        {
            //before setting the new down rate let's update it on the Main UI...
            Program.mainUI.updateStats(rmIndex, 6, downSpeed + " KB/s");

            this.currentTorrent.downloadRate = (long)(this.parseValidFloat(downSpeed, 10f) * 1024f);
        }

        public void randomiseSpeeds()
        {
            string strUploadRate;
            string strDownloadRate;

            try
            {
                //this here so that inputting random to from values as double values won't mess things up :P
                /////////////

                //double numbers don' go with int so get it to double and convert in to int as rounded numbers
                double temp;
                double.TryParse(strRandomUploadFrom, out temp);//got it as a double value

                int upFrom, upTo, downFrom, downTo;//declare the variables

                upFrom = Convert.ToInt32(temp);

                double.TryParse(strRandomUploadTo, out temp);//got it as a double value
                upTo = Convert.ToInt32(temp);

                double.TryParse(strRandomDownloadFrom, out temp);//got it as a double value
                downFrom = Convert.ToInt32(temp);

                double.TryParse(strRandomDownloadTo, out temp);//got it as a double value
                downTo = Convert.ToInt32(temp);

                /////////////

                float num = ((float)this.random.Next(100)) / 100f;
                float num2 = ((float)this.random.Next(100)) / 100f;
                if (boolRandomUpload == true)
                {
                    strUploadRate = (this.random.Next(upFrom, upTo) + num).ToString();
                    setUploadRate(strUploadRate);

                }
                if (boolRandomDownload == true)
                {
                    strDownloadRate = (this.random.Next(downFrom, downTo) + num2).ToString();
                    setDownSpeed(strDownloadRate);
                }
            }
            catch (Exception exception)
            {
                this.AddLogLine("Failed to randomise upload/download speeds: " + exception.ToString());
            }
        }

        public void requestScrapeFromTracker(TorrentInfo torrentInfo)
        {
            if (boolRequestScrap && !this.scrapStatsUpdated)
            {
                try
                {
                    string uriString = this.getScrapeUrlString(torrentInfo);
                    if (uriString == "")
                    {
                        this.AddLogLine("This tracker doesnt seem to support scrape");
                    }
                    else
                    {
                        Uri reqUri = new Uri(uriString);
                        TrackerResponse response = this.MakeWebRequestEx(reqUri);
                        if ((response != null) && (response.Dict != null))
                        {
                            string str2 = BEncode.String(response.Dict["failure reason"]);
                            if (str2.Length > 0)
                            {
                                this.AddLogLine("Tracker Error: " + str2);
                            }
                            else
                            {
                                this.AddLogLine("---------- Scrape Info -----------");
                                ValueDictionary dictionary = (ValueDictionary)response.Dict["files"];
                                string str3 = Encoding.GetEncoding(0x6faf).GetString(this.currentTorrentFile.InfoHash);
                                if (dictionary[str3].GetType() == typeof(ValueDictionary))
                                {
                                    ValueDictionary dictionary2 = (ValueDictionary)dictionary[str3];
                                    this.AddLogLine("complete: " + BEncode.String(dictionary2["complete"]));
                                    this.AddLogLine("downloaded: " + BEncode.String(dictionary2["downloaded"]));
                                    this.AddLogLine("incomplete: " + BEncode.String(dictionary2["incomplete"]));
                                    this.updateScrapStats(BEncode.String(dictionary2["complete"]), BEncode.String(dictionary2["incomplete"]), BEncode.String(dictionary2["downloaded"]));
                                    decimal num = this.ParseValidInt(BEncode.String(dictionary2["incomplete"]), 0);
                                    if (boolStopMinLeecher && (num < Int32.Parse(strStopMinLeecherVal)))
                                    {
                                        this.AddLogLine("Min number of leechers reached...setting Upload speed to 0");
                                        //this.updateTextBox(this.uploadRate, "0");
                                        setUploadRate("0"); // this is to set up rate to 0...
                                    }
                                }
                                else
                                {
                                    this.AddLogLine("Scrape returned : '" + ((ValueString)dictionary[str3]).String + "'");
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.AddLogLine("Error: " + exception.ToString());
                }
            }


        }

        public void setUploadRate(String newUpRate)
        {
            //before setting the new up rate let's update it on the Main UI...
            Program.mainUI.updateStats(rmIndex, 7, newUpRate + " KB/s");
            this.currentTorrent.uploadRate = (long)(this.parseValidFloat(newUpRate, 50f) * 1024f);
        }



        public String[] GetLogForSaving()
        {
            return strLogArray;
        }

        public bool sendEventToTracker(TorrentInfo torrentInfo, string eventType)
        {
            this.scrapStatsUpdated = false;
            this.currentTorrent = torrentInfo;
            string uriString = this.getUrlString(torrentInfo, eventType);
            ValueDictionary dict = null;
            try
            {
                Uri reqUri = new Uri(uriString);
                TrackerResponse response = this.MakeWebRequestEx(reqUri);
                if ((response != null) && (response.Dict != null))
                {
                    dict = response.Dict;
                    string text = BEncode.String(dict["failure reason"]);
                    if (text.Length > 0)
                    {
                        this.AddLogLine("Tracker Error: " + text);
                        if (!boolIgnoreFailureReason)
                        {
                            this.stopTimerAndCounters();
                            MessageBox.Show(text, "Tracker Response", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return false;
                        }
                        if (dict.Contains("interval"))
                        {
                            this.updateInterval(BEncode.String(dict["interval"]));
                        }
                        else
                        {
                            this.updateInterval("1200");
                        }
                    }
                    else
                    {
                        foreach (string str3 in dict.Keys)
                        {
                            if ((str3 != "failure reason") && (str3 != "peers"))
                            {
                                this.AddLogLine(str3 + ": " + BEncode.String(dict[str3]));
                            }
                        }
                        if (dict.Contains("peers"))
                        {
                            this.haveInitialPeers = true;
                            string s = "";
                            if (dict["peers"] is ValueString)
                            {
                                s = BEncode.String(dict["peers"]);
                                Encoding encoding = Encoding.GetEncoding(0x6faf);
                                byte[] bytes = encoding.GetBytes(s);
                                BinaryReader reader = new BinaryReader(new MemoryStream(encoding.GetBytes(s)));
                                PeerList list = new PeerList();
                                for (int i = 0; i < bytes.Length; i += 6)
                                {
                                    list.Add(new Peer(reader.ReadBytes(4), reader.ReadInt16()));
                                }
                                reader.Close();
                                //silentp33r edit:
                                //i had to comment out the overridden ToString method of the list class... which was
                                //done in PeerList.cs, this is the only place where that method is used so i guess it won't
                                //be a problem as this is about logging... :)

                                //Update: that ToString method was fixed... now this should work...

                                this.AddLogLine("peers: " + list.ToString());
                            }
                            else if (dict["peers"] is ValueList)
                            {
                                s = "";
                                ValueList list2 = (ValueList)dict["peers"];
                                PeerList list3 = new PeerList();
                                foreach (object obj2 in list2)
                                {
                                    if (obj2 is ValueDictionary)
                                    {
                                        ValueDictionary dictionary2 = (ValueDictionary)obj2;
                                        list3.Add(new Peer(BEncode.String(dictionary2["ip"]), BEncode.String(dictionary2["port"]), BEncode.String(dictionary2["peer id"])));
                                    }
                                }
                                this.AddLogLine("peers: " + list3.ToString());
                            }
                            else
                            {
                                this.AddLogLine("peers(x): " + BEncode.String(dict["peers"]));
                            }
                        }
                        if (dict.Contains("interval"))
                        {
                            this.updateInterval(BEncode.String(dict["interval"]));
                        }
                        if (dict.Contains("complete") && dict.Contains("incomplete"))
                        {
                            this.updateScrapStats(BEncode.String(dict["complete"]), BEncode.String(dict["incomplete"]), "");
                            decimal num2 = this.ParseValidInt(BEncode.String(dict["incomplete"]), 0);
                            //if (this.textStopMinLeecher.Enabled && (num2 < this.textStopMinLeecher.Value))
                            //this was replaced so that it would use the supplied pars rather than text box values
                            if (this.boolStopMinLeecher && (num2 < Int32.Parse(strStopMinLeecherVal)))
                            {
                                this.AddLogLine("Min number of leechers reached...setting Upload speed to 0");
                                //i am commenting this off. RM class will not update the Form1/UI...
                                //insted the UI will have to "poll" RM and read it's values to get the stats

                                //this.updateTextBox(this.uploadRate, "0");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.AddLogLine("SendEventToTracker Error: " + exception.ToString());
                return false;
            }
            return true;
        }

        public bool Start(
            TorrentClient parSelTorClnt,
            ProxyInfo parProxyInfo,
            TorrentInfo parTorrentInfo,
            Torrent parCurrentTorrentFile
            )
        {

            //update the state of this RM object in the main UI 3 - Status
            Program.mainUI.updateStats(rmIndex, 3, "Started");

            //try
            //{
            currentClient = parSelTorClnt;
            currentProxy = parProxyInfo;
            currentTorrent = parTorrentInfo;
            currentTorrentFile = parCurrentTorrentFile;

            //this.currentClient = this.getCurrentClient();
            //this.currentProxy = this.getCurrentProxy();
            //this.currentTorrent = this.getCurrentTorrent();
            if (this.currentTorrent.trackerUri == null)
            {
                MessageBox.Show("To start , please select a valid torrent file by clicking on 'Browse...' button or Drag & Drop it into rm2", "Start", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.updateScrapStats("", "", "");
                this.totalRunningTimeCounter = 0;
                this.strTimerValue = "updating...";

                //update the mainUI
                Program.mainUI.updateStats(rmIndex, 13, strTimerValue);

                //these were used to be buttons but now they have been replaced by variables
                this.Started = false;
                //this.Stopped = true;

                //i felt that this is not nesccery as started and stop flags are there
                //this.manualUpdate = true;

                this.OpenTcpListener();
                Thread thread = new Thread(new ThreadStart(this.startProcess));
                thread.Name = "startProcess() Thread";
                thread.Start();
                this.serverUpdateTimer.Start();

                //-------------------------
                //sp edit:
                //enable ClientCrash button...

                //i felt that this is not nesccery as started and stop flags are there
                //this.emuCrash = true;

                //------------------------------
            }

            //make sure this variable is set so we can use it to track this RM object...
            Started = true;
            return true;
            //}
            //catch(Exception e)
            //{
            //    
            //   MessageBox.Show(e.Message,"RM Start()");
            //   return false;
            //}
        }

        //this is a extra method :) kinda overloaded Start ;)
        public bool Start()
        {

            //update the state of this RM object in the main UI 3 - Status
            Program.mainUI.updateStats(rmIndex, 3, "Started");

            //we don't need these as we assume that these are already set

            //currentClient = parSelTorClnt;
            //currentProxy = parProxyInfo;
            //currentTorrent = parTorrentInfo;
            //currentTorrentFile = parCurrentTorrentFile;

            this.seedMode = false;
            //this.currentClient = this.getCurrentClient();
            //this.currentProxy = this.getCurrentProxy();
            //this.currentTorrent = this.getCurrentTorrent();
            if (this.currentTorrent.trackerUri == null)
            {
                MessageBox.Show("To start , please select a valid torrent file by clicking on 'Browse...' button or Drag & Drop it into rm2", "Start", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.updateScrapStats("", "", "");
                this.totalRunningTimeCounter = 0;
                this.strTimerValue = "updating...";

                //update the mainUI
                Program.mainUI.updateStats(rmIndex, 13, strTimerValue);

                //these were used to be buttons but now they have been replaced by variables
                this.Started = false;
                //this.Stopped = true;

                //i felt that this is not nesccery as started and stop flags are there
                //this.manualUpdate = true;

                this.OpenTcpListener();
                Thread thread = new Thread(new ThreadStart(this.startProcess));
                thread.Name = "startProcess() Thread";
                thread.Start();
                this.serverUpdateTimer.Start();

                //-------------------------
                //sp edit:
                //enable ClientCrash button...

                //i felt that this is not nesccery as started and stop flags are there
                //this.emuCrash = true;

                //------------------------------
            }

            //make sure this variable is set so we can use it to track this RM object...
            Started = true;
            return true;
            //}
            //catch(Exception e)
            //{
            //    
            //   MessageBox.Show(e.Message,"RM Start()");
            //   return false;
            //}
        }

        public void startProcess()
        {
            this.stopParamsUpdateInProgress = false;
            if (this.sendEventToTracker(this.currentTorrent, "&event=started"))
            {
                this.updateProcessStarted = true;
                this.requestScrapeFromTracker(this.currentTorrent);
            }
        }

        public void Stop()
        {
            this.stopTimerAndCounters();
            Thread thread = new Thread(new ThreadStart(this.stopProcess));
            thread.Name = "stopProcess() Thread";
            thread.Start();
            Started = false;
            //update the state of this RM object in the main UI
            Program.mainUI.updateStats(rmIndex, 3, "Stopped");
        }

        public void stopProcess()
        {
            this.sendEventToTracker(this.currentTorrent, "&event=stopped");
        }

        //this will be modified so that it updates a set of variables...
        public void updateCounters(TorrentInfo torrentInfo)
        {
            //this is for the stats updating of ratio in the mainUI this has got nothing to do with normal RM oparations
            double ratio, down, up;

            /* i am guessing that this is not requared as we are not dealing with a lable here
            if (this.uploadCount.InvokeRequired)
            {
                SetCountersCallback method = new SetCountersCallback(this.updateCounters);
                base.Invoke(method, new object[] { torrentInfo });
            }
            else
            {
             */
            Random random = new Random();
            strUploadCount = this.FormatFileSize((ulong)torrentInfo.uploaded);

            //mainUI stats...
            up = torrentInfo.uploaded;

            if (torrentInfo.uploadRate > 0L)
            {
                long num = (torrentInfo.uploadRate + random.Next(0x2800)) - 0x1400L;
                if (num < 0L)
                {
                    num = 0L;
                }
                torrentInfo.uploaded += num;
            }
            strDownloadCount = this.FormatFileSize((ulong)torrentInfo.downloaded);

            //mainUI stats...
            down = torrentInfo.downloaded;

            if (!this.seedMode && (torrentInfo.downloadRate > 0L))
            {
                long num2 = (torrentInfo.downloadRate + random.Next(0x2800)) - 0x1400L;
                if (num2 < 0L)
                {
                    num2 = 0L;
                }
                torrentInfo.downloaded += num2;
                torrentInfo.left = torrentInfo.totalsize - torrentInfo.downloaded;
            }
            if (torrentInfo.left <= 0L)
            {
                torrentInfo.downloaded = torrentInfo.totalsize;
                torrentInfo.left = 0L;
                torrentInfo.downloadRate = 0L;
                if (!this.seedMode)
                {
                    this.currentTorrent = torrentInfo;
                    this.seedMode = true;
                    this.temporaryIntervalCounter = 0;
                    Thread thread = new Thread(new ThreadStart(this.completedProcess));
                    thread.Name = "completedProcess() Thread";
                    thread.Start();
                }
            }
            this.currentTorrent = torrentInfo;
            if (torrentInfo.totalsize == 0L)
            {
                strFileSizeText = "100";
            }
            else
            {
                float num3 = (((float)(this.currentTorrentFile.totalLength - ((ulong)torrentInfo.left))) / ((float)this.currentTorrentFile.totalLength)) * 100f;
                strFileSizeText = (num3 >= 100f) ? "100" : num3.ToString("F2");
            }

            //ok now everything we need are in the variables so lets get them to the mainUI
            Program.mainUI.updateStats(rmIndex, 8, strUploadCount);
            Program.mainUI.updateStats(rmIndex, 9, strDownloadCount);
            Program.mainUI.updateStats(rmIndex, 2, strFileSizeText + "%");


            //get the raito, these variables were assigned the needed values in the top of this method...
            if (down != 0)
            {
                ratio = up / down;
                //update the raito stats...
                Program.mainUI.updateStats(rmIndex, 10, ratio.ToString().Substring(0, 5));
            }
            else
            {
                //update the raito stats... ratio is infinite as down == 0
                Program.mainUI.updateStats(rmIndex, 10, "infinit");
            }
        }

        //this is called when the tracker wants up to ajuest our interval....
        public void updateInterval(string interval)
        {
            //check whether we should override tracker's request or not
            if (this.boolIntervalOveride == false)
            {
                int result = 0;
                if (int.TryParse(interval, out result))
                {
                    if (result > 0xe10)
                    {
                        result = 0xe10;
                    }
                    if (result < 60)
                    {
                        result = 60;
                    }
                    this.currentTorrent.interval = result;
                    this.AddLogLine("Updating Interval: " + result);
                    this.temporaryIntervalCounter = 5;

                    //lets up date this on the mainUI
                    //Program.mainUI.updateStats(rmIndex, 13, result.ToString());
                }
            }
            else
            {
                this.AddLogLine("Tracker's request to update the interval to: " + interval + " ignored because of user chooesed to do so");
            }
        }



        public void Crash()
        {
            this.stopTimerAndCounters(); // clear everything
            Program.mainUI.updateStats(rmIndex, 3, "Crashed");
        }


        #endregion
        
        #region Private Methods

        private void AcceptTcpConnection()
        {
            Socket socket = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding(0x6faf);
                string str = null;
                while (true)
                {
                    byte[] buffer;
                    do
                    {
                        socket = this.localListen.AcceptSocket();
                        buffer = new byte[0x43];
                    }
                    while ((socket == null) || !socket.Connected);
                    NetworkStream stream = new NetworkStream(socket);
                    stream.ReadTimeout = 0x3e8;
                    try
                    {
                        stream.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception)
                    {
                    }
                    str = encoding.GetString(buffer, 0, buffer.Length);
                    if ((str.IndexOf("BitTorrent protocol") >= 0) && (str.IndexOf(encoding.GetString(this.currentTorrentFile.InfoHash)) >= 0))
                    {
                        byte[] buffer2 = this.createHandshakeResponse();
                        stream.Write(buffer2, 0, buffer2.Length);
                    }
                    socket.Close();
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch (Exception exception)
            {
                this.AddLogLine("Error in AcceptTcpConnection(): " + exception.ToString());
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                    this.AddLogLine("Closed socket");
                }
                this.CloseTcpListener();
            }
        }

        private void CloseTcpListener()
        {
            try
            {
                if (this.localListen != null)
                {
                    this.localListen.Stop();
                    this.localListen = null;
                    this.AddLogLine("TCP Listener closed");
                }
            }
            catch (Exception e)
            {
                manageError(e);
            }
        }

        private byte[] createChokeResponse()
        {
            byte[] buffer2 = new byte[5];
            buffer2[3] = 1;
            return buffer2;
        }

        private byte[] createHandshakeResponse()
        {
            try
            {
                int byteIndex = 0;
                Encoding encoding = Encoding.GetEncoding(0x6faf);
                new StringBuilder();
                string s = "BitTorrent protocol";
                byte[] bytes = new byte[0x100];
                bytes[byteIndex++] = (byte)s.Length;
                encoding.GetBytes(s, 0, s.Length, bytes, byteIndex);
                byteIndex += s.Length;
                for (int i = 0; i < 8; i++)
                {
                    bytes[byteIndex++] = 0;
                }
                Buffer.BlockCopy(this.currentTorrentFile.InfoHash, 0, bytes, byteIndex, this.currentTorrentFile.InfoHash.Length);
                byteIndex += this.currentTorrentFile.InfoHash.Length;
                encoding.GetBytes(this.currentTorrent.peerID.ToCharArray(), 0, this.currentTorrent.peerID.Length, bytes, byteIndex);
                byteIndex += encoding.GetByteCount(this.currentTorrent.peerID);
                return bytes;
            }
            catch (Exception e)
            {
                manageError(e);
                return null;
            }
        }

        private Socket createRegularSocket()
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception exception)
            {
                this.AddLogLine("createSocket error: " + exception.ToString());
            }
            return socket;
        }

        private SocketEx createSocket()
        {
            SocketEx ex = null;
            try
            {
                ex = new SocketEx(this.currentProxy.proxyType, this.currentProxy.proxyServer, this.currentProxy.proxyPort, this.currentProxy.proxyUser, this.currentProxy.proxyPassword);
                ex.SetTimeout(0x30d40);
            }
            catch (Exception exception)
            {
                this.AddLogLine("createSocket error: " + exception.ToString());
            }
            return ex;
        }

        private long getStopProcessUnitsMultiplyer()
        {
            long num = 0L;
            switch (intStopProcessActionBoxSelectedIndex)
            {
                case 0:
                    return 0L;

                case 1:
                case 2:
                    switch (intStopProcessUnitsBoxSelectedIndex)
                    {
                        case 0:
                            return 0x400L;

                        case 1:
                            return 0x100000L;

                        case 2:
                            return 0x40000000L;
                    }
                    return num;

                case 3:
                    switch (intStopProcessUnitsBoxSelectedIndex)
                    {
                        case 0:
                            return 1L;

                        case 1:
                            return 60L;

                        case 2:
                            return 0xe10L;
                    }
                    return num;
            }
            return num;
        }

        private bool isStopProcessCondition()
        {
            string results; //this for the UI updating purposes...

            if (this.stopParamsUpdateInProgress)
            {

                //update the state of this RM object in the main UI 12 - Remaining
                Program.mainUI.updateStats(rmIndex, 12, "Paras Update in Progress");

                return false;
            }
            long num = (long)(this.parseValidFloat(strStopProcessValue, 0f) * this.getStopProcessUnitsMultiplyer());
            bool flag = false;
            switch (intStopProcessActionBoxSelectedIndex)
            {
                case 0:
                    //update the state of this RM object in the main UI 12 - Remaining
                    Program.mainUI.updateStats(rmIndex, 12, "Disabled");
                    return false;

                case 1:
                    if (num <= 0L)
                    {
                        flag = false;
                    }


                    if (this.currentTorrent.uploaded <= num)
                    {
                        //result =  (int) (num - this.currentTorrent.uploaded);
                        results = this.FormatFileSize((ulong)(num - this.currentTorrent.uploaded));
                        //update the state of this RM object in the main UI 12 - Remaining
                        Program.mainUI.updateStats(rmIndex, 12, results + " 2b uploaded");
                    }
                    return (this.currentTorrent.uploaded > num);

                case 2:
                    if (num <= 0L)
                    {
                        flag = false;
                    }

                    if (this.currentTorrent.downloaded <= num)
                    {
                        //result =  (int) (num - this.currentTorrent.downloaded);
                        results = this.FormatFileSize((ulong)(num - this.currentTorrent.downloaded));

                        //update the state of this RM object in the main UI 12 - Remaining
                        Program.mainUI.updateStats(rmIndex, 12, results + " 2b downloaded");
                    }
                    return (this.currentTorrent.downloaded > num);

                case 3:
                    if (num <= 0L)
                    {
                        flag = false;
                    }

                    if (this.totalRunningTimeCounter <= num)
                    {
                        int resulti = (int)num - this.totalRunningTimeCounter;

                        //update the state of this RM object in the main UI 12 - Remaining
                        Program.mainUI.updateStats(rmIndex, 12, resulti.ToString() + " seconds");
                    }
                    return (this.totalRunningTimeCounter > num);
            }
            return flag;
        }

        private TrackerResponse MakeWebRequestEx(Uri reqUri)
        {
            Encoding encoding = Encoding.GetEncoding(0x6faf);
            SocketEx ex = null;
            TrackerResponse response = null;
            try
            {
                string host = reqUri.Host;
                int port = reqUri.Port;
                string pathAndQuery = reqUri.PathAndQuery;
                this.AddLogLine("----------- Connecting to tracker : " + host + " -------- Port : " + port.ToString());
                ex = this.createSocket();
                ex.PreAuthenticate = false;
                int num2 = 0;
                bool flag = false;
                while ((num2 < 5) && !flag)
                {
                    try
                    {
                        ex.Connect(host, port);
                        flag = true;
                        this.AddLogLine("Connected Successfully");
                        continue;
                    }
                    catch (Exception exception)
                    {
                        this.AddLogLine("Exception: " + exception.ToString() + "; Type: " + exception.GetType().ToString());
                        this.AddLogLine("Failed connection attempt: " + num2.ToString());
                        num2++;
                        continue;
                    }
                }
                string logLine = "GET " + pathAndQuery + " " + this.currentClient.HttpProtocol + "\r\n" + this.currentClient.Headers.Replace("{host}", host) + "\r\n";
                this.AddLogLine("----------- Sending Command to Tracker --------");
                this.AddLogLine(logLine);
                ex.Send(encoding.GetBytes(logLine));
                try
                {
                    byte[] buffer = new byte[0x8000];
                    MemoryStream responseStream = new MemoryStream();
                    while (true)
                    {
                        int count = ex.Receive(buffer);
                        if (count == 0)
                        {
                            break;
                        }
                        responseStream.Write(buffer, 0, count);
                    }
                    if (responseStream.Length == 0L)
                    {
                        this.AddLogLine("Error : Tracker Response is empty");
                        return null;
                    }
                    response = new TrackerResponse(responseStream);
                    if (response.doRedirect)
                    {
                        return this.MakeWebRequestEx(new Uri(response.RedirectionURL));
                    }
                    this.AddLogLine("----------- Tracker Response --------");
                    this.AddLogLine(response.Headers);
                    if (response.Dict == null)
                    {
                        this.AddLogLine("*** Failed to decode tracker response :");
                        this.AddLogLine(response.Body);
                    }
                    responseStream.Dispose();
                }
                catch (Exception exception2)
                {
                    this.AddLogLine(Environment.NewLine + exception2.Message);
                    response = null;
                }
            }
            catch (Exception exception3)
            {
                this.AddLogLine("Exception:" + exception3.Message);
            }
            if (ex != null)
            {
                ex.Close();
            }
            return response;
        }
        
        private void OpenTcpListener()
        {
            
            
            try
            {
                if ((boolTCPListen && (this.localListen == null)) && (this.currentProxy.proxyType == ProxyType.None))
                {
                    this.localListen = new TcpListener(int.Parse(this.currentTorrent.port));
                    this.localListen.Start();
                    this.AddLogLine("Started TCP listener on port " + this.currentTorrent.port);
                    Thread thread = new Thread(new ThreadStart(this.AcceptTcpConnection));
                    thread.Name = "AcceptTcpConnection() Thread";
                    thread.Start();
                }
            }
            catch (Exception exception)
            {
                //check wether two torrents are running on one port and thus is causing a exception

                this.AddLogLine("Failed to open Tcp Listener");

                if (exception.Message == "Only one usage of each socket address (protocol/network address/port) is normally permitted")
                { 
                    this.AddLogLine("It seems that you already have a RM or some other client listning in the same port[" + this.currentTorrent.port + "], so the error occured\n");
                    this.AddLogLine("hence there is already a listening process on the port, RB will not try to open a tcp listenner there again...");
                    this.boolTCPListen = false;
                }
                
                this.AddLogLine(exception.ToString());
                if (this.localListen != null)
                {
                    this.localListen.Stop();
                    this.localListen = null;
                }
            }
        }

        private void serverUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (this.updateProcessStarted)
            {
                if (this.haveInitialPeers)
                {
                    this.updateCounters(this.currentTorrent);
                }
                int seconds = this.currentTorrent.interval - this.temporaryIntervalCounter;
                this.totalRunningTimeCounter++;
                this.strTotalRunningTime = this.ConvertToTime(this.totalRunningTimeCounter);

                //update the mainUI
                Program.mainUI.updateStats(rmIndex, 11, strTotalRunningTime);

                if (this.isStopProcessCondition())
                {
                    //this was replace with the Stop method
                    //this.StopButton_Click(null, null);
                    this.Stop();
                }
                else if (seconds > 0)
                {
                    this.temporaryIntervalCounter++;

                    this.strTimerValue = this.ConvertToTime(seconds);

                    //update the mainUI
                    Program.mainUI.updateStats(rmIndex, 13, this.strTimerValue.ToString());
                }
                else
                {
                    this.randomiseSpeeds();
                    this.OpenTcpListener();
                    Thread thread = new Thread(new ThreadStart(this.continueProcess));
                    this.temporaryIntervalCounter = 0;
                    this.strTimerValue = "0";

                    //update the mainUI
                    Program.mainUI.updateStats(rmIndex, 13, this.strTimerValue.ToString());

                    thread.Name = "continueProcess() Thread";
                    thread.Start();
                }
            }

            //i am using this event to update some stats.. scrap stats... it seems that for some
            //strange reason i can't update it from the normal method... ?????? :|
            //let's update the main UI with these scrap stats
            Program.mainUI.updateStats(rmIndex, 4, strScrapStatSeeders);
            Program.mainUI.updateStats(rmIndex, 5, strScrapStatLeechs);
        }

        private void updateScrapStats(string seedStr, string leechStr, string finishedStr)
        {
            //strScrapStatSeeders = "Seeders: " + seedStr;
            //strScrapStatLeechs = "Leechers: " + leechStr;

            strScrapStatSeeders = seedStr;
            strScrapStatLeechs = leechStr;


            //let's update the main UI with these scrap stats
            Program.mainUI.updateStats(rmIndex, 4, strScrapStatSeeders);
            Program.mainUI.updateStats(rmIndex, 5, strScrapStatLeechs);

            //this.updateTextBox(this.seedLabel, "Seeders: " + seedStr);
            //this.updateTextBox(this.leechLabel, "Leechers: " + leechStr);
            this.scrapStatsUpdated = true;
        }

        private void stopTimerAndCounters()//left
        {
            /*
            if (this.StartButton.InvokeRequired)
            {
                stopTimerAndCountersCallback method = new stopTimerAndCountersCallback(this.stopTimerAndCounters);
                base.Invoke(method, new object[0]);
            }
            else
            {
             */
            this.serverUpdateTimer.Stop();
            this.Started = false;
            //this.StartButton.Enabled = true;
            //this.StopButton.Enabled = false;
            //this.manualUpdateButton.Enabled = false;
            this.CloseTcpListener();
            this.temporaryIntervalCounter = 0;
            //this.timerValue.Text = "0";
            this.strTimerValue = "0";
            this.currentTorrent.numberOfPeers = "0";
            this.updateProcessStarted = false;


        }

        private void manageError(Exception error)
        {
            this.AddLogLine(error.ToString());
            MessageBox.Show(error.ToString());
        }
        
        #endregion  

    }
}
