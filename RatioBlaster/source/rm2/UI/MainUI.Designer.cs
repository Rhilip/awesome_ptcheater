namespace rm2.UI
{
    partial class MainUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newRMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeRMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.saveACompleteLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.celarAllLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listMain = new System.Windows.Forms.ListView();
            this.colTorName = new System.Windows.Forms.ColumnHeader();
            this.colSize = new System.Windows.Forms.ColumnHeader();
            this.colDone = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.colSeeds = new System.Windows.Forms.ColumnHeader();
            this.colPeers = new System.Windows.Forms.ColumnHeader();
            this.colDownSpeed = new System.Windows.Forms.ColumnHeader();
            this.colUpSpeed = new System.Windows.Forms.ColumnHeader();
            this.colUploaded = new System.Windows.Forms.ColumnHeader();
            this.colDownloaded = new System.Windows.Forms.ColumnHeader();
            this.colRatio = new System.Windows.Forms.ColumnHeader();
            this.colTotalTime = new System.Windows.Forms.ColumnHeader();
            this.colRemaining = new System.Windows.Forms.ColumnHeader();
            this.colUpdateIn = new System.Windows.Forms.ColumnHeader();
            this.conMmainList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Speeds = new System.Windows.Forms.ToolStripMenuItem();
            this.downSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.upSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.enableRandomizingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableRandomUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableRandomDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomizeNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomizingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.toToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox5 = new System.Windows.Forms.ToolStripTextBox();
            this.toToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox6 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox7 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.updateNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setIntervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox9 = new System.Windows.Forms.ToolStripTextBox();
            this.ignoreIntervalUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.saveLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.timStatus = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupTorrentInfo = new System.Windows.Forms.GroupBox();
            this.torrentComment = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TorrentFileLabel = new System.Windows.Forms.Label();
            this.labelTorrentSize = new System.Windows.Forms.Label();
            this.hashLabel = new System.Windows.Forms.Label();
            this.TrackerLabel = new System.Windows.Forms.Label();
            this.reportParamsGroup = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.keyLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numwantLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtxtTorrentLog = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rtxtGeneralLogBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.conMmainList.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupTorrentInfo.SuspendLayout();
            this.reportParamsGroup.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.changeToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(830, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newRMToolStripMenuItem,
            this.removeRMToolStripMenuItem,
            this.toolStripSeparator4,
            this.saveACompleteLogToolStripMenuItem,
            this.celarAllLogsToolStripMenuItem,
            this.toolStripSeparator5,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fileToolStripMenuItem.Text = "Actions";
            // 
            // newRMToolStripMenuItem
            // 
            this.newRMToolStripMenuItem.Name = "newRMToolStripMenuItem";
            this.newRMToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newRMToolStripMenuItem.Text = "New RM";
            this.newRMToolStripMenuItem.Click += new System.EventHandler(this.newRMToolStripMenuItem_Click);
            // 
            // removeRMToolStripMenuItem
            // 
            this.removeRMToolStripMenuItem.Enabled = false;
            this.removeRMToolStripMenuItem.Name = "removeRMToolStripMenuItem";
            this.removeRMToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.removeRMToolStripMenuItem.Text = "Remove RM";
            this.removeRMToolStripMenuItem.Click += new System.EventHandler(this.removeRMToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(183, 6);
            // 
            // saveACompleteLogToolStripMenuItem
            // 
            this.saveACompleteLogToolStripMenuItem.Name = "saveACompleteLogToolStripMenuItem";
            this.saveACompleteLogToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveACompleteLogToolStripMenuItem.Text = "Save a Complete Log";
            this.saveACompleteLogToolStripMenuItem.Click += new System.EventHandler(this.saveACompleteLogToolStripMenuItem_Click);
            // 
            // celarAllLogsToolStripMenuItem
            // 
            this.celarAllLogsToolStripMenuItem.Name = "celarAllLogsToolStripMenuItem";
            this.celarAllLogsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.celarAllLogsToolStripMenuItem.Text = "Celar All Logs";
            this.celarAllLogsToolStripMenuItem.Click += new System.EventHandler(this.celarAllLogsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(183, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.changeToolStripMenuItem.Text = "Change";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // listMain
            // 
            this.listMain.AllowDrop = true;
            this.listMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTorName,
            this.colSize,
            this.colDone,
            this.colStatus,
            this.colSeeds,
            this.colPeers,
            this.colDownSpeed,
            this.colUpSpeed,
            this.colUploaded,
            this.colDownloaded,
            this.colRatio,
            this.colTotalTime,
            this.colRemaining,
            this.colUpdateIn});
            this.listMain.ContextMenuStrip = this.conMmainList;
            this.listMain.FullRowSelect = true;
            this.listMain.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listMain.Location = new System.Drawing.Point(12, 52);
            this.listMain.MultiSelect = false;
            this.listMain.Name = "listMain";
            this.listMain.Size = new System.Drawing.Size(806, 165);
            this.listMain.TabIndex = 1;
            this.listMain.UseCompatibleStateImageBehavior = false;
            this.listMain.View = System.Windows.Forms.View.Details;
            this.listMain.SelectedIndexChanged += new System.EventHandler(this.listMain_SelectedIndexChanged);
            this.listMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.listMain_DragDrop);
            this.listMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.listMain_DragEnter);
            // 
            // colTorName
            // 
            this.colTorName.Text = "Name";
            this.colTorName.Width = 112;
            // 
            // colSize
            // 
            this.colSize.Text = "Size";
            // 
            // colDone
            // 
            this.colDone.Text = "Done";
            this.colDone.Width = 42;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            // 
            // colSeeds
            // 
            this.colSeeds.Text = "Seeds";
            this.colSeeds.Width = 41;
            // 
            // colPeers
            // 
            this.colPeers.Text = "Peers";
            this.colPeers.Width = 39;
            // 
            // colDownSpeed
            // 
            this.colDownSpeed.Text = "Down Speed";
            // 
            // colUpSpeed
            // 
            this.colUpSpeed.Text = "Up Speed";
            // 
            // colUploaded
            // 
            this.colUploaded.Text = "Uploaded";
            // 
            // colDownloaded
            // 
            this.colDownloaded.Text = "Downloaded";
            // 
            // colRatio
            // 
            this.colRatio.Text = "Ratio";
            this.colRatio.Width = 43;
            // 
            // colTotalTime
            // 
            this.colTotalTime.Text = "Total Time";
            // 
            // colRemaining
            // 
            this.colRemaining.Text = "Remaining";
            // 
            // colUpdateIn
            // 
            this.colUpdateIn.Text = "Update In";
            // 
            // conMmainList
            // 
            this.conMmainList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.removeItem,
            this.crashToolStripMenuItem,
            this.toolStripSeparator3,
            this.Speeds,
            this.toolStripSeparator2,
            this.renameToolStripMenuItem,
            this.toolStripSeparator1,
            this.updateNowToolStripMenuItem,
            this.intervalToolStripMenuItem,
            this.toolStripSeparator6,
            this.saveLogToolStripMenuItem,
            this.clearLogToolStripMenuItem});
            this.conMmainList.Name = "conMmainList";
            this.conMmainList.Size = new System.Drawing.Size(145, 248);
            this.conMmainList.Opening += new System.ComponentModel.CancelEventHandler(this.conMmainList_Opening);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Enabled = false;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // removeItem
            // 
            this.removeItem.Name = "removeItem";
            this.removeItem.Size = new System.Drawing.Size(144, 22);
            this.removeItem.Text = "Remove";
            this.removeItem.Click += new System.EventHandler(this.removeItem_Click);
            // 
            // crashToolStripMenuItem
            // 
            this.crashToolStripMenuItem.Name = "crashToolStripMenuItem";
            this.crashToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.crashToolStripMenuItem.Text = "\"Crash\"";
            this.crashToolStripMenuItem.Click += new System.EventHandler(this.crashToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // Speeds
            // 
            this.Speeds.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downSpeedToolStripMenuItem,
            this.upSpeedToolStripMenuItem,
            this.toolStripSeparator7,
            this.enableRandomizingToolStripMenuItem,
            this.randomizeNowToolStripMenuItem,
            this.randomizingToolStripMenuItem});
            this.Speeds.Name = "Speeds";
            this.Speeds.Size = new System.Drawing.Size(144, 22);
            this.Speeds.Text = "Speeds";
            // 
            // downSpeedToolStripMenuItem
            // 
            this.downSpeedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.downSpeedToolStripMenuItem.Name = "downSpeedToolStripMenuItem";
            this.downSpeedToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.downSpeedToolStripMenuItem.Text = "Down Speed";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            // 
            // upSpeedToolStripMenuItem
            // 
            this.upSpeedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox2});
            this.upSpeedToolStripMenuItem.Name = "upSpeedToolStripMenuItem";
            this.upSpeedToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.upSpeedToolStripMenuItem.Text = "Up Speed";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox2_KeyPress);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(158, 6);
            // 
            // enableRandomizingToolStripMenuItem
            // 
            this.enableRandomizingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableRandomUpToolStripMenuItem,
            this.disableRandomDownToolStripMenuItem});
            this.enableRandomizingToolStripMenuItem.Name = "enableRandomizingToolStripMenuItem";
            this.enableRandomizingToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.enableRandomizingToolStripMenuItem.Text = "En/Disable";
            // 
            // disableRandomUpToolStripMenuItem
            // 
            this.disableRandomUpToolStripMenuItem.Name = "disableRandomUpToolStripMenuItem";
            this.disableRandomUpToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.disableRandomUpToolStripMenuItem.Text = "Disable Random Up";
            this.disableRandomUpToolStripMenuItem.Click += new System.EventHandler(this.disableRandomUpToolStripMenuItem_Click);
            // 
            // disableRandomDownToolStripMenuItem
            // 
            this.disableRandomDownToolStripMenuItem.Name = "disableRandomDownToolStripMenuItem";
            this.disableRandomDownToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.disableRandomDownToolStripMenuItem.Text = "Disable Random Down";
            this.disableRandomDownToolStripMenuItem.Click += new System.EventHandler(this.disableRandomDownToolStripMenuItem_Click);
            // 
            // randomizeNowToolStripMenuItem
            // 
            this.randomizeNowToolStripMenuItem.Name = "randomizeNowToolStripMenuItem";
            this.randomizeNowToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.randomizeNowToolStripMenuItem.Text = "Randomize Now";
            this.randomizeNowToolStripMenuItem.Click += new System.EventHandler(this.randomizeNowToolStripMenuItem_Click);
            // 
            // randomizingToolStripMenuItem
            // 
            this.randomizingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downToolStripMenuItem,
            this.upToolStripMenuItem});
            this.randomizingToolStripMenuItem.Name = "randomizingToolStripMenuItem";
            this.randomizingToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.randomizingToolStripMenuItem.Text = "Randomizing";
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromToolStripMenuItem,
            this.toToolStripMenuItem});
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            this.downToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.downToolStripMenuItem.Text = "Down";
            // 
            // fromToolStripMenuItem
            // 
            this.fromToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox4});
            this.fromToolStripMenuItem.Name = "fromToolStripMenuItem";
            this.fromToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.fromToolStripMenuItem.Text = "From";
            this.fromToolStripMenuItem.Click += new System.EventHandler(this.fromToolStripMenuItem_Click);
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox4_KeyPress);
            // 
            // toToolStripMenuItem
            // 
            this.toToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox3});
            this.toToolStripMenuItem.Name = "toToolStripMenuItem";
            this.toToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.toToolStripMenuItem.Text = "To";
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox3_KeyPress);
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromToolStripMenuItem1,
            this.toToolStripMenuItem1});
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.upToolStripMenuItem.Text = "Up";
            // 
            // fromToolStripMenuItem1
            // 
            this.fromToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox5});
            this.fromToolStripMenuItem1.Name = "fromToolStripMenuItem1";
            this.fromToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            this.fromToolStripMenuItem1.Text = "From";
            // 
            // toolStripTextBox5
            // 
            this.toolStripTextBox5.Name = "toolStripTextBox5";
            this.toolStripTextBox5.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox5_KeyPress);
            // 
            // toToolStripMenuItem1
            // 
            this.toToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox6});
            this.toToolStripMenuItem1.Name = "toToolStripMenuItem1";
            this.toToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            this.toToolStripMenuItem1.Text = "To";
            // 
            // toolStripTextBox6
            // 
            this.toolStripTextBox6.Name = "toolStripTextBox6";
            this.toolStripTextBox6.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox6_KeyPress);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(141, 6);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox7});
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // toolStripTextBox7
            // 
            this.toolStripTextBox7.Name = "toolStripTextBox7";
            this.toolStripTextBox7.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox7_KeyPress);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
            // 
            // updateNowToolStripMenuItem
            // 
            this.updateNowToolStripMenuItem.Name = "updateNowToolStripMenuItem";
            this.updateNowToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.updateNowToolStripMenuItem.Text = "Update Now";
            this.updateNowToolStripMenuItem.Click += new System.EventHandler(this.updateNowToolStripMenuItem_Click);
            // 
            // intervalToolStripMenuItem
            // 
            this.intervalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setIntervalToolStripMenuItem,
            this.ignoreIntervalUpdatesToolStripMenuItem});
            this.intervalToolStripMenuItem.Name = "intervalToolStripMenuItem";
            this.intervalToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.intervalToolStripMenuItem.Text = "Interval";
            // 
            // setIntervalToolStripMenuItem
            // 
            this.setIntervalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox9});
            this.setIntervalToolStripMenuItem.Name = "setIntervalToolStripMenuItem";
            this.setIntervalToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.setIntervalToolStripMenuItem.Text = "Set Interval";
            // 
            // toolStripTextBox9
            // 
            this.toolStripTextBox9.Name = "toolStripTextBox9";
            this.toolStripTextBox9.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox9_KeyPress);
            // 
            // ignoreIntervalUpdatesToolStripMenuItem
            // 
            this.ignoreIntervalUpdatesToolStripMenuItem.Name = "ignoreIntervalUpdatesToolStripMenuItem";
            this.ignoreIntervalUpdatesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ignoreIntervalUpdatesToolStripMenuItem.Text = "Ignore Interval Updates";
            this.ignoreIntervalUpdatesToolStripMenuItem.Click += new System.EventHandler(this.ignoreIntervalUpdatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(141, 6);
            // 
            // saveLogToolStripMenuItem
            // 
            this.saveLogToolStripMenuItem.Name = "saveLogToolStripMenuItem";
            this.saveLogToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.saveLogToolStripMenuItem.Text = "Save Log";
            this.saveLogToolStripMenuItem.Click += new System.EventHandler(this.saveLogToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(830, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "New RM";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Remove RM";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.ToolTipText = "Minimize RB to Tray";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.labStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 464);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(830, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel1.Text = "# RM Objects";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 17);
            this.toolStripStatusLabel2.Text = "0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(55, 17);
            this.toolStripStatusLabel3.Text = "| Status : ";
            // 
            // labStatus
            // 
            this.labStatus.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(38, 17);
            this.labStatus.Text = "Ready";
            // 
            // timStatus
            // 
            this.timStatus.Enabled = true;
            this.timStatus.Interval = 1000;
            this.timStatus.Tick += new System.EventHandler(this.timStatus_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = Program.APPLICATION_NAME;
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseMove);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 223);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(806, 238);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupTorrentInfo);
            this.tabPage1.Controls.Add(this.reportParamsGroup);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(798, 212);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Torrent Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupTorrentInfo
            // 
            this.groupTorrentInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupTorrentInfo.Controls.Add(this.torrentComment);
            this.groupTorrentInfo.Controls.Add(this.label11);
            this.groupTorrentInfo.Controls.Add(this.label10);
            this.groupTorrentInfo.Controls.Add(this.label5);
            this.groupTorrentInfo.Controls.Add(this.label3);
            this.groupTorrentInfo.Controls.Add(this.label2);
            this.groupTorrentInfo.Controls.Add(this.label1);
            this.groupTorrentInfo.Controls.Add(this.TorrentFileLabel);
            this.groupTorrentInfo.Controls.Add(this.labelTorrentSize);
            this.groupTorrentInfo.Controls.Add(this.hashLabel);
            this.groupTorrentInfo.Controls.Add(this.TrackerLabel);
            this.groupTorrentInfo.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupTorrentInfo.Location = new System.Drawing.Point(9, 6);
            this.groupTorrentInfo.Name = "groupTorrentInfo";
            this.groupTorrentInfo.Size = new System.Drawing.Size(783, 107);
            this.groupTorrentInfo.TabIndex = 18;
            this.groupTorrentInfo.TabStop = false;
            this.groupTorrentInfo.Text = "Torrent Information";
            // 
            // torrentComment
            // 
            this.torrentComment.AutoSize = true;
            this.torrentComment.Location = new System.Drawing.Point(500, 52);
            this.torrentComment.Name = "torrentComment";
            this.torrentComment.Size = new System.Drawing.Size(16, 13);
            this.torrentComment.TabIndex = 23;
            this.torrentComment.Text = "...";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(521, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "...";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(439, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Update Interval: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(675, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(95, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 17;
            // 
            // TorrentFileLabel
            // 
            this.TorrentFileLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TorrentFileLabel.Location = new System.Drawing.Point(36, 76);
            this.TorrentFileLabel.Name = "TorrentFileLabel";
            this.TorrentFileLabel.Size = new System.Drawing.Size(42, 23);
            this.TorrentFileLabel.TabIndex = 16;
            this.TorrentFileLabel.Text = "Path:";
            this.TorrentFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTorrentSize
            // 
            this.labelTorrentSize.AutoSize = true;
            this.labelTorrentSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelTorrentSize.Location = new System.Drawing.Point(439, 52);
            this.labelTorrentSize.Name = "labelTorrentSize";
            this.labelTorrentSize.Size = new System.Drawing.Size(54, 13);
            this.labelTorrentSize.TabIndex = 6;
            this.labelTorrentSize.Text = "Comment:";
            // 
            // hashLabel
            // 
            this.hashLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.hashLabel.Location = new System.Drawing.Point(9, 47);
            this.hashLabel.Name = "hashLabel";
            this.hashLabel.Size = new System.Drawing.Size(69, 23);
            this.hashLabel.TabIndex = 4;
            this.hashLabel.Text = "SHA Hash:";
            this.hashLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // reportParamsGroup
            // 
            this.reportParamsGroup.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.reportParamsGroup.Controls.Add(this.label9);
            this.reportParamsGroup.Controls.Add(this.label8);
            this.reportParamsGroup.Controls.Add(this.label7);
            this.reportParamsGroup.Controls.Add(this.label6);
            this.reportParamsGroup.Controls.Add(this.keyLabel);
            this.reportParamsGroup.Controls.Add(this.label4);
            this.reportParamsGroup.Controls.Add(this.numwantLabel);
            this.reportParamsGroup.Controls.Add(this.portLabel);
            this.reportParamsGroup.ForeColor = System.Drawing.SystemColors.Desktop;
            this.reportParamsGroup.Location = new System.Drawing.Point(9, 119);
            this.reportParamsGroup.Name = "reportParamsGroup";
            this.reportParamsGroup.Size = new System.Drawing.Size(783, 75);
            this.reportParamsGroup.TabIndex = 3;
            this.reportParamsGroup.TabStop = false;
            this.reportParamsGroup.Text = "Announce Parameters";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(602, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "...";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(115, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "...";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(599, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(112, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "...";
            // 
            // keyLabel
            // 
            this.keyLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.keyLabel.Location = new System.Drawing.Point(15, 45);
            this.keyLabel.Name = "keyLabel";
            this.keyLabel.Size = new System.Drawing.Size(90, 13);
            this.keyLabel.TabIndex = 3;
            this.keyLabel.Text = "Pass Key (key):";
            this.keyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(9, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Peer ID (peer_id):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numwantLabel
            // 
            this.numwantLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.numwantLabel.Location = new System.Drawing.Point(439, 48);
            this.numwantLabel.Name = "numwantLabel";
            this.numwantLabel.Size = new System.Drawing.Size(153, 13);
            this.numwantLabel.TabIndex = 6;
            this.numwantLabel.Text = "Number of Peers (numwant):";
            this.numwantLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // portLabel
            // 
            this.portLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.portLabel.Location = new System.Drawing.Point(533, 17);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(59, 17);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "Port (port):";
            this.portLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rtxtTorrentLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(798, 212);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Torrent Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rtxtTorrentLog
            // 
            this.rtxtTorrentLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtTorrentLog.Location = new System.Drawing.Point(6, 6);
            this.rtxtTorrentLog.Name = "rtxtTorrentLog";
            this.rtxtTorrentLog.ReadOnly = true;
            this.rtxtTorrentLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtTorrentLog.Size = new System.Drawing.Size(786, 200);
            this.rtxtTorrentLog.TabIndex = 0;
            this.rtxtTorrentLog.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rtxtGeneralLogBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(798, 212);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "General Log";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtxtGeneralLogBox
            // 
            this.rtxtGeneralLogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtGeneralLogBox.Location = new System.Drawing.Point(6, 6);
            this.rtxtGeneralLogBox.Name = "rtxtGeneralLogBox";
            this.rtxtGeneralLogBox.ReadOnly = true;
            this.rtxtGeneralLogBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtGeneralLogBox.Size = new System.Drawing.Size(786, 200);
            this.rtxtGeneralLogBox.TabIndex = 1;
            this.rtxtGeneralLogBox.Text = "";
            // 
            // clsFMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 486);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listMain);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "clsFMain";
            this.Text = Program.APPLICATION_NAME;
            this.Load += new System.EventHandler(this.clsFMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.clsFMain_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.conMmainList.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupTorrentInfo.ResumeLayout(false);
            this.groupTorrentInfo.PerformLayout();
            this.reportParamsGroup.ResumeLayout(false);
            this.reportParamsGroup.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ListView listMain;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ColumnHeader colTorName;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colDone;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colSeeds;
        private System.Windows.Forms.ColumnHeader colPeers;
        private System.Windows.Forms.ColumnHeader colDownSpeed;
        private System.Windows.Forms.ColumnHeader colUpSpeed;
        private System.Windows.Forms.ColumnHeader colUploaded;
        private System.Windows.Forms.ColumnHeader colDownloaded;
        private System.Windows.Forms.ColumnHeader colRatio;
        private System.Windows.Forms.ColumnHeader colTotalTime;
        private System.Windows.Forms.ColumnHeader colRemaining;
        private System.Windows.Forms.ColumnHeader colUpdateIn;
        private System.Windows.Forms.ToolStripMenuItem removeRMToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Timer timStatus;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ContextMenuStrip conMmainList;
        private System.Windows.Forms.ToolStripMenuItem Speeds;
        private System.Windows.Forms.ToolStripMenuItem removeItem;
        private System.Windows.Forms.ToolStripMenuItem downSpeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem upSpeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripMenuItem randomizingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox4;
        private System.Windows.Forms.ToolStripMenuItem toToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox3;
        private System.Windows.Forms.ToolStripMenuItem upToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromToolStripMenuItem1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox5;
        private System.Windows.Forms.ToolStripMenuItem toToolStripMenuItem1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox6;
        private System.Windows.Forms.ToolStripMenuItem crashToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem updateNowToolStripMenuItem;
        private System.Windows.Forms.GroupBox reportParamsGroup;
        private System.Windows.Forms.Label keyLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label numwantLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.GroupBox groupTorrentInfo;
        private System.Windows.Forms.Label labelTorrentSize;
        private System.Windows.Forms.Label hashLabel;
        private System.Windows.Forms.Label TrackerLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TorrentFileLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem randomizeNowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label torrentComment;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem intervalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setIntervalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreIntervalUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem saveACompleteLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem saveLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem celarAllLogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem enableRandomizingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableRandomUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableRandomDownToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem newRMToolStripMenuItem;
        public System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel labStatus;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        internal System.Windows.Forms.RichTextBox rtxtTorrentLog;
        internal System.Windows.Forms.RichTextBox rtxtGeneralLogBox;
    }
}

