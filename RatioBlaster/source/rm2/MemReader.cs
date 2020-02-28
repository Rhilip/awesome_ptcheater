using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ProcessMemoryReaderLib;
using rm2.UI;
using RatioBlaster.rm2.Controllers;

namespace rm2
{
    class MemReader
    {

        #region Member Variables
        //-----------------------------------
        //------- Original Variables --------
        //rm2.RM currentRMIns;

        private TorrerntSettingsUI _mainForm;
        private int absoluteEndOffset = 0x1fffffff; //this is needed
        private int absoluteStartOffset;            //this is needed as well
        private uint bufferSize = 0x10000;
        private string clientSearchString = "&peer_id=-UT1600-";
        private TorrentClient currentClient;
        private string currentClientProcessName = "utorrent";
        private int currentOffset;
        public string customPeersNum;
        private Encoding enc = Encoding.ASCII;
        private ProcessMemoryReader pReader;

        public string reCustomPort;
        private string reHashBox;
        private string reKeyBox;

        private string rePeerIdBox;
        //private string reProcessNameBox;
        //private string reSearchStr;

        //---------------------------------
        //---------End Originals ----------

        #endregion
        
        //------------- methods -----------------

        private void reApplyButton() 
        {
            /*
            this._mainForm.customPeerID.Text = this.rePeerIdBox;
            if (this.customPeersNum != "0")
            {
                this._mainForm.customPeersNum.Text = this.customPeersNum;
            }
            this._mainForm.customKey.Text = this.reKeyBox;
            this._mainForm.customPort.Text = this.reCustomPort;
            //this.reApplyButton.Enabled = false;
             */

            string[] pars = { this.rePeerIdBox.ToString(), this.customPeersNum.ToString(), 
                this.reKeyBox.ToString(), this.reCustomPort.ToString() };
            
            _mainForm.setMemReaderValues(pars);
        }

//        public bool reSearchButton(string reSearchStr, string reProcessNameBox) 
        public void reSearchButton() 
        {
            //this.clientSearchString = clientSearchString;
            //this.currentClientProcessName = currentClientProcessName;
            if (this.InitSearch())
            {//process found

                if (this.StartSearch())
                {
                    reApplyButton();

                   // return true;//this means that the the values were found and successfully extracted
                    //so this method can now call reAlpplyButton() to get the stuff applied
                    //to the text boxes in the form...
                    _mainForm.setLabelMemReaderStatus("Automatic Memory Reader Successful");
                }
                else
                {
                    //return false;
                    _mainForm.setLabelMemReaderStatus("Automatic Memory Reader Unsuccessful");
                    
                }
            }
            else
            {
                //return false;//process not found
                _mainForm.setLabelMemReaderStatus("Automatic Memory Reader Unsuccessful, process not found");
                
            }
        }

