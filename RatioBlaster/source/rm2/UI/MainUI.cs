using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using RatioBlaster.rm2.Controllers;

namespace rm2.UI
{
    public partial class MainUI : Form
    {

        /* ------------ Variable Declarations -------------*/

        //this is to keep the last selected item in the mainList
       // DMSoft.SkinCrafter SkinOb; // 'pointer' to the skin object ;)

        ////the settings
        //RM2Settings settings = new RM2Settings();

        bool expectAppClosing = false;

        //bool autoScrollLog = false; //should we auto scroll the log when its updated?

        int lastSelItem = 9999; //can't set this to null so set this to a defult value of 9999

        //this is mainly for the mainUI's tab interface...
        int selectedItem = 9999;

        TorrentSession tempRM;

        //whether the user wants tool tips or not?
        //bool showToolTips;

        //int indexCounter;

        /* ------------ Methods ---------------------------*/

        //this saveLog method saves all of the logs of all of the RM instences... + mem reader + app settings loader...
        public void saveLog()
        {   
            Stream stream;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory = Application.StartupPath;

            //include some time date info in the file name
            DateTime now = DateTime.Now;
            string dateTime = now.Day + "." + now.Hour + "." + now.Minute + "." + now.Second;
            dialog.FileName = Program.APPLICATION_NAME + dateTime + ".log";
            
            if ((dialog.ShowDialog() == DialogResult.OK) && ((stream = dialog.OpenFile()) != null))
            {
                if (Logger.Dump(stream))
                {
                    MessageBox.Show("Log Saved Successfully", Program.APPLICATION_NAME);
                }
            }
        }

        //this saveLog method only saves a log of one given RM instence...
        private void saveLog(int rmIndex)
        {
            Stream stream;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory = Application.StartupPath;

            //include some time date info in the file name
            DateTime now = DateTime.Now;

            string dateTime = now.Day + "." + now.Hour + "." + now.Minute + "." + now.Second;
            dialog.FileName = Program.APPLICATION_NAME + dateTime + ".log";

            if ((dialog.ShowDialog() == DialogResult.OK) && ((stream = dialog.OpenFile()) != null))
            {
                if (Logger.Dump(stream, Program.rmCol[rmIndex])) ;
                {
                    MessageBox.Show("Log Saved Successfully", Program.APPLICATION_NAME);
                }
            }
        }

        private void loadInfoTab(int rmIndex)
        {
            //read the data off the RM object and present them in the MainUI
            label1.Text = Program.rmCol[rmIndex].currentTorrent.tracker;
            label2.Text = Program.rmCol[rmIndex].currentTorrent.hash;
            label3.Text = Program.rmCol[rmIndex].currentTorrent.filename;
            this.torrentComment.Text = Program.rmCol[rmIndex].currentTorrentFile.Comment;
            label6.Text = Program.rmCol[rmIndex].currentTorrent.peerID;
            label8.Text = Program.rmCol[rmIndex].currentTorrent.key;
            label7.Text = Program.rmCol[rmIndex].currentTorrent.port;
            label9.Text = Program.rmCol[rmIndex].currentTorrent.numberOfPeers;
            label11.Text = Program.rmCol[rmIndex].currentTorrent.interval.ToString();//interval
        }

        private void loadRMsLog(int rmIndex)
        {
            //RM2Settings settings = new RM2Settings();

            try
            {
                rtxtTorrentLog.Text = Program.rmCol[rmIndex].strBigLog;

                //do the auto scrolling if the yours wants it
                if (Program.settings.AutoRMScrollLog == true)
                {
                    rtxtTorrentLog.SelectionStart = rtxtTorrentLog.Text.Length;
                    rtxtTorrentLog.ScrollToCaret();
                }

                if (Program.settings.AutoGenScrollLog == true)
                {
                    rtxtGeneralLogBox.SelectionStart = rtxtTorrentLog.Text.Length;
                    rtxtGeneralLogBox.ScrollToCaret();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "loadRMsLog");
            }

        }

        private void newRM()
        {
            newRM(null);
        }

