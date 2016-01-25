using System;
using System.IO;

namespace RTI.DataBase.Application.Controllers
{
    /// <summary>
    /// Custom read/write to console.
    /// utility class 
    /// </summary>
    static class UserInterface
    {
        public static string OutputText { get; set; }
        private static StringWriter ConsoleStringWriter = new StringWriter();
        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void WriteToConsole(string message = "", object arg0 = null, object arg1 = null, object arg2 = null, object arg3 = null)
        {
            using (ConsoleStringWriter)
            {
                Console.SetOut(ConsoleStringWriter);

                if (message.Length > 0)
                {
                    Console.WriteLine(message, arg0, arg1, arg2, arg3);
                }
                OutputText = ConsoleStringWriter.ToString();
            }

        }

        /// <summary>
        /// Reads user input from console. 
        /// </summary>
        /// <returns></returns>
        public static string ReadFromConsole()
        {
            string input = Console.ReadLine();
            return input;
        }
    }
}