        private bool StartSearch()
        {
            int num2 = 0;
            bool flag = false;
            this.pReader.OpenProcess();
            //this.reProgressBar.Maximum = this.absoluteEndOffset;
            //this.reProgressBar.Minimum = this.absoluteStartOffset;
            //this.reProgressBar.Visible = true;
            
            
            //_mainForm.progressBar1.Maximum = this.absoluteEndOffset;
            //_mainForm.progressBar1.Minimum = this.absoluteStartOffset;
            //_mainForm.progressBar1.Visible = true;
            
            int[] parArry = {absoluteEndOffset, absoluteStartOffset, 0 };
            _mainForm.setProgressBarInit(parArry);
            
            while (this.currentOffset < this.absoluteEndOffset)
            {
                int num;
                //this.reProgressBar.Value = this.currentOffset;
                //_mainForm.progressBar1.Value = this.currentOffset;
                _mainForm.setProgressBarValue(this.currentOffset);

                byte[] memory = this.pReader.ReadProcessMemory((IntPtr)this.currentOffset, this.bufferSize, out num);
                num2 = this.getStringOffsetInsideArray(memory);
                if (num2 >= 0)
                {
                    flag = true;
                    string input = this.enc.GetString(memory);
                    Match match = new Regex("&peer_id=(.+?)(&| )", RegexOptions.Compiled).Match(input);
                    if (match.Success)
                    {
                        this.rePeerIdBox = match.Groups[1].ToString();
                    }
                    match = new Regex("&key=(.+?)(&| )", RegexOptions.Compiled).Match(input);
                    if (match.Success)
                    {
                        this.reKeyBox = match.Groups[1].ToString();
                    }
                    match = new Regex("&port=(.+?)(&| )", RegexOptions.Compiled).Match(input);
                    if (match.Success)
                    {
                        this.reCustomPort = match.Groups[1].ToString();
                    }
                    match = new Regex("&numwant=(.+?)(&| )", RegexOptions.Compiled).Match(input);
                    if (match.Success)
                    {
                        this.customPeersNum = match.Groups[1].ToString();
                    }
                    match = new Regex("&info_hash=(.+?)(&| )", RegexOptions.Compiled).Match(input);
                    if (match.Success)
                    {
                        this.reHashBox = match.Groups[1].ToString();
                    }
                    num2 += this.currentOffset;
                    break;
                }
                this.currentOffset = (this.currentOffset + ((int)this.bufferSize)) - 0x200;
            }
            this.pReader.CloseHandle();
            //_mainForm.progressBar1.Visible = false;
            //this.reProgressBar.Visible = false;
            parArry[0] = absoluteEndOffset;
            parArry[1] = absoluteStartOffset;
            parArry[2] = 1;
            _mainForm.setProgressBarInit(parArry);
            
            
            if (flag)
            {
                return true;
                //MessageBox.Show("Search finished successfully!\r\nPress Apply button to apply found values to current client.", "Search", MessageBoxButtons.OK);
                //this.reApplyButton.Enabled = true;
                //_mainForm.setLabelMemReaderStatus("Search finished successfully!");
            }
            else
            {
                return false;
                //MessageBox.Show("Search failed.\r\nMake sure that torrent client is running and that at least one torrent is working.", "Search", MessageBoxButtons.OK);
                //_mainForm.setLabelMemReaderStatus("Search failed");
            }
        }

        public MemReader(rm2.UI.TorrerntSettingsUI callerForm, TorrentClient parCClient)
        {
            this._mainForm = callerForm;

            //this.currentClient = this._mainForm.getCurrentClient();

            this.currentClient = parCClient;
            
            this.currentClientProcessName = this.currentClient.ProcessName;
            this.clientSearchString = "&peer_id=" + this.currentClient.PeerIDPrefix;
            //this.reSearchStr.Text = this.clientSearchString;
            //this.reProcessNameBox.Text = this.currentClientProcessName;


        }

        //overloaded method when the user gives us the cilentSearchString
        public MemReader(rm2.UI.TorrerntSettingsUI callerForm, TorrentClient parCClient, string clientSString)
        {
            this._mainForm = callerForm;

            //this.currentClient = this._mainForm.getCurrentClient();

            this.currentClient = parCClient;

            this.currentClientProcessName = this.currentClient.ProcessName;
            //this.clientSearchString = "&peer_id=" + this.currentClient.PeerIDPrefix;
            this.clientSearchString = clientSString; //take the user give clientSearchString
            
            
            //this.reSearchStr.Text = this.clientSearchString;
            //this.reProcessNameBox.Text = this.currentClientProcessName;


        }

        private Process FindProcessByName(string processName)
        {
            AddLogLine("Looking for client process...");
            Process[] processesByName = Process.GetProcessesByName(processName);
            if (processesByName.Length == 0)
            {
                string logLine = "No " + this.currentClientProcessName + " process found .\r\nMake sure that torrent client is running and that at least one torrent is working.";
                AddLogLine(logLine);
                //MessageBox.Show(logLine, "Search", MessageBoxButtons.OK);
                return null;
            }
            AddLogLine(this.currentClientProcessName + " process found! ");
            return processesByName[0];
        }

        private int getStringOffsetInsideArray(byte[] memory)
        {
            return this.enc.GetString(memory).IndexOf(this.clientSearchString);
        }

        private bool InitSearch()
        {
            Process process = this.FindProcessByName(this.currentClientProcessName);
            if (process == null)
            {
                return false;
            }
            this.currentOffset = this.absoluteStartOffset;
            this.pReader = new ProcessMemoryReader();
            this.pReader.ReadProcess = process;
            return true;
        }

        private void AddLogLine(string logLine)
        {
            _mainForm.setLabelMemReaderStatus(logLine);
            //Logger.memErrLog = Logger.memErrLog + "\n \n" + logLine;
            Logger.Log(logLine, this);
        }
    }
}
