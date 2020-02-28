namespace rm2
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    internal class VersionChecker
    {
        private const string VERSION_CHECK_URL = "http://www.moofdev.org/vercheck.php";

        private void checkNewVersion()
        {
            try
            {
                int version = Program.internalVersion;
                Program.mainUI.AddLogLine("Local Version: " + version.ToString());
                Program.mainUI.AddLogLine("Checking for new version...");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(VERSION_CHECK_URL);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string s = reader.ReadToEnd();
                Program.mainUI.AddLogLine("Remote Version: " + s);
                if (int.Parse(s) > version)
                {
                    MessageBox.Show("New version released! Get it from http://www.moofdev.org", "New version available");
                }
                response.Close();
                reader.Dispose();
            }
            catch (Exception ex)
            {
                Program.mainUI.AddLogLine("Error checking new version : " + ex.Message);
            }
        }

        public void CheckNewVersion()
        {
            Thread thread = new Thread(new ThreadStart(this.checkNewVersion));
            thread.Name = "_checkNewVersion() Thread";
            thread.Start();
        }

    }
}