        private void newRM(string torrentPath)
        {
   //         try
   //         {

                //make this user proof! :P
                //disable the new RM tool bar button
                toolStripButton1.Enabled = false;
                
            //disable the new RM menu item
                newRMToolStripMenuItem.Enabled = false;

                //global counter for RMs
                Program.RMcount++;

                //Initillize the Item and it's sub items... so that this will go on gracefuly :)
                //add a new item to the list this will be properly modified by the Form1 UI later
                
                //temp sub item
                ListViewItem.ListViewSubItem tmpSitem;
                
                listMain.Items.Add(new ListViewItem());

                //set the items color accoring to the last time's set color...
                if (listMain.Items.Count == 1)
                {
                    listMain.Items[listMain.Items.Count - 1].BackColor = Color.AliceBlue;
                }
                else
                {

                    if (listMain.Items[listMain.Items.Count - 2].BackColor == Color.AliceBlue)
                    {
                        //this is the current last item...
                        listMain.Items[listMain.Items.Count - 1].BackColor = Color.White;
                    }
                    else
                    {
                        listMain.Items[listMain.Items.Count - 1].BackColor = Color.AliceBlue;
                    }
                }
                //fill the item with sub items
                for (int a = 0; a < 14; a++)
                {
                    tmpSitem = new ListViewItem.ListViewSubItem();
                    listMain.Items[listMain.Items.Count - 1].SubItems.Add(tmpSitem);
                }

                //create and add to the collection...
                tempRM = new TorrentSession(Program.RMcount);
                Program.rmCol.Add(tempRM);



                //show the New Torrent UI ;)
                Program.tempConUI = new rm2.UI.TorrerntSettingsUI();
                Program.tempConUI.currentRMIns = Program.rmCol[Program.RMcount];
                Program.tempConUI.Show();
                if (torrentPath != null)
                {
                    Program.tempConUI.loadTorrentFileInfo(torrentPath, true);
                }


                //ListViewItem item = listMain.Items.Add(file.Name);
               // listMain.Items[Program.RMcount].SubItems.Add(Program.rmCol[Program.RMcount].currentTorrent.tracker);
               // listMain.Items[Program.RMcount].Text = Program.RMcount.ToString();
               // listMain.Items[Program.RMcount].SubItems[0].Text = Program.rmCol[Program.RMcount].currentTorrent.tracker;

                
     //       }
     //       catch (Exception e)
     //       {
     //           MessageBox.Show(e.ToString(), "clsMain>newRM() error!");
     //       }
        }

        public void stopRM(int itemIndex, bool crash)
        {
            //stop the torrent, only if it has been already started :)
            if (Program.rmCol[itemIndex].Started)
            {
                //check wther we should stop it or crash it

                if (crash == true)
                {
                    Program.rmCol[itemIndex].Crash();
                }
                else
                {
                    Program.rmCol[itemIndex].Stop();
                }
            }
        }

        public void removeRM(int itemIndex, bool crash)
        {
            //stop the torrent, only if it has been already started :)
            if (Program.rmCol[itemIndex].Started)
            {
                //check wther we should stop it or crash it

                if (crash == true)
                {
                    Program.rmCol[itemIndex].Crash();
                }
                else
                {
                    Program.rmCol[itemIndex].Stop();
                }
            }
            
            //remove the RM object from the collection
            Program.rmCol.RemoveAt(itemIndex);

            //nw reoder the corresponding item list indexs in RM objects...
            reorderMainUiList(itemIndex);

            //remove the correspoinding item from the mainList
            listMain.Items[itemIndex].Remove();

            //reduce the RM counter by one
            Program.RMcount--;
        }

        private void reorderMainUiList(int removedItemIndex)
        {
            //we should not bother doing this if there was only one item in the item list when it, it self
            //got removed...

            //or if the item removed was the last item...
            if ( (listMain.Items.Count != 1) && !(listMain.Items.Count == (removedItemIndex +1) ) )
            {
                int rounds = listMain.Items.Count - (removedItemIndex + 1); //how many rounds should we loop
                //and update RM objects...
                // +1 is because removed Item Index is
                //starting at 0
                int accessRm = removedItemIndex;                    //+1 is not needed here for the starti
                //at 0 thing... and not for any other logic
                //as ^ rmcol automaticaly reorder it self..

                for (int a = 0; a < (rounds); a++)
                {
                    Program.rmCol[accessRm].rmIndex--;
                    //roll the ball!
                    accessRm++;
                }
            }
        }

