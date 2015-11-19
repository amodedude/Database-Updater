using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTI_DataBase_Updater.Controllers
{
    class MenuController
    {
        // Asks the user if they want to start running the AutoUpdater
        public static void startApplication(string start)
        {
            if (start == "n" || start == "N") // Case No
            {
                UserInterfaceController.WriteToConsole("Do you want to exit?  y/n");
                exitApplication(UserInterfaceController.ReadFromConsole());
            }
            else if (start == "y" || start == "Y") // Case Yes
            {
                Console.WriteLine("Initializing RTI updater...");
                Console.WriteLine("Press the 'Esc' key at any time to exit.");
                runNewUpdater(); // Start the updater 
            }
            else // Case Error
            {
                UserInterfaceController.WriteToConsole("Error: Unkonwn input string. Please Try Again.");
                UserInterfaceController.WriteToConsole("\nWould you like to run the RTI database updater? y/n");
                startApplication(UserInterfaceController.ReadFromConsole());
            }
        }

        // Asks the user if they want to exit the AutoUpdater
        public static void exitApplication(string exit)
        {
            if (exit == "y" || exit == "Y") // Case Yes
            {
                UserInterfaceController.WriteToConsole("Goodbye.");
                System.Environment.Exit(0);
            }
            else if (exit == "n" || exit == "N") // Case No 
            {
                UserInterfaceController.WriteToConsole("Would you like to run the RTI database updater? y/n");
                startApplication(UserInterfaceController.ReadFromConsole());
            }
            else // Case Error
            {
                UserInterfaceController.WriteToConsole("Error: Unkonwn input string. Please Try Again.");
                UserInterfaceController.WriteToConsole("\nDo you want to exit?  y/n");
                exitApplication(UserInterfaceController.ReadFromConsole()); // Recursive call to exit
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
                UserInterfaceController.WriteToConsole("Operation Cancled...");


                // Ask to Re-Start the applicatoin 
                UserInterfaceController.WriteToConsole("\nWould you like to re-start the RTI database updater? y/n");
                startApplication(UserInterfaceController.ReadFromConsole());
            }
            catch(Exception error)
            {
                UserInterfaceController.WriteToConsole("The updater application has encountered a fatal error.\nPlease view the log file for more details." );
            }
        }

        // Pauses the FileFetcher thread
        public static bool breakCurrentOperation(FileFetcher fetcherProcess)
        {
            var consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.Escape)
                {
                    fetcherProcess.Pause(); // Pause
                    UserInterfaceController.WriteToConsole("Do you want to stop the current process? \nType s to stop or c to continue.");
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
                        UserInterfaceController.WriteToConsole("Error: Input was not recognized, the current process will now continue. Press Esc to stop the operation.");
                        fetcherProcess.Pause(); // Unpause
                    }
                }
            return false;
        }
    }
}

