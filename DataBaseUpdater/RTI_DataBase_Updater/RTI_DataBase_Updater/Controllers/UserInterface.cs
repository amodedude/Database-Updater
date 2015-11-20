using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI.DataBase.Application.Controllers
{
    /// <summary>
    /// Custom read/write to console.
    /// utility class 
    /// </summary>
    class UserInterface
    {
        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="message"></param>
        public static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
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