        /*
         * this is not needed
        private void reorderRmCol(int removedRmObjIndex)
        {
            //this is gonna have a copied modified version of whats found in the reorderMainUiList
            int rounds = Program.rmCol.Count - removedRmObjIndex + 1; //how many rounds should we loop
            //and update RM objects...
            // +1 is because removed Item Index is
            //starting at 0
            int accessRm = removedRmObjIndex + 1;//+1 is not needed here for the starting at 0 thing... but for some other logic

            for (int a = 0; a < (rounds + 1); a++)
            {
                Program.rmCol[accessRm] = Program.rmCol[accessRm - 1;
            }
        }*/

        public void addToList()
        {   
            
            //add this to main UI's item list..
            //listMain.Items.Add(Program.RMcount.ToString());
            //Program.currentRM = Program.rmCol[Program.RMcount];
            //ListViewItem item = new ListViewItem();
            listMain.Items[Program.RMcount].Text = Program.rmCol[Program.RMcount].currentTorrentFile.Name;
            listMain.Items[Program.RMcount].Tag = Program.rmCol[Program.RMcount];

            //1 size
            //ListViewItem.ListViewSubItem sitem = new ListViewItem.ListViewSubItem();

            //format file size was used so that we can have some sensible file size... like 34GB and not 35235525! :)
            string filesize;
            filesize = Program.rmCol[Program.RMcount].FormatFileSize(Program.rmCol[Program.RMcount].currentTorrentFile.totalLength);

            listMain.Items[Program.RMcount].SubItems[1].Text = filesize;
            //listMain.Items[Program.RMcount].SubItems[1].Text = Program.rmCol[Program.RMcount].currentTorrent.totalsize.ToString();
            //item.SubItems.Add(sitem);

            //2 done
            //sitem = new ListViewItem.ListViewSubItem();
            listMain.Items[Program.RMcount].SubItems[2].Text = Program.rmCol[Program.RMcount].strFileSizeText;
            //item.SubItems.Add(sitem);

            /*
            //3 status
            sitem = new ListViewItem.ListViewSubItem();
            sitem.Text = "Started";
            item.SubItems.Add(sitem);
            */
            /*
            //4 seeds
            sitem = new ListViewItem.ListViewSubItem();
            sitem.Text = "...";
            item.SubItems.Add(sitem);
            */
            /*
            //5 peers
            sitem = new ListViewItem.ListViewSubItem();
            sitem.Text = "...";
            item.SubItems.Add(sitem);
            */

            //6 download speed
            //sitem = new ListViewItem.ListViewSubItem();
            //float result;
            //result = Program.rmCol[Program.RMcount].currentTorrent.downloadRate / 1024f;
            //listMain.Items[Program.RMcount].SubItems[6].Text = result.ToString() + "kB/s";
            //item.SubItems.Add(sitem);

            //7 update speed
            //sitem = new ListViewItem.ListViewSubItem();
            //result = Program.rmCol[Program.RMcount].currentTorrent.uploadRate / 1024f;
            //listMain.Items[Program.RMcount].SubItems[7].Text = result.ToString() + "kB/s";
            //item.SubItems.Add(sitem);

            //8 uploaded
            //sitem = new ListViewItem.ListViewSubItem();
            listMain.Items[Program.RMcount].SubItems[8].Text = Program.rmCol[Program.RMcount].currentTorrent.uploaded.ToString();
            //item.SubItems.Add(sitem);

            //9 downloaded
            //sitem = new ListViewItem.ListViewSubItem();
            listMain.Items[Program.RMcount].SubItems[9].Text = Program.rmCol[Program.RMcount].currentTorrent.downloaded.ToString();
            //item.SubItems.Add(sitem);

            //10 ratio
            //sitem = new ListViewItem.ListViewSubItem();
            if (!(Program.rmCol[Program.RMcount].currentTorrent.downloaded == 0))
            {
                long ratio = (Program.rmCol[Program.RMcount].currentTorrent.uploaded / Program.rmCol[Program.RMcount].currentTorrent.downloaded);
                listMain.Items[Program.RMcount].SubItems[10].Text = ratio.ToString();
            }
            else
                listMain.Items[Program.RMcount].SubItems[9].Text = "...";
            //item.SubItems.Add(sitem);

            /*
            //11 total running time
            sitem = new ListViewItem.ListViewSubItem();
            sitem.Text = "...";
            item.SubItems.Add(sitem);
            */

            /*
            //12 remaining time/up data amount/ or whatever conditions status to stop seeding...
            sitem = new ListViewItem.ListViewSubItem();
            sitem.Text = "...";
            item.SubItems.Add(sitem);
            */

            /*
            //13 Next update in
            sitem = new ListViewItem.ListViewSubItem();
            sitem.Text = "...";
            item.SubItems.Add(sitem);
            */
             
            //MessageBox.Show(item.Index.ToString());

            //this.listMain.Items.Add(item);
            //this.listMain.Items[Program.RMcount] = item;
        }

