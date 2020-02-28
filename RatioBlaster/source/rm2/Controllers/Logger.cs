using System;
using System.Collections.Generic;
using System.Text;
using rm2;
using System.IO;

namespace RatioBlaster.rm2.Controllers
{
    /// <summary>
    /// This class handles all the logging
    /// </summary>
    public static class Logger
    {
        private const string NEW_LINE = "\n";
        //this is the private error log of application settings . cs
        private static StringBuilder appErrLog = new StringBuilder();

        //this is the private error log of memory reader class... memRead.cs
        private static StringBuilder memErrLog = new StringBuilder();

        public static void Log(string message, object messageSender)
        {
            if (messageSender.GetType() == typeof(MemReader))
            {
                memErrLog.Append(NEW_LINE + message);
                return;
            }

            if (messageSender.GetType() == typeof(ApplicationSettings))
            {
                appErrLog.Append(NEW_LINE + message);
            }
        }

        /// <summary>
        /// Clear all the logs stored
        /// </summary>
        /// <returns></returns>
        public static bool Clear()
        {
            appErrLog = new StringBuilder();
            memErrLog = new StringBuilder();

            foreach (TorrentSession rmIns in Program.rmCol)
            {
                rmIns.strBigLog = "";
            }

            return true;
        }

        //this saveLog method saves all of the logs of all of the RM instences... + mem reader + app settings loader...
        public static bool Dump(Stream stream)
        {
            //construct the string... with all of the logs together...
            string log;
            log = "\n ApplicationSettings.cs Log \n\n" + Logger.appErrLog;
            //if the log is empty then add a empy line
            if (appErrLog.Length == 0)
            {
                log += "~~~EMPTY~~~\n\n";
            }

            log += "\n memRead.cs Log \n\n" + Logger.memErrLog;

            //if the log is empty then add a empty line
            if (memErrLog.Length == 0)
            {
                log += "~~~EMPTY~~~\n\n";
            }

            foreach (TorrentSession rmIns in Program.rmCol)
            {
                log += "\n\n ====== RM Object:" + rmIns.rmIndex.ToString() + " : " + rmIns.currentTorrentFile.Name + "======\n\n" + rmIns.strBigLog;
            }


            return WriteLog(stream, log);
        }

        /// <summary>
        /// Write the given String to the Given Stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private static bool WriteLog(Stream stream, string log)
        {
            try
            {
                if (stream != null)
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(log);
                    writer.Close();
                    stream.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //this saveLog method only saves a log of one given RM instence...
        public static bool Dump(Stream stream, TorrentSession tSession)
        {

            //construct the string... with all of the logs together...
            string log;
            log = "\n ApplicationSettings.cs Log \n\n" + Logger.appErrLog;
            //if the log is empty then add a empy line
            if (Logger.appErrLog.Length == 0)
            {
                log += "~~~EMPTY~~~\n\n";
            }

            log += "\n memRead.cs Log \n\n" + Logger.memErrLog;

            //if the log is empty then add a empy line
            if (Logger.memErrLog.Length == 0)
            {
                log += "~~~EMPTY~~~\n\n";
            }

            log += "\n\n ====== RM Object : " + tSession.currentTorrentFile.Name + "======\n\n" + tSession.strBigLog;

            return WriteLog(stream, log);
        }

    }
}
