using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using rm2.UI;


namespace rm2
{
    static class Program
    {
        #region Constants
        //version of the program
        public const String version = "0.16 beta";
        public const int internalVersion = 1990;

        //program name! :P
        public const String APPLICATION_NAME = "Ratio Master 2"; //lol :D easy to modify now isn't it? ;) 
        #endregion

        #region Static Variables
        ////max peers to show in the log, these info is retrived from the tracker anyways... only the displayed
        ////number is defined here
        //public static int maxPeersToShow = 5;

        /// <summary>
        /// User settings for RB2 is stored here
        /// </summary>
        internal static RM2Settings settings = new RM2Settings();

        //main form/UI
        public static MainUI mainUI;

        public static int RMcount = -1;

        //this one holds the current RM instence that should be associated with the RM UI.
        public static TorrentSession currentRM;

        public static rm2.UI.TorrerntSettingsUI tempConUI;




        //this is the RM collection! :)
        public static List<TorrentSession> rmCol = new List<TorrentSession>();

        // localIP
        public static string localIP;
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //---------load some properties -------------

                RM2Settings settings;
                settings = new RM2Settings();

                Application.Run(new MainUI());

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Main()");

                //ask the user whether we should save the error to a log?
                DialogResult result = MessageBox.Show("Save the error to a log file?", Program.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)//ok then save the log
                {
                    HandleAppRunTimeException(e);
                }
            }
        }

        /// <summary>
        /// This method will be called when the application's main thread throws a unhandled exception
        /// this method should take care of handling that, logging that and letting the user know about the exception
        /// </summary>
        /// <param name="e"></param>
        private static void HandleAppRunTimeException(Exception e)
        {
            try
            {
                //----- Let's Try to save the error to a log file --------
                Stream stream;
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
                dialog.FilterIndex = 2;
                dialog.RestoreDirectory = true;
                dialog.InitialDirectory = Application.StartupPath;

                //include some time date info in the file name
                DateTime now = DateTime.Now;
                string dateTime = now.Day + "." + now.Hour + "." + now.Minute + "." + now.Second;
                dialog.FileName = "rb_Main_error_" + dateTime + ".log";
                if ((dialog.ShowDialog() == DialogResult.OK) && ((stream = dialog.OpenFile()) != null))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(e.ToString());
                    writer.Close();
                    stream.Close();
                }

                DialogResult result2 = MessageBox.Show("Save other log files as well?", Program.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result2 == DialogResult.Yes)//ok then save the log
                {
                    Program.mainUI.saveLog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, error saving the log, Application is quitting :(", Program.APPLICATION_NAME);
            }
        }
    }
}