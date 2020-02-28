using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace rm2.UI
{
    public partial class OptionsUI : Form
    {
        RM2Settings settings;

        public OptionsUI()
        {
            InitializeComponent();
            settings = new RM2Settings();
            loadValues();

            //disalbe the skin change buttons and stuff
            grpbxSkin.Enabled = false;
        }

        private void loadValues()
        {
            chkScrlGLog.Checked = settings.AutoGenScrollLog;
            chkScrlLogRM.Checked = settings.AutoRMScrollLog;
            chkShowBallon.Checked = settings.ShowBallon;
            chkChkVer.Checked = settings.CheckVersion;

            txtMaxNoPeers.Text = settings.MaxNumOfPeersToShow.ToString();

            lblSkinPath.Text = settings.AppSkinPath;
        }

        private void btnChangeSkin_Click(object sender, EventArgs e)
        {
            //change the app skin
            //show the open file dialog box
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //file was selected and is ok so lets set the skin file variable to this file
            //---------load some properties -------------


            try
            {
                // Create the file and clean up handles.
                //using (FileStream fs = File.Create(path)) { }

                // Ensure that the target does not exist.
                File.Delete(Application.StartupPath + "\\Skins\\new.skf");

                // Copy the file.
                File.Copy(openFileDialog1.FileName, Application.StartupPath + "\\Skins\\new.skf");
                //Console.WriteLine("{0} copied to {1}", path, path2);

                // Try to copy the same file again, which should succeed.
                //File.Copy(path, path2, true);
                //Console.WriteLine("The second Copy operation succeeded, which was expected.");

                settings.AppSkinPath = "\\Skins\\new.skf";

                settings.Save();

#if !NO_SKINNABLE
                //SkinOb.LoadSkinFromFile(Application.StartupPath + "\\Skins\\Vista-style_ST.skf");
                SkinOb.LoadSkinFromFile(Application.StartupPath + settings.AppSkinPath);
                SkinOb.ApplySkin();
#endif
                //MessageBox.Show("Skin change will take place once you restart rm2", Program.programName,MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException IOe)
            {
                MessageBox.Show(IOe.ToString(), "Error loading the Skin File");
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //save the values
            if (chkShowBallon.Checked) { settings.ShowBallon = true; }
            else { settings.ShowBallon = false; }

            // Then Enter key was pressed
            if (txtMaxNoPeers.Text != "")
            {
                try
                {
                    settings.MaxNumOfPeersToShow = int.Parse(txtMaxNoPeers.Text);
                    //MessageBox.Show("MaxPeersToShow: " + Program.maxPeersToShow.ToString(), Program.programName);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a valid number", Program.APPLICATION_NAME);
                }
            }

            if (chkScrlGLog.Checked) 
            { 
                settings.AutoGenScrollLog = true;
            }
            else { settings.AutoGenScrollLog = false; }

            if (chkScrlLogRM.Checked) { settings.AutoRMScrollLog = true; }
            else { settings.AutoRMScrollLog = false; }

            if (chkChkVer.Checked) { settings.CheckVersion = true; }
            else { settings.CheckVersion = false; }

            settings.Save();
            this.Dispose(true);
        }


    }
}