        private void stopAllRMs()
        {
            foreach(TorrentSession ratiomaster in Program.rmCol)
            {
                ratiomaster.Stop();
            }
        }

        //this enables the method to call it self... in the end
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void updateStatsCallback(int pitemNum, int psubItemNum, string ptext);

        //update the listMain component with the info sent...
        public void updateStats(int pitemNum, int psubItemNum, string ptext)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listMain.InvokeRequired)
            {
                updateStatsCallback d = new updateStatsCallback(updateStats);
                this.Invoke(d, new object[] { pitemNum, psubItemNum, ptext });
            }
            else
            {
                listMain.Items[pitemNum].SubItems[psubItemNum].Text = ptext;
            }
            
            //if (!(listMain.Items.Count == 0))
            //{
               // listMain.Items[pitemNum].SubItems[psubItemNum].Text = ptext;
            //}

        }

        private void exitApp()
        {
            //get confirmation
            DialogResult = MessageBox.Show("Exit rm2?", Program.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DialogResult == DialogResult.Yes)//ok the user is sure that he want to quit RB
            {
                //let the event handalar at the form closing know that we want him to close... and
                //not to recall us
                expectAppClosing = true;

                //first let's stop all the RM objects running there process so 
                //the torrents will be stopped before quiting...
                labStatus.Text = "Stopping All the Torrents...";
                stopAllRMs();
                labStatus.Text = "Exiting";

                notifyIcon1.Visible = false; // thanks arialtm for the fix

                Application.Exit();
            }
        }

        /* ------------ Events ----------------------------*/
        public MainUI()
        {
            InitializeComponent();
            //Program.tempConUI =  new RatioMaster.Form1();

            Program.mainUI = this;
        }

        private void newRMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newRM();
            
        }

        //add a new torrent
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newRM();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exitApp();
        }

        private void listMain_SelectedIndexChanged(object sender, EventArgs e)
        {

                if (listMain.SelectedItems.Count != 0)
                {
                    //check whether the selecte items -> RM is inillizited or not?
                    if (Program.rmCol[listMain.SelectedItems[0].Index].isInitillized == true)
                    {//it is initillized so do this stuff

                        toolStripButton2.Enabled = true;
                        removeRMToolStripMenuItem.Enabled = true;

                        //enable the mainUI's tabs...
                        //load the log in to the mainUI's log tab.. ;)
                        loadRMsLog(listMain.SelectedItems[0].Index);

                        selectedItem = listMain.SelectedItems[0].Index;

                        //fill in the info tab's data
                        loadInfoTab(listMain.SelectedItems[0].Index);
                     }
                }
                else
                {
                    toolStripButton2.Enabled = false;
                    removeRMToolStripMenuItem.Enabled = false;
                    
                    //clear the selected item variable
                    selectedItem = 9999;
                }
            
            
        }

        //remove torrent menu item
        private void removeRMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeRM(listMain.SelectedItems[0].Index, false); 
        }

        //remove torrent tool button
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            removeRM(listMain.SelectedItems[0].Index, false); 
        }

        private void clsFMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (expectAppClosing == false)// have exit app bean called? already..
            {
                //first clancel the closing, and call the exitApp() so that RB will exit in the correct way
                e.Cancel = true;
                exitApp();
            }
            
        }

        //Update some genaral stats in the mainUI...
        private void timStatus_Tick(object sender, EventArgs e)
        {
            //update the number of RM objects alive...
            toolStripStatusLabel2.Text = Program.rmCol.Count.ToString();

            //check to see wether we have to update the logs displayed in the mainUI
            if (selectedItem != 9999)
            {
                loadRMsLog(selectedItem);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == false)
            {
                this.Visible = true;
            }
            else
            {
                this.Visible = false;
            }
        }

        //show some info in a tool tip ballon kinda thing...
        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            //check whether the user wants tooltips or not :)
            if (Program.settings.ShowBallon == true)
            {

                string info = "";

                //int counter = 0;
                //for (int a = 0; a < Program.rmCol.Count; a++)
                foreach (TorrentSession currentRM in Program.rmCol)
                {

                    //show tooltip only if the RM object is in a started state....
                    if (currentRM.Started == true)
                    {
                        info += "RM #: " + currentRM.rmIndex + "|";
                        info += "Torrent: " + currentRM.currentTorrentFile.Name + "|";
                        info += "Tracker: " + currentRM.currentTorrent.tracker + "|";
                        info += "Uploaded: " + currentRM.strUploadCount + "\n";
                    }
                    //counter++;
                }

                //finally add the number of rm objects active...
                info += "\n # RM Objects: " + Program.rmCol.Count.ToString();

                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipTitle = "rm2 Stats";
                //notifyIcon1.BalloonTipText = "# RM Objects: " + Program.rmCol.Count.ToString();
                notifyIcon1.BalloonTipText = info;
                notifyIcon1.ShowBalloonTip(500);

            }
        }

        private void clsFMain_Load(object sender, EventArgs e)
        {
            //init misc variables in Program
            Program.localIP = getLocalIP();
            
            //set the form's caption text with the version number of the program
            this.Text = Program.APPLICATION_NAME + " " + Program.version;

            if (Program.settings.CheckVersion)
            {
                // check for new version
                // TODO need to make a checkbox for this -- done, phis
                VersionChecker versionChecker = new VersionChecker();
                versionChecker.CheckNewVersion(); 
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //show the about form UI
            AboutUI aboutDiag = new AboutUI();
            aboutDiag.Show();

#if !NO_SKINNABLE
            this.WindowState = FormWindowState.Minimized;//this is avoid skin conflicts with the about window...
#endif
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //minimize to system tray
            this.Hide();
        }

        private void conMmainList_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                //set the last selected item variable
                lastSelItem = listMain.SelectedItems[0].Index;

                if (listMain.SelectedItems.Count == 0)
                {
                    e.Cancel = true;

                }
                else
                {
                    //check whether the selecte items -> RM is inillizited or not?
                    if (Program.rmCol[listMain.SelectedItems[0].Index].isInitillized == true)
                    {//it is initillized so do this stuff

                        //check whetehr the RM object is started or stoped, and enable or disable menus
                        if (Program.rmCol[listMain.SelectedItems[0].Index].Started)//if the current torrent is started
                        {
                            stopToolStripMenuItem.Enabled = true;
                            startToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            stopToolStripMenuItem.Enabled = false;
                            startToolStripMenuItem.Enabled = true;

                        }
                        ///////////////


                        //ignore tracker interval updates or not menue text....
                        //check the value of the variable and change it as needed
                        if (Program.rmCol[listMain.SelectedItems[0].Index].boolIntervalOveride == true)
                        {
                            //Program.rmCol[listMain.SelectedItems[0].Index].boolIntervalOveride = false;
                            this.ignoreIntervalUpdatesToolStripMenuItem.Text = "Do Not Ignore Interval Updates";

                        }
                        else
                        {
                            //Program.rmCol[listMain.SelectedItems[0].Index].boolIntervalOveride = true;
                            ignoreIntervalUpdatesToolStripMenuItem.Text = "Ignore Interval Updates";
                        }

                        /////////////////
                        //this about enabling or disabling random up/down speeds

                        //dis/enable random download
                        if (Program.rmCol[listMain.SelectedItems[0].Index].boolRandomDownload == true)
                        {
                            //Program.rmCol[listMain.SelectedItems[0].Index].boolRandomDownload = false;
                            disableRandomDownToolStripMenuItem.Text = "Disable Random Down";
                        }
                        else
                        {
                            //Program.rmCol[listMain.SelectedItems[0].Index].boolRandomDownload = true;
                            disableRandomDownToolStripMenuItem.Text = "Enable Random Down";
                        }

                        //-------

                        //dis/enable random upload
                        if (Program.rmCol[listMain.SelectedItems[0].Index].boolRandomUpload == true)
                        {
                            //Program.rmCol[listMain.SelectedItems[0].Index].boolRandomUpload = false;
                            disableRandomUpToolStripMenuItem.Text = "Disable Random Up";
                        }
                        else
                        {
                            //Program.rmCol[listMain.SelectedItems[0].Index].boolRandomUpload = true;
                            disableRandomUpToolStripMenuItem.Text = "Enable Random Up";
                        }
                        ////////////////////
                    }
                    else
                    {
                        //so the selected RM object is not initillized
                        //cancel the menu
                        e.Cancel = true;
                    }
                }
            }
            catch (ArgumentOutOfRangeException) //this is expected at some time... when nothing is selected
            {
                e.Cancel = true;
            }
        }

        private void removeItem_Click(object sender, EventArgs e)
        {
            //remove the selected torrent and STOP it
            this.removeRM(listMain.SelectedItems[0].Index, false);

        }

        private void crashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //"crash" - just remove and clear everything but will not send stop to tracker...
            this.removeRM(listMain.SelectedItems[0].Index , true);
        }

        private void fromToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void updateNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //manual update the tracker
            int index = listMain.SelectedItems[0].Index;
            Program.rmCol[index].manualUpdate();
        }

        private void randomizeNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //randomize the speeds with out waiting for the next update to happen...
            int index = listMain.SelectedItems[0].Index;
            Program.rmCol[index].randomiseSpeeds();
        }

        //private void skinToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    //change the app skin
        //    //show the open file dialog box
        //    openFileDialog1.ShowDialog();

        //}

