using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Logger
{
    public enum MessageLevel { Information, Warning, Error }

    /// <summary>
    /// Logger class,
    /// - multithread safe
    /// - generated in exe folder
    /// </summary>
    public class Log
    {

        private const string LOG_NAME = "\\Log.txt";

        // max size of log file - rename previous file if needed
        private const long MAX_SIZE = 1000000;

        // queue of messages to write in file
        private static Queue<LogMessage> m_Messages = null;

        // thread that write message form queue to file
        private static Thread m_ThreadWriting = null;

        // keep thread of writing running
        private static bool m_running = false;

        // name of log file complete path - log
        private static string m_fileName = string.Empty;


        static Log()
        {
            try
            {
                // *** variable initialization ***
                m_Messages = new Queue<LogMessage>();

                m_fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MonitoringSite") + LOG_NAME;

                if (!Directory.Exists(Path.GetDirectoryName(m_fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(m_fileName));

                if (File.Exists(m_fileName))
                    RemaneFile();

                // *** starting of writting thread log ***
                m_ThreadWriting = new Thread(Writtring)
                {
                    Priority = ThreadPriority.BelowNormal,
                    Name = "Logger Thread",
                    IsBackground = true
                };
                m_ThreadWriting.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        public static MessageLevel LogLevel { get; set; }

        public static void DisposeInstance()
        {
            try
            {
                // *** stop thread of writting ***
                m_running = false;

                if (m_ThreadWriting != null)
                {
                    m_ThreadWriting.Abort();
                    m_ThreadWriting.Join();
                    m_ThreadWriting = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Function that write the message from queue to file
        /// check the log file size, rename old log and create new if needed
        /// </summary>
        private static void Writtring()
        {
            // ***  check file size ***
            FileInfo fi = new FileInfo(m_fileName);

            if (!fi.Exists)
            {
                CreateLog();
                fi = new FileInfo(m_fileName);
            }

            m_running = true;

            while (m_running)
            {
                try
                {

                    if (fi.Length > MAX_SIZE)
                    {
                        RemaneFile();
                    }

                    // ***  writing of all message in waiting ***
                    if (m_Messages.Count > 0)
                    {

                        while (m_Messages.Count > 0)
                        {
                            LogMessage message = m_Messages.Dequeue();

                            if ((int)message.Level >= (int)LogLevel)
                            {
                                // string messageStr = message.ToString();
                                using (StreamWriter sw = new StreamWriter(m_fileName, true))
                                {
                                    sw.WriteLine(message);
                                }
                            }
                        }
                    }

                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// rename current file log xxxxx to xxxxx.old
        /// delete previous xxxxx.old if existing
        /// </summary>
        private static void RemaneFile()
        {
            try
            {
                string backupFile = m_fileName + ".old";

                if (File.Exists(backupFile))
                    File.Delete(backupFile);

                File.Move(m_fileName, backupFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        /// <summary>
        /// creation of log file and write header
        /// </summary>
        private static void CreateLog()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(m_fileName, false))
                {
                    sw.WriteLine(LogMessage.GetHeader());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Add normal message to queue message log
        /// insert current time to log message
        /// </summary>
        /// <param name="message">message to write</param>
        public static void WriteInformation(string message)
        {
            m_Messages.Enqueue(new LogMessage(DateTime.Now, MessageLevel.Information, message));
        }

        /// <summary>
        /// Add normal message to queue message log
        /// insert current time to log message
        /// </summary>
        /// <param name="message">message to write</param>
        public static void WriteWarning(string message)
        {
            m_Messages.Enqueue(new LogMessage(DateTime.Now, MessageLevel.Warning, message));
        }

        /// <summary>
        /// for exection, add Message and stackTrace to log 
        /// </summary>
        /// <param name="ex">exception to log</param>
        public static void WriteError(Exception ex, [CallerMemberName] String propertyName = "",  [CallerFilePath] String fileName = "")
        {
            m_Messages.Enqueue(new LogMessage(DateTime.Now, MessageLevel.Error, ex.Message + Environment.NewLine + "FileName : " + fileName + " -- Caller : " + propertyName +
               Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace ));
        }

        /// <summary>
        /// for exection, add Message and stackTrace to log 
        /// </summary>
        /// <param name="ex">exception to log</param>
        public static void WriteError(string message)
        {
            m_Messages.Enqueue(new LogMessage(DateTime.Now, MessageLevel.Error, message));
        }

    }
}
