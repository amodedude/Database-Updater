using RTI.DataBase.Application.Controllers;
using System;
using System.IO;
using System.Text;

namespace RTI.DataBase.Application.Logger
{
    static class ApplicationLog
    {

        /// <summary>
        /// Writes a message to the log file. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="includeDate"></param>
        public static void WriteMessageToLog(string message, bool includeDate, bool useTopSeporator, bool useBottomSeporator)
        {
                var fullPath = GetFullFilePath();
                string formatedMessage = GenerateFormatedString(message);

                if (VerifyLogFile(fullPath))
                    InsertTextToLog(formatedMessage, fullPath, includeDate, useTopSeporator, useBottomSeporator);
        }

        /// <summary>
        /// Formats the message string to appear exactly within
        /// the defined number of characters in the ApplicationLog 
        /// class' "fileWidth" parameter. 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string GenerateFormatedString(string message)
        {
            string formatedText = "";
            if (message.Length < fileWidth)
            {
                formatedText = message.PadRight(fileWidth);
            }
            else
            {
                int numCharsRead = -1;
                char[] buffer = new char[fileWidth];
                using (StringReader reader = new StringReader(message))
                {
                    StringBuilder builder = new StringBuilder();
                    while (reader.Peek() != (-1))
                    {
                        numCharsRead = reader.ReadBlock(buffer, 0, buffer.Length);
                        if (numCharsRead > 0)
                        {
                            builder.Append(buffer, 0, numCharsRead);
                            builder.Append(Environment.NewLine);
                        }
                    }
                    formatedText = builder.ToString();
                }
            }
            return formatedText;
        }

        /// <summary>
        /// Writes text to the log file. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="filePath"></param>
        /// <param name="includeDate"></param>
        private static void InsertTextToLog(string message, Uri filePath, bool includeDate, bool includeTopSeporator, bool includeBottomSeporator)
        {
            using (StreamWriter log = new StreamWriter(filePath.LocalPath, true))
            {
                if (includeTopSeporator)
                {
                    StringBuilder str = new StringBuilder();
                    for (int i = 0; i < fileWidth; i++)
                    {
                        str.Append("-");
                    }

                    str.Append(Environment.NewLine);
                    log.Write(str.ToString());
                }

                if (includeDate)
                {
                    DateTime messageDate = DateTime.Now;
                    string dateStamp = messageDate.ToString("MM/dd/yyyy, hh:mm tt");
                    log.Write("@ " + dateStamp + " " + Environment.UserName + " - " + Environment.NewLine + message + Environment.NewLine);
                }
                else
                {
                    log.Write(message + Environment.NewLine);
                }

                if (includeBottomSeporator)
                {
                    StringBuilder str = new StringBuilder();
                    for (int i = 0; i < fileWidth; i++)
                    {
                        str.Append("-");
                    }

                    str.Append(Environment.NewLine);
                    log.Write(str.ToString());
                }
            }
        }
        

        /// <summary>
        /// Adds a dividing line to seporate 
        /// </summary>
        public static void InsertLineSeporator()
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < fileWidth; i++)
            {
                str.Append("-");
            }

            str.Append("\n");

            var fullPath = GetFullFilePath();
            if (VerifyLogFile(fullPath))
                InsertTextToLog(str.ToString(), fullPath, false, false, false);
        }


        /// <summary>
        /// Verifies that the log file exists. 
        /// If not, this method creates it. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool VerifyLogFile(Uri path)
        {
            if(File.Exists(path.LocalPath))
            {
                return true;
            }
            else
            {
                File.Create(LogFileName).Close();
                if (File.Exists(path.LocalPath))
                    return true;
                else
                {
                    UserInterface.WriteToConsole("Warning, Unable to create log file in " + path.LocalPath + ".\nNo Logs will be written. \nPress any key to continue...");
                    return false;
                }
            }
        }

        private static Uri GetFullFilePath()
        {
            string LogFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string fullpath = LogFilePath + "\\" + LogFileName;
            return new Uri(fullpath);
        }

        /// <summary>
        /// Defines the log file name. 
        /// </summary>
        private const string LogFileName = "RTIDatabaseUpdaterLog.txt";

        /// <summary>
        /// Defines the log files width 
        /// in number of characters. 
        /// </summary>
        private const int fileWidth = 83;
    }
}