//        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
//        {
//            //file was selected and is ok so lets set the skin file variable to this file
//            //---------load some properties -------------

            
//            try
//            {
//                RM2Settings settings;
//                settings = new RM2Settings();

//                // Create the file and clean up handles.
//                //using (FileStream fs = File.Create(path)) { }

//                // Ensure that the target does not exist.
//                File.Delete(Application.StartupPath + "\\Skins\\new.skf");

//                // Copy the file.
//                File.Copy(openFileDialog1.FileName, Application.StartupPath + "\\Skins\\new.skf");
//                //Console.WriteLine("{0} copied to {1}", path, path2);

//                // Try to copy the same file again, which should succeed.
//                //File.Copy(path, path2, true);
//                //Console.WriteLine("The second Copy operation succeeded, which was expected.");

//                settings.AppSkinPath = "\\Skins\\new.skf";

//                settings.Save();

//#if !NO_SKINNABLE
//                //SkinOb.LoadSkinFromFile(Application.StartupPath + "\\Skins\\Vista-style_ST.skf");
//                SkinOb.LoadSkinFromFile(Application.StartupPath + settings.AppSkinPath);
//                SkinOb.ApplySkin();
//#endif
//                //MessageBox.Show("Skin change will take place once you restart rm2", Program.programName,MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (IOException IOe)
//            {
//                MessageBox.Show(IOe.ToString(), "Error loading the Skin File");
//            }

