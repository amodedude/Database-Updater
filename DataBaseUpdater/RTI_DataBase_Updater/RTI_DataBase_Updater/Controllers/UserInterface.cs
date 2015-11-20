using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI.DataBase.Application.Controllers
{
    class UserInterface
    {
        // Writes to the console
        public static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }

        // Reads a user input from the console
        public static string ReadFromConsole()
        {
            string input = Console.ReadLine();
            return input;
        }
    }
}
