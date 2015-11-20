using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTI.DataBase.Application.Controllers
{
    class MenuController
    {
        // Asks the user if they want to start running the AutoUpdater
        public static void startApplication(string start)
        {
            Console.Clear();
            if (start == "n" || start == "N") // Case No
            {
                UserInterface.WriteToConsole("Do you want to exit?  y/n");
                exitApplication(UserInterface.ReadFromConsole());
            }
            else if (start == "y" || start == "Y") // Case Yes
            {
                UserInterface.WriteToConsole("\n\nPress the 'Esc' key at any time to exit.");
                runNewUpdater(); // Start the updater 
            }
            else // Case Error
            {
                UserInterface.WriteToConsole("Error: Unkonwn input string. Please Try Again.");
                UserInterface.WriteToConsole("\nWould you like to run the RTI database updater? y/n");
                startApplication(UserInterface.ReadFromConsole());
            }
        }

        // Asks the user if they want to exit the AutoUpdater
        public static void exitApplication(string exit)
        {
            if (exit == "y" || exit == "Y") // Case Yes
            {
                UserInterface.WriteToConsole("Goodbye.");
                System.Environment.Exit(0);
            }
            else if (exit == "n" || exit == "N") // Case No 
            {
                UserInterface.WriteToConsole("Would you like to run the RTI database updater? y/n");
                startApplication(UserInterface.ReadFromConsole());
            }
            else // Case Error
            {
                UserInterface.WriteToConsole("Error: Unkonwn input string. Please Try Again.");
                UserInterface.WriteToConsole("\nDo you want to exit?  y/n");
                exitApplication(UserInterface.ReadFromConsole()); // Recursive call to exit
            }
        }

        // Runs the AutoUpdater application 
        public static void runNewUpdater()
        {
            try
            {
                FileFetcher fetcherProcess = new FileFetcher();
                Thread fetcherThread = new Thread(() => fetcherProcess.fetchFile());
                fetcherThread.Start(); // Start the File Fetcher Thread
                fetcherProcess.Start();
                while (!fetcherThread.IsAlive) ; // Hault untill Thread becomes Active 

                // Check for User Process Cancelation 
                while (!breakCurrentOperation(fetcherProcess));

                // Cancle the Process
                fetcherProcess.Stop();

                // Notify the User
                UserInterface.WriteToConsole("Operation Cancled...");


                // Ask to Re-Start the applicatoin 
                UserInterface.WriteToConsole("\nWould you like to re-start the RTI database updater? y/n");
                startApplication(UserInterface.ReadFromConsole());
            }
            catch(Exception error)
            {
                UserInterface.WriteToConsole("The updater application has encountered a fatal error.\nPlease view the log file for more details." );
            }
        }

        // Pauses the FileFetcher thread
        public static bool breakCurrentOperation(FileFetcher fetcherProcess)
        {
            var consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.Escape)
                {
                    fetcherProcess.Pause(); // Pause
                    UserInterface.WriteToConsole("Do you want to stop the current process? \nType s to stop or c to continue.");
                    string input = Console.ReadLine();
                    if (input == "c" || input == "C")
                    {
                        fetcherProcess.Pause(); // Unpause
                        return false; // Continue 
                    }
                    else if (input == "s" || input == "S")
                    {
                        return true; // Break the loop
                    }
                    else
                    {
                        UserInterface.WriteToConsole("Error: Input was not recognized, the current process will now continue. Press Esc to stop the operation.");
                        fetcherProcess.Pause(); // Unpause
                    }
                }
            return false;
        }
    }
}

