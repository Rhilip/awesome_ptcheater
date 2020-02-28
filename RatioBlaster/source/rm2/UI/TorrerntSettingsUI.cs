namespace rm2.UI
{
    using BitTorrent;
    using BytesRoad.Net.Sockets;
    
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using rm2;

    //---------
    //sp edit
    using System.Timers;

    public class TorrerntSettingsUI : Form
    {
        //----------------------------------------------------
        //silentp33r edit:
        //i am gonna declar some static variables here



        //----------------------------------------------------

        private ApplicationSettings applicationSettings;//
        private Button applyStopSettingsButton;//
        private Button browseButton;//
        //
        public CheckBox checkIgnoreFailureReason;
        public CheckBox checkRandomDownload;
        public CheckBox checkRandomUpload;
        public CheckBox checkRequestScrap;
        public CheckBox checkTCPListen;
        //
        private Label ClientLabel;
        public ComboBox comboProxyType;
        //---
        private IContainer components;
        public TorrentClient currentClient;
        public ProxyInfo currentProxy;
        public TorrentInfo currentTorrent;
        public Torrent currentTorrentFile;
        //---------
        public TextBox customKey;
        public TextBox customPeerID;
        public TextBox customPeersNum;
        public TextBox customPort;
        public TextBox downloadRate;
        private Label downloadRateLabel;
        public TextBox fileSize;
        private Label FileSizeLabel;
        private GroupBox groupBox2;
        private GroupBox groupNetworkMisc;
        private GroupBox groupTorrentFile;
        private GroupBox groupTorrentInfo;
        private Label hashLabel;
        

        
        public Icon iconForTray;
        public TextBox interval;
        private Label intervalLabel;
        
        public string ipAddress = "";//

        private Label keyLabel;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label4;
        private Label labelProxyHost;
        private Label labelProxyPass;
        private Label labelProxyPort;
        private Label labelProxyType;
        private Label labelProxyUser;
        private Label labelStopMinLeecher;
        private Label labelTorrentSize;
        private LinkLabel linkProxyHelp;
        private Label numwantLabel;
        private OpenFileDialog openFileDialog1;
        private Label portInfoLabel;
        private Label portLabel;
        private GroupBox proxySettingsGroup;
        
        private Random random = new Random((int) DateTime.Now.Ticks);//
        
        public TextBox RandomDownloadFrom;
        private Label RandomDownloadFromLabel;
        public TextBox RandomDownloadTo;
        private Label RandomDownloadToLabel;
        private GroupBox randomSpeedGroup;
        public TextBox RandomUploadFrom;
        private Label RandomUploadFromLabel;
        public TextBox RandomUploadTo;
        private Label RandomUploadToLabel;
        private GroupBox reportParamsGroup;//
        
        public TextBox shaHash;
        private Button StartButton;
        
        public ComboBox stopProcessActionBox;
        private Label stopProcessLabel;
        public ComboBox stopProcessUnitsBox;
        public TextBox stopProcessValue;
        private TabPage tabAdvanced;
        private TabControl tabControl1;
        private TabPage tabGeneral;
        private TabPage tabNetwork;
        public int temporaryIntervalCounter; //
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        public TextBox textProxyHost;
        public TextBox textProxyPass;
        public TextBox textProxyPort;
        public TextBox textProxyUser;
        public NumericUpDown textStopMinLeecher;
        private TorrentClient[] TorrentClients; //
        public ComboBox TorrentClientsBox;
        private TorrentClientsEnum TorrentClientsObj; 
        public TextBox torrentFile;
        private Label TorrentFileLabel;
        private Label torrentSize;
        public int totalRunningTimeCounter;//
        private TextBox trackerAddress;
        private Label TrackerLabel;


        public CheckBox updateAnnounceParamsOnStart;//
        public TextBox uploadRate;
        private Label uploadRateLabel;
        private Label labDownkbits;
        private Label labUpkbits;
        public Label labAutoMem;
        public ProgressBar progressBar1;
        public CheckBox checkAutoMemReader;
        private CheckBox chkUnifiedClientSimu;
        public Label labUCSstatus;
        private TextBox txtCustomSearchString;
        private CheckBox checkBoxCustomSearchString;
        private Label labDefultSearchString;


        //----sp edit----
        public TorrentSession currentRMIns;
        
        //--------------

        public TorrerntSettingsUI()
        {
            InitForm();
        }

        private void InitForm()
        {
            Application.EnableVisualStyles();
            this.InitializeComponent();
            //this.versionChecker = new VersionChecker(this);
            this.applicationSettings = new ApplicationSettings(this);
            this.TorrentClientsObj = new TorrentClientsEnum(this);

            //this.Text = "Ratio Master " + this.versionChecker.PublicVersion;
            //this.versionAboutLabel.Text = this.versionChecker.PublicVersion;

            //------------

            /*//the RM object this GUI is associated with
            if (Program.RMcount != -1)
            {
                currentRMIns = Program.rmCol[Program.RMcount];
            }*/
        }


        private void adjustProcessActionGui()//not transfering this to RM.class
        {
            switch (this.stopProcessActionBox.SelectedIndex)
            {
                case 0:
                    this.stopProcessValue.Visible = false;
                    this.stopProcessUnitsBox.Visible = false;
                    this.applyStopSettingsButton.Visible = false;
                    return;

                case 1:
                    this.stopProcessValue.Visible = true;
                    this.stopProcessUnitsBox.Visible = true;
                    this.stopProcessUnitsBox.DataSource = System.Enum.GetValues(typeof(SizeUnits));
                    return;

                case 2:
                    this.stopProcessValue.Visible = true;
                    this.stopProcessUnitsBox.Visible = true;
                    this.stopProcessUnitsBox.DataSource = System.Enum.GetValues(typeof(SizeUnits));
                    return;

                case 3:
                    this.stopProcessValue.Visible = true;
                    this.stopProcessUnitsBox.Visible = true;
                    this.stopProcessUnitsBox.DataSource = System.Enum.GetValues(typeof(TimeUnits));
                    return;
            }
        }

        private void applyStopSettingsButton_Click(object sender, EventArgs e)
        {
            currentRMIns.stopParamsUpdateInProgress = false;
            this.applyStopSettingsButton.Visible = false;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void checkRequestScrap_CheckedChanged(object sender, EventArgs e)
        {
            currentRMIns.boolRequestScrap = this.checkRequestScrap.Checked;
            this.textStopMinLeecher.Enabled = this.checkRequestScrap.Checked;
        }

        

        
        private void closeButton_Click(object sender, EventArgs e)
        {
            //this.ExitRatioMaster();
        }

        public void deployDefaultValues()//this was transfered and then commented out... in RM.class
        {
            TorrentInfo info = new TorrentInfo(0L, 0L);
            this.trackerAddress.Text = info.tracker;
            this.shaHash.Text = info.hash;
            this.TorrentClients = this.TorrentClientsObj.TorrentClients;
            this.TorrentClientsBox.DataSource = this.TorrentClients;
            this.TorrentClientsBox.DisplayMember = "Name";
            this.comboProxyType.SelectedIndex = 0;
            this.stopProcessActionBox.SelectedIndex = 0;
            this.applicationSettings.LoadAppSettings();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        

        private void downloadRate_TextChanged(object sender, EventArgs e)
        {
            //this.currentTorrent.downloadRate = (long) (this.parseValidFloat(this.downloadRate.Text, 10f) * 1024f);
            currentRMIns.setDownSpeed(this.downloadRate.Text);
            //sp edit: lets have the KB/s values pluse kbit/s values for our display

            try
            {
                if (downloadRate.Text.Equals(""))
                {
                    this.labDownkbits.Text = "0";
                }
                else
                {
                    double temp = double.Parse(downloadRate.Text);
                    temp = temp * 8;
                    this.labDownkbits.Text = temp.ToString() + " kbit/s";
                }
                //--------------------------
            }            
            catch (FormatException)//this occures when you type only "." or some alpha chars...
            {
                //ignore this...
            }
        }

        

      

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (data == null)
            {
                data = (string[]) e.Data.GetData("System.String[]", true);
                if (data == null)
                {
                    return;
                }
            }
            this.loadTorrentFileInfo(data[0], true);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text)) || e.Data.GetFormats().ToString().Equals("System.String[]"))
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            this.deployDefaultValues();

            if (this.updateAnnounceParamsOnStart.Checked)
            {
                this.TorrentClientsBox_SelectedIndexChanged(null, null);
            }
            this.checkRequestScrap_CheckedChanged(null, null);

            //added this so the defult up/down speeds will always change randomly when starting up...
            // its not needed, first update should be according to what user set
            //this.randomiseDefultSpeeds();
           
        }

        //this is to randomize speed when this form is been loaded... so there will be no defult single set of values

        public void randomiseDefultSpeeds()
        {
            //string strUploadRate;
            //string strDownloadRate;

            //do some inputvalidation first
            bool inpValResult = inputValidation(false);

            if (inpValResult == true)
            {

                try
                {
                    //double numbers don' go with int so get it to double and convert in to int as rounded numbers
                    double temp;
                    double.TryParse(RandomUploadFrom.Text, out temp);//got it as a double value

                    int upFrom, upTo, downFrom, downTo;//declare the variables

                    upFrom = Convert.ToInt32(temp);

                    double.TryParse(RandomUploadTo.Text, out temp);//got it as a double value
                    upTo = Convert.ToInt32(temp);

                    double.TryParse(this.RandomDownloadFrom.Text, out temp);//got it as a double value
                    downFrom = Convert.ToInt32(temp);

                    double.TryParse(this.RandomDownloadTo.Text, out temp);//got it as a double value
                    downTo = Convert.ToInt32(temp);

                    ////////////////////////////////

                    float num = ((float)this.random.Next(100)) / 100f;
                    float num2 = ((float)this.random.Next(100)) / 100f;
                    //if (boolRandomUpload == true)
                    //{
                    uploadRate.Text = (this.random.Next(upFrom, upTo) + num).ToString();
                    Program.mainUI.updateStats(currentRMIns.rmIndex, 7, uploadRate.Text + " KB/s");

                    //  setUploadRate(strUploadRate);

                    //}
                    //if (boolRandomDownload == true)
                    //{
                    downloadRate.Text = (this.random.Next(downFrom, downTo) + num2).ToString();
                    Program.mainUI.updateStats(currentRMIns.rmIndex, 6, downloadRate.Text + " KB/s");

                    //setDownSpeed(strDownloadRate);
                    //}
                }

                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "randomiseDefultSpeeds() -> in New RM Dialog");

                }
            }
        }

        
        private ProxyInfo getCurrentProxy()//transfered, but this will be needed here, to genrate a proxyinfo varia
            //ble and pass it to the RM object...
        {
            Encoding encoding = Encoding.GetEncoding(0x6faf);
            ProxyInfo info = new ProxyInfo();
            switch (this.comboProxyType.SelectedIndex)
            {
                case 0:
                    info.proxyType = ProxyType.None;
                    break;

                case 1:
                    info.proxyType = ProxyType.HttpConnect;
                    break;

                case 2:
                    info.proxyType = ProxyType.Socks4;
                    break;

                case 3:
                    info.proxyType = ProxyType.Socks4a;
                    break;

                case 4:
                    info.proxyType = ProxyType.Socks5;
                    break;

                default:
                    info.proxyType = ProxyType.None;
                    break;
            }
            info.proxyServer = this.textProxyHost.Text;
            info.proxyPort = this.ParseValidInt(this.textProxyPort.Text, 0);
            info.proxyUser = encoding.GetBytes(this.textProxyUser.Text);
            info.proxyPassword = encoding.GetBytes(this.textProxyPass.Text);
            return info;
        }


        private void InitializeComponent()//transfered and edited, only the studd relating to timer was transfered
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.StartButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabNetwork = new System.Windows.Forms.TabPage();
            this.groupNetworkMisc = new System.Windows.Forms.GroupBox();
            this.checkIgnoreFailureReason = new System.Windows.Forms.CheckBox();
            this.textStopMinLeecher = new System.Windows.Forms.NumericUpDown();
            this.labelStopMinLeecher = new System.Windows.Forms.Label();
            this.interval = new System.Windows.Forms.TextBox();
            this.intervalLabel = new System.Windows.Forms.Label();
            this.checkRequestScrap = new System.Windows.Forms.CheckBox();
            this.checkTCPListen = new System.Windows.Forms.CheckBox();
            this.proxySettingsGroup = new System.Windows.Forms.GroupBox();
            this.linkProxyHelp = new System.Windows.Forms.LinkLabel();
            this.textProxyPass = new System.Windows.Forms.TextBox();
            this.textProxyUser = new System.Windows.Forms.TextBox();
            this.labelProxyPass = new System.Windows.Forms.Label();
            this.labelProxyUser = new System.Windows.Forms.Label();
            this.textProxyPort = new System.Windows.Forms.TextBox();
            this.textProxyHost = new System.Windows.Forms.TextBox();
            this.labelProxyPort = new System.Windows.Forms.Label();
            this.labelProxyHost = new System.Windows.Forms.Label();
            this.comboProxyType = new System.Windows.Forms.ComboBox();
            this.labelProxyType = new System.Windows.Forms.Label();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.txtCustomSearchString = new System.Windows.Forms.TextBox();
            this.checkBoxCustomSearchString = new System.Windows.Forms.CheckBox();
            this.labUCSstatus = new System.Windows.Forms.Label();
            this.chkUnifiedClientSimu = new System.Windows.Forms.CheckBox();
            this.checkAutoMemReader = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labAutoMem = new System.Windows.Forms.Label();
            this.updateAnnounceParamsOnStart = new System.Windows.Forms.CheckBox();
            this.TorrentClientsBox = new System.Windows.Forms.ComboBox();
            this.ClientLabel = new System.Windows.Forms.Label();
            this.randomSpeedGroup = new System.Windows.Forms.GroupBox();
            this.RandomDownloadTo = new System.Windows.Forms.TextBox();
            this.RandomUploadTo = new System.Windows.Forms.TextBox();
            this.RandomDownloadFrom = new System.Windows.Forms.TextBox();
            this.RandomUploadFrom = new System.Windows.Forms.TextBox();
            this.RandomDownloadToLabel = new System.Windows.Forms.Label();
            this.RandomUploadToLabel = new System.Windows.Forms.Label();
            this.RandomDownloadFromLabel = new System.Windows.Forms.Label();
            this.RandomUploadFromLabel = new System.Windows.Forms.Label();
            this.checkRandomDownload = new System.Windows.Forms.CheckBox();
            this.checkRandomUpload = new System.Windows.Forms.CheckBox();
            this.reportParamsGroup = new System.Windows.Forms.GroupBox();
            this.customKey = new System.Windows.Forms.TextBox();
            this.customPeerID = new System.Windows.Forms.TextBox();
            this.keyLabel = new System.Windows.Forms.Label();
            this.customPeersNum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numwantLabel = new System.Windows.Forms.Label();
            this.portInfoLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.customPort = new System.Windows.Forms.TextBox();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupTorrentFile = new System.Windows.Forms.GroupBox();
            this.torrentFile = new System.Windows.Forms.TextBox();
            this.TorrentFileLabel = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labDownkbits = new System.Windows.Forms.Label();
            this.labUpkbits = new System.Windows.Forms.Label();
            this.applyStopSettingsButton = new System.Windows.Forms.Button();
            this.stopProcessUnitsBox = new System.Windows.Forms.ComboBox();
            this.stopProcessActionBox = new System.Windows.Forms.ComboBox();
            this.stopProcessValue = new System.Windows.Forms.TextBox();
            this.stopProcessLabel = new System.Windows.Forms.Label();
            this.fileSize = new System.Windows.Forms.TextBox();
            this.FileSizeLabel = new System.Windows.Forms.Label();
            this.uploadRate = new System.Windows.Forms.TextBox();
            this.downloadRate = new System.Windows.Forms.TextBox();
            this.uploadRateLabel = new System.Windows.Forms.Label();
            this.downloadRateLabel = new System.Windows.Forms.Label();
            this.groupTorrentInfo = new System.Windows.Forms.GroupBox();
            this.torrentSize = new System.Windows.Forms.Label();
            this.labelTorrentSize = new System.Windows.Forms.Label();
            this.shaHash = new System.Windows.Forms.TextBox();
            this.hashLabel = new System.Windows.Forms.Label();
            this.trackerAddress = new System.Windows.Forms.TextBox();
            this.TrackerLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.labDefultSearchString = new System.Windows.Forms.Label();
            this.tabNetwork.SuspendLayout();
            this.groupNetworkMisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textStopMinLeecher)).BeginInit();
            this.proxySettingsGroup.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.randomSpeedGroup.SuspendLayout();
            this.reportParamsGroup.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupTorrentFile.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupTorrentInfo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Torrent Files|*.torrent";
            this.openFileDialog1.ShowReadOnly = true;
            this.openFileDialog1.SupportMultiDottedExtensions = true;
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(498, 392);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(88, 23);
            this.StartButton.TabIndex = 22;
            this.StartButton.Text = "Start";
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(377, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(43, 20);
            this.textBox1.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(273, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Number of Peers:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(208, 39);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(41, 20);
            this.textBox2.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(143, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Key:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(6, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(354, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "If those fields are left empty, " + Program.APPLICATION_NAME + "will use random or default values";
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(6, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 17);
            this.label14.TabIndex = 1;
            this.label14.Text = "Port:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(68, 39);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(44, 20);
            this.textBox3.TabIndex = 0;
            // 
            // tabNetwork
            // 
            this.tabNetwork.Controls.Add(this.groupNetworkMisc);
            this.tabNetwork.Controls.Add(this.proxySettingsGroup);
            this.tabNetwork.Location = new System.Drawing.Point(4, 22);
            this.tabNetwork.Name = "tabNetwork";
            this.tabNetwork.Padding = new System.Windows.Forms.Padding(3);
            this.tabNetwork.Size = new System.Drawing.Size(570, 348);
            this.tabNetwork.TabIndex = 4;
            this.tabNetwork.Text = "Network";
            this.tabNetwork.UseVisualStyleBackColor = true;
            // 
            // groupNetworkMisc
            // 
            this.groupNetworkMisc.Controls.Add(this.checkIgnoreFailureReason);
            this.groupNetworkMisc.Controls.Add(this.textStopMinLeecher);
            this.groupNetworkMisc.Controls.Add(this.labelStopMinLeecher);
            this.groupNetworkMisc.Controls.Add(this.interval);
            this.groupNetworkMisc.Controls.Add(this.intervalLabel);
            this.groupNetworkMisc.Controls.Add(this.checkRequestScrap);
            this.groupNetworkMisc.Controls.Add(this.checkTCPListen);
            this.groupNetworkMisc.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupNetworkMisc.Location = new System.Drawing.Point(7, 117);
            this.groupNetworkMisc.Name = "groupNetworkMisc";
            this.groupNetworkMisc.Size = new System.Drawing.Size(557, 146);
            this.groupNetworkMisc.TabIndex = 4;
            this.groupNetworkMisc.TabStop = false;
            this.groupNetworkMisc.Text = "Miscellaneous";
            // 
            // checkIgnoreFailureReason
            // 
            this.checkIgnoreFailureReason.AutoSize = true;
            this.checkIgnoreFailureReason.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkIgnoreFailureReason.Location = new System.Drawing.Point(8, 113);
            this.checkIgnoreFailureReason.Name = "checkIgnoreFailureReason";
            this.checkIgnoreFailureReason.Size = new System.Drawing.Size(208, 17);
            this.checkIgnoreFailureReason.TabIndex = 25;
            this.checkIgnoreFailureReason.Text = "Ignore \'failure reason\' tracker response";
            this.checkIgnoreFailureReason.UseVisualStyleBackColor = true;
            this.checkIgnoreFailureReason.CheckedChanged += new System.EventHandler(this.checkIgnoreFailureReason_CheckedChanged);
            // 
            // textStopMinLeecher
            // 
            this.textStopMinLeecher.Enabled = false;
            this.textStopMinLeecher.Location = new System.Drawing.Point(283, 87);
            this.textStopMinLeecher.Name = "textStopMinLeecher";
            this.textStopMinLeecher.Size = new System.Drawing.Size(39, 20);
            this.textStopMinLeecher.TabIndex = 24;
            this.textStopMinLeecher.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textStopMinLeecher.ValueChanged += new System.EventHandler(this.textStopMinLeecher_ValueChanged);
            // 
            // labelStopMinLeecher
            // 
            this.labelStopMinLeecher.AutoSize = true;
            this.labelStopMinLeecher.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelStopMinLeecher.Location = new System.Drawing.Point(19, 89);
            this.labelStopMinLeecher.Name = "labelStopMinLeecher";
            this.labelStopMinLeecher.Size = new System.Drawing.Size(258, 13);
            this.labelStopMinLeecher.TabIndex = 22;
            this.labelStopMinLeecher.Text = "Stop uploading when number of leechers is less than:";
            this.labelStopMinLeecher.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interval
            // 
            this.interval.Location = new System.Drawing.Point(160, 15);
            this.interval.Name = "interval";
            this.interval.Size = new System.Drawing.Size(38, 20);
            this.interval.TabIndex = 21;
            this.interval.Text = "1800";
            this.interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // intervalLabel
            // 
            this.intervalLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intervalLabel.Location = new System.Drawing.Point(5, 13);
            this.intervalLabel.Name = "intervalLabel";
            this.intervalLabel.Size = new System.Drawing.Size(149, 23);
            this.intervalLabel.TabIndex = 19;
            this.intervalLabel.Text = "Tracker Update Interval (s):";
            this.intervalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkRequestScrap
            // 
            this.checkRequestScrap.AutoSize = true;
            this.checkRequestScrap.Checked = true;
            this.checkRequestScrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRequestScrap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkRequestScrap.Location = new System.Drawing.Point(8, 64);
            this.checkRequestScrap.Name = "checkRequestScrap";
            this.checkRequestScrap.Size = new System.Drawing.Size(279, 17);
            this.checkRequestScrap.TabIndex = 9;
            this.checkRequestScrap.Text = "Get Seeders/Leechers stats (scrape info from tracker)";
            this.checkRequestScrap.UseVisualStyleBackColor = true;
            this.checkRequestScrap.CheckedChanged += new System.EventHandler(this.checkRequestScrap_CheckedChanged);
            // 
            // checkTCPListen
            // 
            this.checkTCPListen.AutoSize = true;
            this.checkTCPListen.Checked = true;
            this.checkTCPListen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkTCPListen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkTCPListen.Location = new System.Drawing.Point(8, 41);
            this.checkTCPListen.Name = "checkTCPListen";
            this.checkTCPListen.Size = new System.Drawing.Size(260, 17);
            this.checkTCPListen.TabIndex = 0;
            this.checkTCPListen.Text = "Use TCP listener (appear connectable on tracker)";
            this.checkTCPListen.UseVisualStyleBackColor = true;
            this.checkTCPListen.CheckedChanged += new System.EventHandler(this.checkTCPListen_CheckedChanged);
            // 
            // proxySettingsGroup
            // 
            this.proxySettingsGroup.Controls.Add(this.linkProxyHelp);
            this.proxySettingsGroup.Controls.Add(this.textProxyPass);
            this.proxySettingsGroup.Controls.Add(this.textProxyUser);
            this.proxySettingsGroup.Controls.Add(this.labelProxyPass);
            this.proxySettingsGroup.Controls.Add(this.labelProxyUser);
            this.proxySettingsGroup.Controls.Add(this.textProxyPort);
            this.proxySettingsGroup.Controls.Add(this.textProxyHost);
            this.proxySettingsGroup.Controls.Add(this.labelProxyPort);
            this.proxySettingsGroup.Controls.Add(this.labelProxyHost);
            this.proxySettingsGroup.Controls.Add(this.comboProxyType);
            this.proxySettingsGroup.Controls.Add(this.labelProxyType);
            this.proxySettingsGroup.ForeColor = System.Drawing.SystemColors.Desktop;
            this.proxySettingsGroup.Location = new System.Drawing.Point(6, 15);
            this.proxySettingsGroup.Name = "proxySettingsGroup";
            this.proxySettingsGroup.Size = new System.Drawing.Size(558, 96);
            this.proxySettingsGroup.TabIndex = 2;
            this.proxySettingsGroup.TabStop = false;
            this.proxySettingsGroup.Text = "Proxy Server Settings";
            // 
            // linkProxyHelp
            // 
            this.linkProxyHelp.AutoSize = true;
            this.linkProxyHelp.Location = new System.Drawing.Point(44, 69);
            this.linkProxyHelp.Name = "linkProxyHelp";
            this.linkProxyHelp.Size = new System.Drawing.Size(87, 13);
            this.linkProxyHelp.TabIndex = 11;
            this.linkProxyHelp.TabStop = true;
            this.linkProxyHelp.Text = "Help with proxies";
            this.linkProxyHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkProxyHelp_LinkClicked);
            // 
            // textProxyPass
            // 
            this.textProxyPass.Location = new System.Drawing.Point(417, 62);
            this.textProxyPass.Name = "textProxyPass";
            this.textProxyPass.Size = new System.Drawing.Size(90, 20);
            this.textProxyPass.TabIndex = 10;
            // 
            // textProxyUser
            // 
            this.textProxyUser.Location = new System.Drawing.Point(221, 62);
            this.textProxyUser.Name = "textProxyUser";
            this.textProxyUser.Size = new System.Drawing.Size(90, 20);
            this.textProxyUser.TabIndex = 9;
            // 
            // labelProxyPass
            // 
            this.labelProxyPass.AutoSize = true;
            this.labelProxyPass.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProxyPass.Location = new System.Drawing.Point(354, 65);
            this.labelProxyPass.Name = "labelProxyPass";
            this.labelProxyPass.Size = new System.Drawing.Size(56, 13);
            this.labelProxyPass.TabIndex = 8;
            this.labelProxyPass.Text = "Password:";
            // 
            // labelProxyUser
            // 
            this.labelProxyUser.AutoSize = true;
            this.labelProxyUser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProxyUser.Location = new System.Drawing.Point(187, 65);
            this.labelProxyUser.Name = "labelProxyUser";
            this.labelProxyUser.Size = new System.Drawing.Size(32, 13);
            this.labelProxyUser.TabIndex = 7;
            this.labelProxyUser.Text = "User:";
            // 
            // textProxyPort
            // 
            this.textProxyPort.Location = new System.Drawing.Point(461, 26);
            this.textProxyPort.Name = "textProxyPort";
            this.textProxyPort.Size = new System.Drawing.Size(45, 20);
            this.textProxyPort.TabIndex = 5;
            // 
            // textProxyHost
            // 
            this.textProxyHost.Location = new System.Drawing.Point(221, 26);
            this.textProxyHost.Name = "textProxyHost";
            this.textProxyHost.Size = new System.Drawing.Size(144, 20);
            this.textProxyHost.TabIndex = 4;
            this.textProxyHost.TextChanged += new System.EventHandler(this.textProxyHost_TextChanged);
            // 
            // labelProxyPort
            // 
            this.labelProxyPort.AutoSize = true;
            this.labelProxyPort.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProxyPort.Location = new System.Drawing.Point(414, 29);
            this.labelProxyPort.Name = "labelProxyPort";
            this.labelProxyPort.Size = new System.Drawing.Size(29, 13);
            this.labelProxyPort.TabIndex = 3;
            this.labelProxyPort.Text = "Port:";
            // 
            // labelProxyHost
            // 
            this.labelProxyHost.AutoSize = true;
            this.labelProxyHost.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProxyHost.Location = new System.Drawing.Point(187, 28);
            this.labelProxyHost.Name = "labelProxyHost";
            this.labelProxyHost.Size = new System.Drawing.Size(32, 13);
            this.labelProxyHost.TabIndex = 2;
            this.labelProxyHost.Text = "Host:";
            // 
            // comboProxyType
            // 
            this.comboProxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProxyType.FormattingEnabled = true;
            this.comboProxyType.Items.AddRange(new object[] {
            "None",
            "HTTP",
            "SOCKS4",
            "SOCKS4a",
            "SOCKS5"});
            this.comboProxyType.Location = new System.Drawing.Point(75, 25);
            this.comboProxyType.Name = "comboProxyType";
            this.comboProxyType.Size = new System.Drawing.Size(88, 21);
            this.comboProxyType.TabIndex = 1;
            // 
            // labelProxyType
            // 
            this.labelProxyType.AutoSize = true;
            this.labelProxyType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProxyType.Location = new System.Drawing.Point(6, 28);
            this.labelProxyType.Name = "labelProxyType";
            this.labelProxyType.Size = new System.Drawing.Size(63, 13);
            this.labelProxyType.TabIndex = 0;
            this.labelProxyType.Text = "Proxy Type:";
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.labDefultSearchString);
            this.tabAdvanced.Controls.Add(this.txtCustomSearchString);
            this.tabAdvanced.Controls.Add(this.checkBoxCustomSearchString);
            this.tabAdvanced.Controls.Add(this.labUCSstatus);
            this.tabAdvanced.Controls.Add(this.chkUnifiedClientSimu);
            this.tabAdvanced.Controls.Add(this.checkAutoMemReader);
            this.tabAdvanced.Controls.Add(this.progressBar1);
            this.tabAdvanced.Controls.Add(this.labAutoMem);
            this.tabAdvanced.Controls.Add(this.updateAnnounceParamsOnStart);
            this.tabAdvanced.Controls.Add(this.TorrentClientsBox);
            this.tabAdvanced.Controls.Add(this.ClientLabel);
            this.tabAdvanced.Controls.Add(this.randomSpeedGroup);
            this.tabAdvanced.Controls.Add(this.reportParamsGroup);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Size = new System.Drawing.Size(570, 348);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = "Advanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // txtCustomSearchString
            // 
            this.txtCustomSearchString.Enabled = false;
            this.txtCustomSearchString.Location = new System.Drawing.Point(225, 90);
            this.txtCustomSearchString.Name = "txtCustomSearchString";
            this.txtCustomSearchString.Size = new System.Drawing.Size(150, 20);
            this.txtCustomSearchString.TabIndex = 35;
            // 
            // checkBoxCustomSearchString
            // 
            this.checkBoxCustomSearchString.AutoSize = true;
            this.checkBoxCustomSearchString.Location = new System.Drawing.Point(25, 92);
            this.checkBoxCustomSearchString.Name = "checkBoxCustomSearchString";
            this.checkBoxCustomSearchString.Size = new System.Drawing.Size(128, 17);
            this.checkBoxCustomSearchString.TabIndex = 34;
            this.checkBoxCustomSearchString.Text = "Custom Search String";
            this.checkBoxCustomSearchString.UseVisualStyleBackColor = true;
            this.checkBoxCustomSearchString.CheckedChanged += new System.EventHandler(this.checkBoxCustomSearchString_CheckedChanged);
            // 
            // labUCSstatus
            // 
            this.labUCSstatus.AutoSize = true;
            this.labUCSstatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labUCSstatus.Location = new System.Drawing.Point(222, 118);
            this.labUCSstatus.Name = "labUCSstatus";
            this.labUCSstatus.Size = new System.Drawing.Size(171, 13);
            this.labUCSstatus.TabIndex = 33;
            this.labUCSstatus.Text = "Unified Clent Simulation Not Active";
            // 
            // chkUnifiedClientSimu
            // 
            this.chkUnifiedClientSimu.AutoSize = true;
            this.chkUnifiedClientSimu.Checked = true;
            this.chkUnifiedClientSimu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUnifiedClientSimu.Location = new System.Drawing.Point(24, 114);
            this.chkUnifiedClientSimu.Name = "chkUnifiedClientSimu";
            this.chkUnifiedClientSimu.Size = new System.Drawing.Size(178, 17);
            this.chkUnifiedClientSimu.TabIndex = 32;
            this.chkUnifiedClientSimu.Text = "Perform Unified Client Simulation";
            this.chkUnifiedClientSimu.UseVisualStyleBackColor = true;
            this.chkUnifiedClientSimu.CheckedChanged += new System.EventHandler(this.chkUnifiedClientSimu_CheckedChanged);
            // 
            // checkAutoMemReader
            // 
            this.checkAutoMemReader.AutoSize = true;
            this.checkAutoMemReader.Checked = true;
            this.checkAutoMemReader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAutoMemReader.Location = new System.Drawing.Point(24, 69);
            this.checkAutoMemReader.Name = "checkAutoMemReader";
            this.checkAutoMemReader.Size = new System.Drawing.Size(151, 17);
            this.checkAutoMemReader.TabIndex = 31;
            this.checkAutoMemReader.Text = "Automatic Memory Reader";
            this.checkAutoMemReader.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(225, 51);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(335, 11);
            this.progressBar1.TabIndex = 30;
            this.progressBar1.Visible = false;
            // 
            // labAutoMem
            // 
            this.labAutoMem.AutoSize = true;
            this.labAutoMem.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labAutoMem.Location = new System.Drawing.Point(222, 73);
            this.labAutoMem.Name = "labAutoMem";
            this.labAutoMem.Size = new System.Drawing.Size(199, 13);
            this.labAutoMem.TabIndex = 29;
            this.labAutoMem.Text = "Automatic Memory Reader Unsuccessful";
            // 
            // updateAnnounceParamsOnStart
            // 
            this.updateAnnounceParamsOnStart.AutoSize = true;
            this.updateAnnounceParamsOnStart.Checked = true;
            this.updateAnnounceParamsOnStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.updateAnnounceParamsOnStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateAnnounceParamsOnStart.Location = new System.Drawing.Point(25, 45);
            this.updateAnnounceParamsOnStart.Name = "updateAnnounceParamsOnStart";
            this.updateAnnounceParamsOnStart.Size = new System.Drawing.Size(190, 17);
            this.updateAnnounceParamsOnStart.TabIndex = 27;
            this.updateAnnounceParamsOnStart.Text = "Update peer_id and key on startup";
            this.updateAnnounceParamsOnStart.UseVisualStyleBackColor = true;
            // 
            // TorrentClientsBox
            // 
            this.TorrentClientsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TorrentClientsBox.FormattingEnabled = true;
            this.TorrentClientsBox.Location = new System.Drawing.Point(123, 15);
            this.TorrentClientsBox.Name = "TorrentClientsBox";
            this.TorrentClientsBox.Size = new System.Drawing.Size(437, 21);
            this.TorrentClientsBox.TabIndex = 24;
            this.TorrentClientsBox.SelectedIndexChanged += new System.EventHandler(this.TorrentClientsBox_SelectedIndexChanged);
            // 
            // ClientLabel
            // 
            this.ClientLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ClientLabel.Location = new System.Drawing.Point(12, 15);
            this.ClientLabel.Name = "ClientLabel";
            this.ClientLabel.Size = new System.Drawing.Size(105, 18);
            this.ClientLabel.TabIndex = 23;
            this.ClientLabel.Text = "Client Simulation:";
            this.ClientLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // randomSpeedGroup
            // 
            this.randomSpeedGroup.Controls.Add(this.RandomDownloadTo);
            this.randomSpeedGroup.Controls.Add(this.RandomUploadTo);
            this.randomSpeedGroup.Controls.Add(this.RandomDownloadFrom);
            this.randomSpeedGroup.Controls.Add(this.RandomUploadFrom);
            this.randomSpeedGroup.Controls.Add(this.RandomDownloadToLabel);
            this.randomSpeedGroup.Controls.Add(this.RandomUploadToLabel);
            this.randomSpeedGroup.Controls.Add(this.RandomDownloadFromLabel);
            this.randomSpeedGroup.Controls.Add(this.RandomUploadFromLabel);
            this.randomSpeedGroup.Controls.Add(this.checkRandomDownload);
            this.randomSpeedGroup.Controls.Add(this.checkRandomUpload);
            this.randomSpeedGroup.ForeColor = System.Drawing.SystemColors.Desktop;
            this.randomSpeedGroup.Location = new System.Drawing.Point(15, 262);
            this.randomSpeedGroup.Name = "randomSpeedGroup";
            this.randomSpeedGroup.Size = new System.Drawing.Size(543, 74);
            this.randomSpeedGroup.TabIndex = 12;
            this.randomSpeedGroup.TabStop = false;
            this.randomSpeedGroup.Text = "Randomise Upload/Download speeds";
            // 
            // RandomDownloadTo
            // 
            this.RandomDownloadTo.Location = new System.Drawing.Point(344, 40);
            this.RandomDownloadTo.Name = "RandomDownloadTo";
            this.RandomDownloadTo.Size = new System.Drawing.Size(56, 20);
            this.RandomDownloadTo.TabIndex = 9;
            this.RandomDownloadTo.Text = "100";
            this.RandomDownloadTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.RandomDownloadTo.TextChanged += new System.EventHandler(this.RandomDownloadTo_TextChanged);
            // 
            // RandomUploadTo
            // 
            this.RandomUploadTo.Location = new System.Drawing.Point(344, 17);
            this.RandomUploadTo.Name = "RandomUploadTo";
            this.RandomUploadTo.Size = new System.Drawing.Size(56, 20);
            this.RandomUploadTo.TabIndex = 8;
            this.RandomUploadTo.Text = "100";
            this.RandomUploadTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.RandomUploadTo.TextChanged += new System.EventHandler(this.RandomUploadTo_TextChanged);
            // 
            // RandomDownloadFrom
            // 
            this.RandomDownloadFrom.Location = new System.Drawing.Point(210, 39);
            this.RandomDownloadFrom.Name = "RandomDownloadFrom";
            this.RandomDownloadFrom.Size = new System.Drawing.Size(58, 20);
            this.RandomDownloadFrom.TabIndex = 7;
            this.RandomDownloadFrom.Text = "0";
            this.RandomDownloadFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.RandomDownloadFrom.TextChanged += new System.EventHandler(this.RandomDownloadFrom_TextChanged);
            // 
            // RandomUploadFrom
            // 
            this.RandomUploadFrom.Location = new System.Drawing.Point(210, 17);
            this.RandomUploadFrom.Name = "RandomUploadFrom";
            this.RandomUploadFrom.Size = new System.Drawing.Size(58, 20);
            this.RandomUploadFrom.TabIndex = 6;
            this.RandomUploadFrom.Text = "6.8";
            this.RandomUploadFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.RandomUploadFrom.TextChanged += new System.EventHandler(this.RandomUploadFrom_TextChanged);
            // 
            // RandomDownloadToLabel
            // 
            this.RandomDownloadToLabel.AutoSize = true;
            this.RandomDownloadToLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RandomDownloadToLabel.Location = new System.Drawing.Point(308, 43);
            this.RandomDownloadToLabel.Name = "RandomDownloadToLabel";
            this.RandomDownloadToLabel.Size = new System.Drawing.Size(30, 13);
            this.RandomDownloadToLabel.TabIndex = 5;
            this.RandomDownloadToLabel.Text = "Max:";
            // 
            // RandomUploadToLabel
            // 
            this.RandomUploadToLabel.AutoSize = true;
            this.RandomUploadToLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RandomUploadToLabel.Location = new System.Drawing.Point(308, 20);
            this.RandomUploadToLabel.Name = "RandomUploadToLabel";
            this.RandomUploadToLabel.Size = new System.Drawing.Size(30, 13);
            this.RandomUploadToLabel.TabIndex = 4;
            this.RandomUploadToLabel.Text = "Max:";
            // 
            // RandomDownloadFromLabel
            // 
            this.RandomDownloadFromLabel.AutoSize = true;
            this.RandomDownloadFromLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RandomDownloadFromLabel.Location = new System.Drawing.Point(177, 43);
            this.RandomDownloadFromLabel.Name = "RandomDownloadFromLabel";
            this.RandomDownloadFromLabel.Size = new System.Drawing.Size(27, 13);
            this.RandomDownloadFromLabel.TabIndex = 3;
            this.RandomDownloadFromLabel.Text = "Min:";
            // 
            // RandomUploadFromLabel
            // 
            this.RandomUploadFromLabel.AutoSize = true;
            this.RandomUploadFromLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RandomUploadFromLabel.Location = new System.Drawing.Point(177, 20);
            this.RandomUploadFromLabel.Name = "RandomUploadFromLabel";
            this.RandomUploadFromLabel.Size = new System.Drawing.Size(27, 13);
            this.RandomUploadFromLabel.TabIndex = 2;
            this.RandomUploadFromLabel.Text = "Min:";
            // 
            // checkRandomDownload
            // 
            this.checkRandomDownload.AutoSize = true;
            this.checkRandomDownload.Checked = true;
            this.checkRandomDownload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRandomDownload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkRandomDownload.Location = new System.Drawing.Point(20, 42);
            this.checkRandomDownload.Name = "checkRandomDownload";
            this.checkRandomDownload.Size = new System.Drawing.Size(96, 17);
            this.checkRandomDownload.TabIndex = 1;
            this.checkRandomDownload.Text = "Download (kB)";
            this.checkRandomDownload.UseVisualStyleBackColor = true;
            this.checkRandomDownload.CheckedChanged += new System.EventHandler(this.checkRandomDownload_CheckedChanged);
            // 
            // checkRandomUpload
            // 
            this.checkRandomUpload.AutoSize = true;
            this.checkRandomUpload.Checked = true;
            this.checkRandomUpload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRandomUpload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkRandomUpload.Location = new System.Drawing.Point(20, 19);
            this.checkRandomUpload.Name = "checkRandomUpload";
            this.checkRandomUpload.Size = new System.Drawing.Size(85, 17);
            this.checkRandomUpload.TabIndex = 0;
            this.checkRandomUpload.Text = "Upload  (kB)";
            this.checkRandomUpload.UseVisualStyleBackColor = true;
            this.checkRandomUpload.CheckedChanged += new System.EventHandler(this.checkRandomUpload_CheckedChanged);
            // 
            // reportParamsGroup
            // 
            this.reportParamsGroup.Controls.Add(this.customKey);
            this.reportParamsGroup.Controls.Add(this.customPeerID);
            this.reportParamsGroup.Controls.Add(this.keyLabel);
            this.reportParamsGroup.Controls.Add(this.customPeersNum);
            this.reportParamsGroup.Controls.Add(this.label4);
            this.reportParamsGroup.Controls.Add(this.numwantLabel);
            this.reportParamsGroup.Controls.Add(this.portInfoLabel);
            this.reportParamsGroup.Controls.Add(this.portLabel);
            this.reportParamsGroup.Controls.Add(this.customPort);
            this.reportParamsGroup.ForeColor = System.Drawing.SystemColors.Desktop;
            this.reportParamsGroup.Location = new System.Drawing.Point(15, 134);
            this.reportParamsGroup.Name = "reportParamsGroup";
            this.reportParamsGroup.Size = new System.Drawing.Size(545, 122);
            this.reportParamsGroup.TabIndex = 1;
            this.reportParamsGroup.TabStop = false;
            this.reportParamsGroup.Text = "Announce Parameters";
            // 
            // customKey
            // 
            this.customKey.Location = new System.Drawing.Point(111, 65);
            this.customKey.Name = "customKey";
            this.customKey.Size = new System.Drawing.Size(226, 20);
            this.customKey.TabIndex = 4;
            // 
            // customPeerID
            // 
            this.customPeerID.Location = new System.Drawing.Point(111, 39);
            this.customPeerID.Name = "customPeerID";
            this.customPeerID.Size = new System.Drawing.Size(383, 20);
            this.customPeerID.TabIndex = 10;
            // 
            // keyLabel
            // 
            this.keyLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.keyLabel.Location = new System.Drawing.Point(15, 68);
            this.keyLabel.Name = "keyLabel";
            this.keyLabel.Size = new System.Drawing.Size(90, 13);
            this.keyLabel.TabIndex = 3;
            this.keyLabel.Text = "Key (key):";
            this.keyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPeersNum
            // 
            this.customPeersNum.Location = new System.Drawing.Point(346, 91);
            this.customPeersNum.Name = "customPeersNum";
            this.customPeersNum.Size = new System.Drawing.Size(46, 20);
            this.customPeersNum.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(9, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Peer ID (peer_id):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numwantLabel
            // 
            this.numwantLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.numwantLabel.Location = new System.Drawing.Point(187, 94);
            this.numwantLabel.Name = "numwantLabel";
            this.numwantLabel.Size = new System.Drawing.Size(153, 13);
            this.numwantLabel.TabIndex = 6;
            this.numwantLabel.Text = "Number of Peers (numwant):";
            this.numwantLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // portInfoLabel
            // 
            this.portInfoLabel.AutoSize = true;
            this.portInfoLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.portInfoLabel.Location = new System.Drawing.Point(6, 16);
            this.portInfoLabel.Name = "portInfoLabel";
            this.portInfoLabel.Size = new System.Drawing.Size(354, 13);
            this.portInfoLabel.TabIndex = 2;
            this.portInfoLabel.Text = "If those fields are left empty, " + Program.APPLICATION_NAME + "will use random or default values";
            // 
            // portLabel
            // 
            this.portLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.portLabel.Location = new System.Drawing.Point(12, 92);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(93, 17);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "Port (port):";
            this.portLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customPort
            // 
            this.customPort.Location = new System.Drawing.Point(111, 91);
            this.customPort.Name = "customPort";
            this.customPort.Size = new System.Drawing.Size(44, 20);
            this.customPort.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.groupTorrentFile);
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.groupTorrentInfo);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(570, 348);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // groupTorrentFile
            // 
            this.groupTorrentFile.Controls.Add(this.torrentFile);
            this.groupTorrentFile.Controls.Add(this.TorrentFileLabel);
            this.groupTorrentFile.Controls.Add(this.browseButton);
            this.groupTorrentFile.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupTorrentFile.Location = new System.Drawing.Point(12, 6);
            this.groupTorrentFile.Name = "groupTorrentFile";
            this.groupTorrentFile.Size = new System.Drawing.Size(544, 49);
            this.groupTorrentFile.TabIndex = 20;
            this.groupTorrentFile.TabStop = false;
            this.groupTorrentFile.Text = "Torrent File";
            // 
            // torrentFile
            // 
            this.torrentFile.Location = new System.Drawing.Point(54, 19);
            this.torrentFile.Name = "torrentFile";
            this.torrentFile.Size = new System.Drawing.Size(400, 20);
            this.torrentFile.TabIndex = 15;
            // 
            // TorrentFileLabel
            // 
            this.TorrentFileLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TorrentFileLabel.Location = new System.Drawing.Point(6, 17);
            this.TorrentFileLabel.Name = "TorrentFileLabel";
            this.TorrentFileLabel.Size = new System.Drawing.Size(42, 23);
            this.TorrentFileLabel.TabIndex = 14;
            this.TorrentFileLabel.Text = "Path:";
            this.TorrentFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // browseButton
            // 
            this.browseButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.browseButton.Location = new System.Drawing.Point(460, 17);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(78, 23);
            this.browseButton.TabIndex = 16;
            this.browseButton.Text = "Browse...";
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labDownkbits);
            this.groupBox2.Controls.Add(this.labUpkbits);
            this.groupBox2.Controls.Add(this.applyStopSettingsButton);
            this.groupBox2.Controls.Add(this.stopProcessUnitsBox);
            this.groupBox2.Controls.Add(this.stopProcessActionBox);
            this.groupBox2.Controls.Add(this.stopProcessValue);
            this.groupBox2.Controls.Add(this.stopProcessLabel);
            this.groupBox2.Controls.Add(this.fileSize);
            this.groupBox2.Controls.Add(this.FileSizeLabel);
            this.groupBox2.Controls.Add(this.uploadRate);
            this.groupBox2.Controls.Add(this.downloadRate);
            this.groupBox2.Controls.Add(this.uploadRateLabel);
            this.groupBox2.Controls.Add(this.downloadRateLabel);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupBox2.Location = new System.Drawing.Point(12, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 115);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // labDownkbits
            // 
            this.labDownkbits.AutoSize = true;
            this.labDownkbits.Location = new System.Drawing.Point(231, 50);
            this.labDownkbits.Name = "labDownkbits";
            this.labDownkbits.Size = new System.Drawing.Size(49, 13);
            this.labDownkbits.TabIndex = 26;
            this.labDownkbits.Text = "10 kbit/s";
            // 
            // labUpkbits
            // 
            this.labUpkbits.AutoSize = true;
            this.labUpkbits.Location = new System.Drawing.Point(232, 24);
            this.labUpkbits.Name = "labUpkbits";
            this.labUpkbits.Size = new System.Drawing.Size(49, 13);
            this.labUpkbits.TabIndex = 25;
            this.labUpkbits.Text = "50 kbit/s";
            // 
            // applyStopSettingsButton
            // 
            this.applyStopSettingsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.applyStopSettingsButton.Location = new System.Drawing.Point(460, 74);
            this.applyStopSettingsButton.Name = "applyStopSettingsButton";
            this.applyStopSettingsButton.Size = new System.Drawing.Size(67, 23);
            this.applyStopSettingsButton.TabIndex = 24;
            this.applyStopSettingsButton.Text = "Apply";
            this.applyStopSettingsButton.Visible = false;
            this.applyStopSettingsButton.Click += new System.EventHandler(this.applyStopSettingsButton_Click);
            // 
            // stopProcessUnitsBox
            // 
            this.stopProcessUnitsBox.FormattingEnabled = true;
            this.stopProcessUnitsBox.Location = new System.Drawing.Point(358, 75);
            this.stopProcessUnitsBox.Name = "stopProcessUnitsBox";
            this.stopProcessUnitsBox.Size = new System.Drawing.Size(79, 21);
            this.stopProcessUnitsBox.TabIndex = 23;
            this.stopProcessUnitsBox.Visible = false;
            this.stopProcessUnitsBox.SelectedIndexChanged += new System.EventHandler(this.stopProcessUnitsBox_SelectedIndexChanged);
            // 
            // stopProcessActionBox
            // 
            this.stopProcessActionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stopProcessActionBox.FormattingEnabled = true;
            this.stopProcessActionBox.Items.AddRange(new object[] {
            "Do not stop",
            "Uploaded",
            "Downloaded",
            "Time"});
            this.stopProcessActionBox.Location = new System.Drawing.Point(145, 75);
            this.stopProcessActionBox.Name = "stopProcessActionBox";
            this.stopProcessActionBox.Size = new System.Drawing.Size(121, 21);
            this.stopProcessActionBox.TabIndex = 22;
            this.stopProcessActionBox.SelectedIndexChanged += new System.EventHandler(this.stopProcessActionBox_SelectedIndexChanged);
            this.stopProcessActionBox.DropDown += new System.EventHandler(this.stopProcessActionBox_DropDown);
            // 
            // stopProcessValue
            // 
            this.stopProcessValue.Location = new System.Drawing.Point(272, 76);
            this.stopProcessValue.Name = "stopProcessValue";
            this.stopProcessValue.Size = new System.Drawing.Size(80, 20);
            this.stopProcessValue.TabIndex = 21;
            this.stopProcessValue.Text = "1000";
            this.stopProcessValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.stopProcessValue.Visible = false;
            this.stopProcessValue.TextChanged += new System.EventHandler(this.stopProcessValue_TextChanged);
            // 
            // stopProcessLabel
            // 
            this.stopProcessLabel.AutoSize = true;
            this.stopProcessLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.stopProcessLabel.Location = new System.Drawing.Point(44, 78);
            this.stopProcessLabel.Name = "stopProcessLabel";
            this.stopProcessLabel.Size = new System.Drawing.Size(96, 13);
            this.stopProcessLabel.TabIndex = 20;
            this.stopProcessLabel.Text = "Stop process after:";
            this.stopProcessLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fileSize
            // 
            this.fileSize.Location = new System.Drawing.Point(392, 21);
            this.fileSize.Name = "fileSize";
            this.fileSize.Size = new System.Drawing.Size(62, 20);
            this.fileSize.TabIndex = 19;
            this.fileSize.Text = "0";
            this.fileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FileSizeLabel
            // 
            this.FileSizeLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FileSizeLabel.Location = new System.Drawing.Point(250, 19);
            this.FileSizeLabel.Name = "FileSizeLabel";
            this.FileSizeLabel.Size = new System.Drawing.Size(136, 23);
            this.FileSizeLabel.TabIndex = 17;
            this.FileSizeLabel.Text = "Finished (%)";
            this.FileSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uploadRate
            // 
            this.uploadRate.Location = new System.Drawing.Point(146, 19);
            this.uploadRate.Name = "uploadRate";
            this.uploadRate.Size = new System.Drawing.Size(80, 20);
            this.uploadRate.TabIndex = 13;
            this.uploadRate.Text = "50";
            this.uploadRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.uploadRate.TextChanged += new System.EventHandler(this.uploadRate_TextChanged);
            // 
            // downloadRate
            // 
            this.downloadRate.Location = new System.Drawing.Point(146, 45);
            this.downloadRate.Name = "downloadRate";
            this.downloadRate.Size = new System.Drawing.Size(80, 20);
            this.downloadRate.TabIndex = 12;
            this.downloadRate.Text = "10";
            this.downloadRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.downloadRate.TextChanged += new System.EventHandler(this.downloadRate_TextChanged);
            // 
            // uploadRateLabel
            // 
            this.uploadRateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadRateLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uploadRateLabel.Location = new System.Drawing.Point(16, 19);
            this.uploadRateLabel.Name = "uploadRateLabel";
            this.uploadRateLabel.Size = new System.Drawing.Size(124, 23);
            this.uploadRateLabel.TabIndex = 11;
            this.uploadRateLabel.Text = "Upload Speed (kB/s) :";
            this.uploadRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // downloadRateLabel
            // 
            this.downloadRateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadRateLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.downloadRateLabel.Location = new System.Drawing.Point(6, 45);
            this.downloadRateLabel.Name = "downloadRateLabel";
            this.downloadRateLabel.Size = new System.Drawing.Size(134, 23);
            this.downloadRateLabel.TabIndex = 10;
            this.downloadRateLabel.Text = "Download Speed (kB/s) :";
            this.downloadRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupTorrentInfo
            // 
            this.groupTorrentInfo.Controls.Add(this.torrentSize);
            this.groupTorrentInfo.Controls.Add(this.labelTorrentSize);
            this.groupTorrentInfo.Controls.Add(this.shaHash);
            this.groupTorrentInfo.Controls.Add(this.hashLabel);
            this.groupTorrentInfo.Controls.Add(this.trackerAddress);
            this.groupTorrentInfo.Controls.Add(this.TrackerLabel);
            this.groupTorrentInfo.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupTorrentInfo.Location = new System.Drawing.Point(12, 61);
            this.groupTorrentInfo.Name = "groupTorrentInfo";
            this.groupTorrentInfo.Size = new System.Drawing.Size(544, 76);
            this.groupTorrentInfo.TabIndex = 17;
            this.groupTorrentInfo.TabStop = false;
            this.groupTorrentInfo.Text = "Torrent Information";
            // 
            // torrentSize
            // 
            this.torrentSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.torrentSize.Location = new System.Drawing.Point(457, 42);
            this.torrentSize.Name = "torrentSize";
            this.torrentSize.Size = new System.Drawing.Size(78, 19);
            this.torrentSize.TabIndex = 7;
            this.torrentSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTorrentSize
            // 
            this.labelTorrentSize.AutoSize = true;
            this.labelTorrentSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelTorrentSize.Location = new System.Drawing.Point(424, 45);
            this.labelTorrentSize.Name = "labelTorrentSize";
            this.labelTorrentSize.Size = new System.Drawing.Size(30, 13);
            this.labelTorrentSize.TabIndex = 6;
            this.labelTorrentSize.Text = "Size:";
            // 
            // shaHash
            // 
            this.shaHash.Location = new System.Drawing.Point(95, 42);
            this.shaHash.Name = "shaHash";
            this.shaHash.Size = new System.Drawing.Size(314, 20);
            this.shaHash.TabIndex = 5;
            // 
            // hashLabel
            // 
            this.hashLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.hashLabel.Location = new System.Drawing.Point(9, 39);
            this.hashLabel.Name = "hashLabel";
            this.hashLabel.Size = new System.Drawing.Size(69, 23);
            this.hashLabel.TabIndex = 4;
            this.hashLabel.Text = "SHA Hash:";
            this.hashLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackerAddress
            // 
            this.trackerAddress.Location = new System.Drawing.Point(95, 18);
            this.trackerAddress.Name = "trackerAddress";
            this.trackerAddress.Size = new System.Drawing.Size(314, 20);
            this.trackerAddress.TabIndex = 3;
            // 
            // TrackerLabel
            // 
            this.TrackerLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TrackerLabel.Location = new System.Drawing.Point(9, 16);
            this.TrackerLabel.Name = "TrackerLabel";
            this.TrackerLabel.Size = new System.Drawing.Size(69, 23);
            this.TrackerLabel.TabIndex = 2;
            this.TrackerLabel.Text = "Tracker:";
            this.TrackerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabAdvanced);
            this.tabControl1.Controls.Add(this.tabNetwork);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(578, 374);
            this.tabControl1.TabIndex = 20;
            // 
            // labDefultSearchString
            // 
            this.labDefultSearchString.AutoSize = true;
            this.labDefultSearchString.Location = new System.Drawing.Point(381, 92);
            this.labDefultSearchString.Name = "labDefultSearchString";
            this.labDefultSearchString.Size = new System.Drawing.Size(16, 13);
            this.labDefultSearchString.TabIndex = 36;
            this.labDefultSearchString.Text = "...";
            // 
            // Form1
            // 
            this.AcceptButton = this.StartButton;
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(603, 423);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New RM";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabNetwork.ResumeLayout(false);
            this.groupNetworkMisc.ResumeLayout(false);
            this.groupNetworkMisc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textStopMinLeecher)).EndInit();
            this.proxySettingsGroup.ResumeLayout(false);
            this.proxySettingsGroup.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.randomSpeedGroup.ResumeLayout(false);
            this.randomSpeedGroup.PerformLayout();
            this.reportParamsGroup.ResumeLayout(false);
            this.reportParamsGroup.PerformLayout();
            this.tabGeneral.ResumeLayout(false);
            this.groupTorrentFile.ResumeLayout(false);
            this.groupTorrentFile.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupTorrentInfo.ResumeLayout(false);
            this.groupTorrentInfo.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       
        

        private void linkProxyHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.moofdev.org/ratiomaster/proxy-help");
        }

       
        
        //Stopped working here.... so start up from here now! :)
        //this was not transfered. this will excat the info from the torrent info and will put them in text boxes..
        //etc etc... and when the user press start we will pass this info to the RM object...
        public bool loadTorrentFileInfo(string torrentFilePath, bool loadSettings)
        {
            try
            {
                if ((torrentFilePath != null) && (torrentFilePath.Length != 0))
                {
                    currentRMIns.AddLogLine("Torrent Path=" + torrentFilePath);
                    FileInfo info = new FileInfo(torrentFilePath);
                    if (info.Exists)
                    {
                        this.currentTorrentFile = new Torrent(torrentFilePath);
                        this.torrentFile.Text = torrentFilePath;
                        this.trackerAddress.Text = this.currentTorrentFile.Announce.ToString();
                        this.shaHash.Text = ToHexString(this.currentTorrentFile.InfoHash);
                        this.torrentSize.Text = currentRMIns.FormatFileSize(this.currentTorrentFile.totalLength);
                        currentRMIns.AddLogLine("Torrent size:" + this.currentTorrentFile.totalLength.ToString());
                        if (loadSettings)
                        {
                            
                            this.applicationSettings.LoadTorrentSettings(this.shaHash.Text);
                        }
                        return true;
                    }
                    currentRMIns.AddLogLine("Torrent with this path doesnt exists.Please enter valid full path of the torrent.");
                }
                return false;
            }
            catch (Exception exception)
            {
                currentRMIns.AddLogLine(exception.ToString());
                return false;
            }
        }


        private void manualUpdateButton_Click(object sender, EventArgs e)// transfered and edited
        {
            currentRMIns.manualUpdate();
        }

        
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string[] fileNames = this.openFileDialog1.FileNames;
            this.loadTorrentFileInfo(fileNames[0], true);
        }

        
        
        private float parseValidFloat(string str, float defVal)//
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

        private int ParseValidInt(string str, int defVal)//
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

        private long parseValidInt64(string str, long defVal)//
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

        public TorrentClient getCurrentClient()
        {
            return (TorrentClient)this.TorrentClientsBox.SelectedItem;
        }


        private TorrentInfo getCurrentTorrent()
        {
            Uri uri;
            TorrentInfo info = new TorrentInfo(0L, 0L);
            try
            {
                uri = new Uri(this.trackerAddress.Text);
            }
            catch (Exception exception)
            {
                currentRMIns.AddLogLine(exception.ToString());
                return info;
            }
            info.tracker = this.trackerAddress.Text;
            info.trackerUri = uri;
            info.hash = this.shaHash.Text;
            info.uploadRate = (long)(this.parseValidFloat(this.uploadRate.Text, 50f) * 1024f);
            info.downloadRate = (long)(this.parseValidFloat(this.downloadRate.Text, 10f) * 1024f);
            info.interval = this.ParseValidInt(this.interval.Text, 0x708);
            this.interval.Text = info.interval.ToString();
            float num = this.parseValidFloat(this.fileSize.Text, 0f);
            if ((num < 0f) || (num > 100f))
            {
                currentRMIns.AddLogLine("Finished value is invalid: " + this.fileSize.Text + ",assuming 0 as default value");
                num = 0f;
            }
            if (num >= 100f)
            {
               currentRMIns.seedMode = true;
                num = 100f;
            }
            this.fileSize.Text = num.ToString();
            if (this.currentTorrentFile != null)
            {
                //----------------------------------------------
                //silentp33r edit:
                //gonna replace this switch thing with a nested if, as it seems that c# switch requres a int,
                //where a float is give here
                /*
                switch (num)
                {
                    case 0f:
                        info.totalsize = (long) this.currentTorrentFile.totalLength;
                        goto Label_01B9;

                    case 100f:
                        info.totalsize = 0L;
                        goto Label_01B9;
                }
                 */

                if (num == 0f)
                {
                    info.totalsize = (long)this.currentTorrentFile.totalLength;
                    goto Label_01B9;
                }
                else
                {
                    if (num == 100f)
                    {
                        info.totalsize = 0L;
                        goto Label_01B9;
                    }
                }
                //edit ends here
                //---------------------------------------------------

                info.totalsize = (long)((this.currentTorrentFile.totalLength * (100f - num)) / 100f);
            }
            else
            {
                info.totalsize = 0L;
            }
        Label_01B9:
            info.left = info.totalsize;
            info.filename = this.torrentFile.Text;
            info.numberOfPeers = this.getValueDefault(this.customPeersNum.Text, info.numberOfPeers);
            info.port = this.getValueDefault(this.customPort.Text, info.port);
            info.key = this.getValueDefault(this.customKey.Text, this.currentClient.Key);
            info.peerID = this.getValueDefault(this.customPeerID.Text, this.currentClient.PeerID);
            return info;
        }


        public string getValueDefault(string value, string defValue)
        {
            if (value == "")
            {
                return defValue;
            }
            return value;
        }

        private bool inputValidation(bool DoShoutAtUser)
        {
            //check user given inputs so that they don't cause havok! :)

            //check randomization values
            
            //first convert them from string to int

            /////////////////////////////
            //double numbers don' go with int so get it to double and convert in to int as rounded numbers
            double temp;
            double.TryParse(RandomUploadFrom.Text, out temp);//got it as a double value

            int upFrom, upTo, downFrom, downTo;//declare the variables

            upFrom = Convert.ToInt32(temp);

            double.TryParse(RandomUploadTo.Text, out temp);//got it as a double value
            upTo = Convert.ToInt32(temp);

            double.TryParse(this.RandomDownloadFrom.Text, out temp);//got it as a double value
            downFrom = Convert.ToInt32(temp);

            double.TryParse(this.RandomDownloadTo.Text, out temp);//got it as a double value
            downTo = Convert.ToInt32(temp);

            ////////////////////////////////

            //now lets compare and do the checking neeeded

            if (upFrom >= upTo)
            {
                if (DoShoutAtUser == true)
                {
                    MessageBox.Show("Please enter Randmized Up From which is lower than Rand Up To", Program.APPLICATION_NAME);
                }
                    return false;
            }

            if (downFrom >= downTo)
            {
                if (DoShoutAtUser == true)
                {
                    MessageBox.Show("Please enter Randmized Down From which is lower than Rand Down To", Program.APPLICATION_NAME);
                }
                    return false;
            }
            ///////////////////////////////////////
             
  
            //this code won't be reached if everything didn't went fine
            return true;
        }

        private void StartButton_Click(object sender, EventArgs e)//
        {
            //do some input validation
            bool inpValiResult = inputValidation(true);

            if (inpValiResult == true)//then do the stuff else skip all of it...
            {
                this.applicationSettings.SaveAppSettings();

                //this is here so that this event does occure and random uploadTo/and some more value don't stay null if the user
                //don't change it... this is somemthing strange as well :{
                RandomUploadTo_TextChanged(null, null);
                RandomDownloadFrom_TextChanged(null, null);
                RandomDownloadTo_TextChanged(null, null);


                currentRMIns.seedMode = false;

                this.currentClient = this.getCurrentClient();
                this.currentProxy = this.getCurrentProxy();
                this.currentTorrent = this.getCurrentTorrent();
                if (this.currentTorrent.trackerUri == null)
                {
                    MessageBox.Show("To start , please select a valid torrent file by clicking on 'Browse...' button or Drag & Drop it into rm2", "Start", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    //this.updateScrapStats("", "", "");
                    //this.totalRunningTimeCounter = 0;
                    //this.timerValue.Text = "updating...";
                    this.StartButton.Enabled = false;

                    //ok now this RM object is initillized so set the variable in it to indicate it
                    currentRMIns.isInitillized = true;

                    //user proof stuff goes here

                    //enable the new RM tool bar button
                    Program.mainUI.toolStripButton1.Enabled = true;

                    //enable the new RM menu item
                    Program.mainUI.newRMToolStripMenuItem.Enabled = true;

                    //this.StopButton.Enabled = true;
                    //this.manualUpdateButton.Enabled = true;
                    //this.OpenTcpListener();
                    //Thread thread = new Thread(new ThreadStart(this.startProcess));
                    //thread.Name = "startProcess() Thread";
                    //thread.Start();
                    //this.serverUpdateTimer.Start();

                    //-------------------------
                    //sp edit:
                    //enable ClientCrash button...

                    //butCrash.Enabled = true;
                    //------------------------------

                    //start the torrent in the current RM instence...
                    bool result = currentRMIns.Start(this.currentClient, this.currentProxy, this.currentTorrent, this.currentTorrentFile);

                    //add the current RM instence to the list view control in the main UI
                    Program.mainUI.addToList();

                    //reenable the listMain in clsFMain

                    if (result == false)
                    {
                        MessageBox.Show("Error with the RM.Start() :(", "New RM");
                    }
                    else
                    {
                        this.Dispose();
                    }
                }
            }
        }

        


        
        //changed this method so that it sets a variable in the RM instence...
        private void stopProcessActionBox_DropDown(object sender, EventArgs e)
        {
            //this.stopParamsUpdateInProgress = true;
            currentRMIns.stopParamsUpdateInProgress = true;
        }

        //Start from here!!!!!!! last worked at this on sep 06 2007
        //started from here, on sep 08
        private void stopProcessActionBox_SelectedIndexChanged(object sender, EventArgs e)//left
        {
            //let's expose the selected index to the RM object
            currentRMIns.intStopProcessActionBoxSelectedIndex = stopProcessActionBox.SelectedIndex;

            ////////////

            if (currentRMIns.updateProcessStarted)
            {
                currentRMIns.stopParamsUpdateInProgress = true;
                //this.stopParamsUpdateInProgress = true;
                this.applyStopSettingsButton.Visible = true;
            }
            this.adjustProcessActionGui();
        }





        private void TorrentClientsBox_SelectedIndexChanged(object sender, EventArgs e)//left
        {
            this.currentClient = this.getCurrentClient();
            if (this.currentClient != null)
            {
                this.customKey.Text = this.currentClient.Key.ToString();
                this.customPeerID.Text = this.currentClient.PeerID.ToString();
                //display the defult client search string
                this.labDefultSearchString.Text = "&peer_id=" + this.currentClient.PeerIDPrefix;

                //call the unified client simulation
                uniClientSimiulation();

            }

            //now try to get memory reader to try and read the stuff form the ram automatically! :P
            /*
            memRead memreader = new memRead(this);
            if (memreader.reSearchButton())
            {
                labAutoMem.Text = "Automatic Memory Reader Successful";
            }*/

            //Thread thread = new Thread(new ThreadStart(this.memScanThread));

            //check wether the the autoMemReader check box is checked or not and proceed
            if (checkAutoMemReader.Checked)
            {
                //check whether user wants to give a custom search string or not
                if (checkBoxCustomSearchString.Checked == true)
                {
                    //yes its custom baby!
                    memScanThread(txtCustomSearchString.Text);
                }
                else
                {
                    //no, so use the defult value from the client file 
                    memScanThread();
                }
            }
            
        }

        private void uniClientSimiulation()
        {
            //this whole "Unified Client Simulation should be done or not?
            //if (this.chkUnifiedClientSimu.Checked == true)
            //{
                //ok the user wants us to do it....
                foreach (TorrentSession rmIns in Program.rmCol)
                {
                    //check whther this is the last RM in the list, which is not started yet
                    //and the currently working on RM object
                    if (rmIns.rmIndex != Program.rmCol.Count - 1)
                    {
                        if (rmIns.currentClient.PeerIDPrefix == this.currentClient.PeerIDPrefix)//both torrents are using the same client file
                        {
                            //the assumption here is that PeerIDPrefix changes from client file to client file... mean version to version...

                            //do some coping from the relavant RM's values

                            this.currentClient.PeerID = rmIns.currentClient.PeerID;
                            this.currentClient.Key = rmIns.currentClient.Key;
                            this.customPort.Text = rmIns.currentTorrent.port;
                            this.customPeersNum.Text = rmIns.currentTorrent.numberOfPeers;

                            //now update New RM UI with these
                            this.customKey.Text = this.currentClient.Key.ToString();
                            this.customPeerID.Text = this.currentClient.PeerID.ToString();

                            //also set the status of the lab so that the user now universal client spoof is active
                            labUCSstatus.Text = "Unified Clent Simulation Active";

                        }
                        else
                        {
                            //also set the status of the lab so that the user now universal client spoof is active
                            //labUCSstatus.Text = "Unified Clent Simulation Not Active";
                        }
                    }
                    else
                    {
                        //also set the status of the lab so that the user now universal client spoof is active
                        //labUCSstatus.Text = "Unified Clent Simulation Not Active";
                    }

                }
            //}
        }

        private void memScanThread(string customSearchString)
        {
            TorrentClient tempCC = this.getCurrentClient();

            MemReader memreader = new MemReader(this, tempCC, customSearchString);
            Thread memRead = new Thread(memreader.reSearchButton);

            memRead.Name = "memScanThread() Thread";
            //running this process as a thread was done so that it won't slow down everything else
            //and it would not interfere with the UI... and flicker it or so...
            //so let's set the priority of this thread to bellow normal...
            memRead.Priority = ThreadPriority.Lowest;

            memRead.Start();

            /*
            if (memreader.reSearchButton())
            {
                labAutoMem.Text = "Automatic Memory Reader Successful";
            }*/
        }

        private void memScanThread()
        {
            TorrentClient tempCC = this.getCurrentClient();

            MemReader memreader = new MemReader(this, tempCC);
            Thread memRead = new Thread(memreader.reSearchButton);

            memRead.Name = "memScanThread() Thread";
            //running this process as a thread was done so that it won't slow down everything else
            //and it would not interfere with the UI... and flicker it or so...
            //so let's set the priority of this thread to bellow normal...
            memRead.Priority = ThreadPriority.Lowest;

            memRead.Start();
            
            /*
            if (memreader.reSearchButton())
            {
                labAutoMem.Text = "Automatic Memory Reader Successful";
            }*/
        }
         
        
        /*
         * We are not gonna have a tray icon for this Form1 or RM's GUI so... no need to have a tray icon
        private void trayIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.trayIconBaloonIsUp = false;
        }

        private void trayIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            this.trayIconBaloonIsUp = false;
        }

        private void trayIcon_BalloonTipShown(object sender, EventArgs e)
        {
            this.trayIconBaloonIsUp = true;
        }

        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                this.winRestore();
            }
        }

        private void trayIcon_MouseMove(object sender, MouseEventArgs e)
        {
            //------------------------
            //sp edit:
            //RM keeps showing this ballons when u move the mouse pointor over it... it keeps poping up! and i don't
            //like this. now i want the ballon tips to appear but not very quikly... so let's give it a timer and
            //make sure that it doesn't appear too often.....
            
            //this is the original code
            
            if (this.checkShowTrayBaloon.Checked && !this.trayIconBaloonIsUp)
            {
                if (this.currentTorrent.trackerUri != null)
                {
                    this.trayIcon.BalloonTipText = this.currentTorrent.trackerUri.Host + "\r\n";
                }
                else
                {
                    this.trayIcon.BalloonTipText = "";
                }
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Uploaded: " + this.FormatFileSize((ulong) this.currentTorrent.uploaded) + "\r\n";
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Downloaded: " + this.FormatFileSize((ulong) this.currentTorrent.downloaded) + "\r\n";
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Update in: " + this.timerValue.Text + "\r\n";
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Total time: " + this.totalRunningTime.Text;
                this.trayIcon.ShowBalloonTip(0x3e8);
             
 
            //edit starts here
            if (this.checkShowTrayBaloon.Checked && !this.trayIconBaloonIsUp && ShowToolTip==true)
            {
                //ok let's disable the flag so that this method will not be runned again means no ballons for a while
                //enable the timer so that it will set the showtooltip to true again afrer some time...
                ShowToolTip = false;


                ttTimer.Start();

                if (this.currentTorrent.trackerUri != null)
                {
                    this.trayIcon.BalloonTipText = this.currentTorrent.trackerUri.Host + "\r\n";
                }
                else
                {
                    this.trayIcon.BalloonTipText = "";
                }
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Uploaded: " + this.FormatFileSize((ulong)this.currentTorrent.uploaded) + "\r\n";
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Downloaded: " + this.FormatFileSize((ulong)this.currentTorrent.downloaded) + "\r\n";
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Update in: " + this.timerValue.Text + "\r\n";
                this.trayIcon.BalloonTipText = this.trayIcon.BalloonTipText + "Total time: " + this.totalRunningTime.Text;
                this.trayIcon.ShowBalloonTip(0x3e8);
            
            }
        }
        //this is for the code above, tool tips, ballons, tray icon...
        static Boolean ShowToolTip = true;
        static System.Timers.Timer ttTimer = new System.Timers.Timer();

        //safe... need not be transfered
        //this is also for the above code block, this is the timer's event

        public static void ttTimerTimeEvent(object source, ElapsedEventArgs e)
        {
            ttTimer.Stop();
            ShowToolTip = true;

            
        }
        //------------------

        */

        

        
        

        //safe... need not be transfered
        private void uploadRate_TextChanged(object sender, EventArgs e)
        {
            currentRMIns.setUploadRate(uploadRate.Text);
            //this.currentTorrent.uploadRate = (long) (this.parseValidFloat(this.uploadRate.Text, 50f) * 1024f);
            //------------------
            //sp edit: lets have the KB/s values pluse kbit/s values for our display

            try
            {
                if (uploadRate.Text.Equals(""))
                {
                    this.labUpkbits.Text = "0";
                }
                else
                {
                    double temp = double.Parse(uploadRate.Text);
                    temp = temp * 8;
                    this.labUpkbits.Text = temp.ToString() + " kbit/s";
                }
                //--------------------------
            }
            catch (FormatException)//this occures when you type only "." or some alpha chars...
            {
                //ignore this...
            }
        }


        //this is not used in the RM class so this is not ben transfered
        //however this is used by the load torrent file method in Form1
        public static string ToHexString(byte[] bytes)
        {
            char[] chArray = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            char[] chArray2 = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int num2 = bytes[i];
                chArray2[i * 2] = chArray[num2 >> 4];
                chArray2[(i * 2) + 1] = chArray[num2 & 15];
            }
            return new string(chArray2);
        }
        


        public enum SizeUnits//
        {
            KB,
            MB,
            GB
        }

        
        public enum TimeUnits//
        {
            Seconds,
            Minutes,
            Hours
        }

        
        private void butCrash_Click_1(object sender, EventArgs e)
        {
            currentRMIns.Crash();
        }

        private void checkRandomUpload_CheckedChanged(object sender, EventArgs e)
        {

                currentRMIns.boolRandomUpload = checkRandomUpload.Checked;
        }

        private void checkRandomDownload_CheckedChanged(object sender, EventArgs e)
        {

                currentRMIns.boolRandomDownload = checkRandomDownload.Checked;

        }

        private void RandomUploadFrom_TextChanged(object sender, EventArgs e)
        {
            currentRMIns.strRandomUploadFrom = RandomUploadFrom.Text;
        }

        private void RandomUploadTo_TextChanged(object sender, EventArgs e)
        {
            currentRMIns.strRandomUploadTo = RandomDownloadTo.Text;
        }

        private void RandomDownloadFrom_TextChanged(object sender, EventArgs e)
        {
            currentRMIns.strRandomDownloadFrom = RandomDownloadFrom.Text;
        }

        private void RandomDownloadTo_TextChanged(object sender, EventArgs e)
        {
            currentRMIns.strRandomDownloadTo = RandomDownloadTo.Text;
        }

        private void textProxyHost_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkTCPListen_CheckedChanged(object sender, EventArgs e)
        {
            currentRMIns.boolTCPListen = checkTCPListen.Checked;
        }

        private void textStopMinLeecher_ValueChanged(object sender, EventArgs e)
        {
            currentRMIns.strStopMinLeecherVal = textStopMinLeecher.Value.ToString();
        }

        private void checkIgnoreFailureReason_CheckedChanged(object sender, EventArgs e)
        {
            currentRMIns.boolIgnoreFailureReason = checkIgnoreFailureReason.Checked;
        }

        private void stopProcessUnitsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRMIns.intStopProcessUnitsBoxSelectedIndex = stopProcessUnitsBox.SelectedIndex;
        }

        private void stopProcessValue_TextChanged(object sender, EventArgs e)
        {
            currentRMIns.strStopProcessValue = stopProcessValue.Text;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ok the user decided to cancel the 'start the torret' act

            
            //user proof stuff goes here

            //enable the new RM tool bar button
            Program.mainUI.toolStripButton1.Enabled = true;

            //enable the new RM menu item
            Program.mainUI.newRMToolStripMenuItem.Enabled = true;


            //we should remove the RM object and the item in the listMain... mainUI
            Program.mainUI.removeRM(Program.RMcount, false);

            //---------------------------------
        }

        //this enables the method to call it self... in the end
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);


        public void setLabelMemReaderStatus(string text)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.labAutoMem.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(setLabelMemReaderStatus);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.labAutoMem.Text = text;

                    //check the status set on the lable and hide the progress bar...
                    if (!text.Equals("Looking for client process..."))
                    {
                        progressBar1.Visible = false;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                //i am not very sure about this but this occures when the New RM UI is closed while the memreader thread
                //is still exicuting... and it tries to update values here... then this exception is thrown

                //Ignoring this! :P
            }

        }


        //this enables the method to call it self... in the end
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetProgBarCallback(int[] pars);

        public void setProgressBarInit(int[] pars)

        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.progressBar1.InvokeRequired)
                {
                    SetProgBarCallback d = new SetProgBarCallback(setProgressBarInit);
                    this.Invoke(d, new object[] { pars });
                }
                else
                {
                    this.progressBar1.Maximum = pars[0];
                    this.progressBar1.Minimum = pars[1];

                    if (pars[2] == 0)
                    {
                        this.progressBar1.Visible = true;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                //i am not very sure about this but this occures when the New RM UI is closed while the memreader thread
                //is still exicuting... and it tries to update values here... then this exception is thrown

                //Ignoring this! :P
            }
        }

        //this enables the method to call it self... in the end
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetProgBarValueCallback(int value);

        public void setProgressBarValue(int value)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.progressBar1.InvokeRequired)
                {
                    SetProgBarValueCallback d = new SetProgBarValueCallback(setProgressBarValue);
                    this.Invoke(d, new object[] { value });
                }
                else
                {
                    this.progressBar1.Value = value;
                }
            }
            catch (ObjectDisposedException)
            {
                //i am not very sure about this but this occures when the New RM UI is closed while the memreader thread
                //is still exicuting... and it tries to update values here... then this exception is thrown

                //Ignoring this! :P
            }
        }

        //this enables the method to call it self... in the end
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void setMemReaderValuesCallback(string[] values);

        public void setMemReaderValues(string[] values)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (((this.customPeerID.InvokeRequired) || (this.customPeersNum.InvokeRequired))
                   || ((this.customKey.InvokeRequired) || (this.customPort.InvokeRequired)))
                {
                    setMemReaderValuesCallback d = new setMemReaderValuesCallback(setMemReaderValues);
                    this.Invoke(d, new object[] { values });
                }
                else
                {
                    customPeerID.Text = values[0];
                    if (values[1] != "0")
                    {
                        customPeersNum.Text = values[1];
                    }
                    customKey.Text = values[2];
                    customPort.Text = values[3];
                }
            }
            catch (ObjectDisposedException)
            {
                //i am not very sure about this but this occures when the New RM UI is closed while the memreader thread
                //is still exicuting... and it tries to update values here... then this exception is thrown

                //Ignoring this! :P
            }
        }

        private void chkUnifiedClientSimu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnifiedClientSimu.Checked == true)
            {
                labUCSstatus.Text = "Unified Clent Simulation Not Active";

                uniClientSimiulation();
            }
            else
            {
                labUCSstatus.Text = "Unified Clent Simulation Disabled";
            }
        }

        private void checkBoxCustomSearchString_CheckedChanged(object sender, EventArgs e)
        {
            //enable disable custom search string for the memory reader
            if (checkBoxCustomSearchString.Checked == true)
            {
                txtCustomSearchString.Enabled = true;
            }
            else
            {
                txtCustomSearchString.Enabled = false;
            }
        }


    }
}