//        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //stop the selected torrent

            if (Program.rmCol[listMain.SelectedItems[0].Index].Started)//if the current torrent is started
            {
                stopRM(listMain.SelectedItems[0].Index, false);
                stopToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //start the selcted torrent
            if (!Program.rmCol[listMain.SelectedItems[0].Index].Started)//if the current torrent is stopped
            {
                Program.rmCol[listMain.SelectedItems[0].Index].Start();
                stopToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;
            }
        }

        private void toolStripTextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //the user renamed the rm and pressed enter so lets rename the list entry for the last selected
                //rm

                if (e.KeyChar == (char)13)
                {
                    // Then Enter key was pressed
                    //change the name value
                    if ((toolStripTextBox7.Text != "") && (lastSelItem != 9999))
                    {
                        int index = lastSelItem; // use the last selected item as when the user click the text box
                        //the item looes the focus...

                        listMain.Items[index].SubItems[0].Text = toolStripTextBox7.Text;
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
            }
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

                if (e.KeyChar == (char)13)
                {
                    // Then Enter key was pressed
                    
                    int result;
                    if (int.TryParse(toolStripTextBox1.Text, out result) == true)
                    {
                        //set the download rate
                        if ((toolStripTextBox1.Text != "") && (lastSelItem != 9999))
                        {
                            int index = lastSelItem; // use the last selected item as when the user click the text box
                            //the item looes the focus...
                            Program.rmCol[index].setDownSpeed(toolStripTextBox1.Text);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                    }
                }
        }

        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

                if (e.KeyChar == (char)13)
                {
                    int result;
                    if (int.TryParse(toolStripTextBox2.Text, out result) == true)
                    {

                        // Then Enter key was pressed
                        //change the up speed
                        if ((toolStripTextBox2.Text != "") && (lastSelItem != 9999))
                        {
                            int index = lastSelItem; // use the last selected item as when the user click the text box
                            //the item looes the focus...
                            Program.rmCol[index].setUploadRate(toolStripTextBox2.Text);
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                    }
                    
                }
        }

        private void toolStripTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

                if (e.KeyChar == (char)13)
                {
                    int result;
                    if (int.TryParse(toolStripTextBox4.Text, out result) == true)
                    {
                        // Then Enter key was pressed
                        //change the random down min speed value
                        if ((toolStripTextBox4.Text != "") && (lastSelItem != 9999))
                        {
                            int index = lastSelItem; // use the last selected item as when the user click the text box
                            //the item looes the focus...
                            Program.rmCol[index].strRandomDownloadFrom = toolStripTextBox4.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                    }
                }
        }

        private void toolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

                if (e.KeyChar == (char)13)
                {
                    int result;
                    if (int.TryParse(toolStripTextBox3.Text, out result) == true)
                    {
                        // Then Enter key was pressed
                        //change the random down max speed value
                        if ((toolStripTextBox3.Text != "") && (lastSelItem != 9999))
                        {
                            int index = lastSelItem; // use the last selected item as when the user click the text box
                            //the item looes the focus...
                            Program.rmCol[index].strRandomDownloadTo = toolStripTextBox3.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                    }
                }
        }

        private void toolStripTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
                if (e.KeyChar == (char)13)
                {
                    int result;
                    if (int.TryParse(toolStripTextBox5.Text, out result) == true)
                    {
                        // Then Enter key was pressed
                        //change the random up min speed value
                        if ((toolStripTextBox5.Text != "") && (lastSelItem != 9999))
                        {
                            int index = lastSelItem; // use the last selected item as when the user click the text box
                            //the item looes the focus...
                            Program.rmCol[index].strRandomUploadFrom = toolStripTextBox5.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                    }
                }

        }

        private void toolStripTextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

                if (e.KeyChar == (char)13)
                {
                    int result;
                    if (int.TryParse(toolStripTextBox6.Text, out result) == true)
                    {
                        // Then Enter key was pressed
                        //change the random up max speed value
                        if ((toolStripTextBox6.Text != "") && (lastSelItem != 9999))
                        {
                            int index = lastSelItem; // use the last selected item as when the user click the text box
                            //the item looes the focus...

                            Program.rmCol[index].strRandomUploadTo = toolStripTextBox6.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                    }
                }
        }

        private void ignoreIntervalUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ignore tracker's req to change the update interval
            //check the value of the variable and change it as needed
            if (Program.rmCol[listMain.SelectedItems[0].Index].boolIntervalOveride == true)
            {
                Program.rmCol[listMain.SelectedItems[0].Index].boolIntervalOveride = false;
                this.ignoreIntervalUpdatesToolStripMenuItem.Text = "Ignore Interval Updates";
                
            }
            else
            {
                Program.rmCol[listMain.SelectedItems[0].Index].boolIntervalOveride = true;
                ignoreIntervalUpdatesToolStripMenuItem.Text = "Do Not Ignore Interval Updates";
            }

        }

        private void toolStripTextBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //change the interval of the selected RM instence...
                //check the value of the variable and change it as needed
                if (e.KeyChar == (char)13)
                {
                    // Then Enter key was pressed
                    int result;
                    if (int.TryParse(toolStripTextBox9.Text,out result) == true)
                    {
                        Program.rmCol[listMain.SelectedItems[0].Index].currentTorrent.interval = result;
                        Program.rmCol[listMain.SelectedItems[0].Index].AddLogLine("Updating Interval(Manual): " + result);
                        Program.rmCol[listMain.SelectedItems[0].Index].temporaryIntervalCounter = 5;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                    }
                }
            }catch(FormatException)
            {
                MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
            }
        }

        private void saveACompleteLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //save a log of all the RM object's logs.... puls memreader + application settings log... etc etc
            saveLog();
        }

        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //save a log the selected RM object only
            saveLog(listMain.SelectedItems[0].Index);
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear the selected RM's log
            Program.rmCol[listMain.SelectedItems[0].Index].strBigLog = "";
            MessageBox.Show("Log Cleared For RM object: " + listMain.SelectedItems[0].Index + " " + Program.rmCol[listMain.SelectedItems[0].Index].currentTorrentFile.Name, Program.APPLICATION_NAME);
        }

        private void celarAllLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////clear all of the logs
            //Logger.appErrLog = ""; //clear app logs
            //Logger.memErrLog = ""; //clear mem logs

            //foreach (TorrentSession rmIns in Program.rmCol)
            //{
            //    rmIns.strBigLog = "";
            //}

            if (Logger.Clear())
            {
                MessageBox.Show("All Logs Cleared!", Program.APPLICATION_NAME); 
            }

        }

        private void disableRandomUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dis/enable random upload
            if (Program.rmCol[listMain.SelectedItems[0].Index].boolRandomUpload == true)
            {
                Program.rmCol[listMain.SelectedItems[0].Index].boolRandomUpload = false;
                disableRandomUpToolStripMenuItem.Text = "Enable Random Up";
            }
            else
            {
                Program.rmCol[listMain.SelectedItems[0].Index].boolRandomUpload = true;
                disableRandomUpToolStripMenuItem.Text = "Disable Random Up";
            }
        }

        private void disableRandomDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dis/enable random download
            if (Program.rmCol[listMain.SelectedItems[0].Index].boolRandomDownload == true)
            {
                Program.rmCol[listMain.SelectedItems[0].Index].boolRandomDownload = false;
                disableRandomDownToolStripMenuItem.Text = "Enable Random Down";
            }
            else
            {
                Program.rmCol[listMain.SelectedItems[0].Index].boolRandomDownload = true;
                disableRandomDownToolStripMenuItem.Text = "Disable Random Down";
            }
        }

        // Handling of Torrent File Drag and Drop for list
        #region DragAndDropForList
        private void listMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (s == null)
            {
                s = (string[])e.Data.GetData("System.String[]", true);
                if (s == null)
                {
                    return;
                }
            }
            //oadTorrentFileInfo(s[0], true);
            newRM(s[0]);
        }

        private void listMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetFormats().ToString().Equals("System.String[]"))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        // Handlgin of control text updates from different threads
        // general function for updating 
        #region ControlTextUpdates
    
        delegate void updateTextBoxCallback(TextBox textbox, string text);
        private void updateTextBox(TextBox textbox, string text)
        {
            if (textbox.InvokeRequired)
            {
                updateTextBoxCallback d = new updateTextBoxCallback(updateTextBox);
                this.Invoke(d, new object[] { textbox, text });
            }
            else
            {
                textbox.Text = text;
            }
        }

        delegate void SetTextCallback(string logLine);

        // update general log control
        public void AddLogLine(string logLine)
        {
            if (rtxtGeneralLogBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AddLogLine);
                this.Invoke(d, new object[] { logLine });
            }
            else
            {
                //if (checkLogEnabled.Checked)
                //{
                    try
                    {
                        DateTime dtNow = DateTime.Now;
                        string dateString = "[" + String.Format("{0:hh:mm:ss}", dtNow) + "]";
                        rtxtGeneralLogBox.AppendText(dateString + " " + logLine + "\r\n");
                        rtxtGeneralLogBox.ScrollToCaret();
                    }
                    catch (Exception) { }
                //}
            }
        }
        #endregion

        // Getting local IP and putting it into localIP in Programs
        // this should be done on application start
        public string getLocalIP()
        {
            string localIP = "";
            string hostName = Dns.GetHostName();
            this.AddLogLine("Host Name = " + hostName);
            foreach (IPAddress address in Dns.GetHostEntry(hostName).AddressList)
            {
                this.AddLogLine("IPAddress = " + address.ToString());
                localIP = address.ToString();
            }
            return localIP;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsUI options = new OptionsUI();
            options.Show();
        }

    }
}