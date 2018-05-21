using System;
using System.Text;
using static Logger.Log;

namespace Logger
{
    /// <summary>
    /// Line of log file
    ///  - ToString() function is used to format the line for the file
    ///  - GetHeader() function that return a line for name header of columns 
    /// </summary>
    internal class LogMessage
    {
        // size in char of date time column
        private const int SIZE_DATETIME = 25;

        // siez in char of message level column
        private const int SIZE_LEVEL = 15;

        // Date of the message
        private DateTime Date { get; set; }

        // level of the message
        internal MessageLevel Level { get; private set; }

        // message to add at log file
        private string Message { get; set; }


        public LogMessage()
        {
            Date = default(DateTime);
            Level = MessageLevel.Information;
            Message = string.Empty;
        }

        public LogMessage(DateTime date, MessageLevel level, string message)
        {
            Date = date;
            Level = level;
            Message = message;
        }

        /// <summary>
        /// return the header of log file, use size of datetime in order to
        /// keep columns align
        /// </summary>
        /// <returns></returns>
        public static string GetHeader()
        {
            return String.Format("{0,-" + SIZE_DATETIME + "}", "Date Time")+ ";" + String.Format("{0,-" + SIZE_LEVEL + "}", "Level") + ";Message";
        }

        /// <summary>
        /// return a string of the line that fit to log file
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            Message = Message.Replace(Environment.NewLine, Environment.NewLine + new string(' ', SIZE_DATETIME) + ";" + new string(' ', SIZE_LEVEL) + ";") ;

            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("{0,-" + SIZE_DATETIME + "};", Date.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            sb.Append(String.Format("{0,-" + SIZE_LEVEL + "};",Level.ToString()));
            sb.Append(Message);

            return sb.ToString();
        }
    }
}
