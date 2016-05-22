using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTI.DataBase.Application.Controllers
{
    /// <summary>
    /// Custom read/write to console.
    /// utility class 
    /// </summary>
    static class UserInterface
    {
        private static StringBuilder UiString = new StringBuilder();
        private static int LineNumber = 0;
        private static string OutputText { get; set; }
        /// <summary>
        /// Writes to the console and records a log 
        /// of visible output that can be retrieved by 
        /// calling the "GetCurrentOurput()" method. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void WriteToConsole(string message = "", object arg0 = null, object arg1 = null, object arg2 = null, object arg3 = null, object arg4 = null, object arg5 = null)
        {
            if (message.Length > 0)
            {
                Console.Write(message, arg0, arg1, arg2, arg3, arg4, arg5);
                UiString.AppendFormat(message, arg0, arg1, arg2, arg3, arg4, arg5);
                LineNumber += UiString.ToString().Split('\n').Length;
            }
        }

        /// <summary>
        /// Reads user input from the console window.
        /// </summary>
        /// <param name="readkey"></param>
        /// <returns>Input String</returns>
        public static string ReadFromConsole(bool readkey = false)
        {
            if (!readkey)
            {
                string input = Console.ReadLine();
                return input;
            }
            else
            {
                var input = Console.ReadKey();
                return input.KeyChar.ToString();
            }
        }

        /// <summary>
        /// Returns the number of lines of text
        /// currently written to the console. 
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfLines()
        {
            return LineNumber;
        }

        /// <summary>
        /// Returns a string containing all of the current 
        /// text written to the console window. 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentOutput(int numberOfLinesToRemove = 0)
        {
            // Remove x number of lines from the output
            if (numberOfLinesToRemove > 0)
            {
                var UiArray = Convert.ToString(UiString).Split('\n');
                int totalNumberOfLines = UiArray.Count();
                int StartingPosition = 0;
                int lineNumber = 1;
                foreach(var line in UiArray)
                {
                    if(lineNumber <= (totalNumberOfLines - numberOfLinesToRemove))
                        StartingPosition += line.Length;
                    lineNumber++;
                }
                StartingPosition += (lineNumber-3);

                // Ensure index always remains positive. 
                StartingPosition = StartingPosition < 0 ? 0 : StartingPosition;

                int numCharToRemove = UiArray.Last().Length;
                numCharToRemove = numCharToRemove <= 0 ? -1 : numCharToRemove;
                string output = UiString.Remove(StartingPosition, numCharToRemove+1).ToString();
                return output;
            }
            return UiString.ToString();
        }

        /// <summary>
        /// Clear the Console Window and reset 
        /// the LineNumber count. 
        /// </summary>
        public static void ClearUI()
        {
            Console.Clear();
            UiString.Clear();
            LineNumber = 0;
        }
    }
}
