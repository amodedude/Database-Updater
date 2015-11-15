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
                runNewUpdater();
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
                UserInterfaceController.WriteToConsole("Goodbye...!");
                System.Environment.Exit(0);
            }
            else if (exit == "n" || exit == "N") // Case No 
            {
                UserInterfaceController.WriteToConsole("Okay, would you like to run the RTI database updater? y/n");
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
            FileFetcher Fetcher = new FileFetcher();
            while (true)
            {
                for (int step = 0; step <= int.MaxValue; step++)
                {
                    if (!breakCurrentOperation()) // Checks if the user has halted the current opperation by pressing ESC
                    {
                        
                        UserInterfaceController.WriteToConsole("\nDownloaded " + step.ToString() + " file(s) out of " + int.MaxValue.ToString());
                        
                        // Generate a USGS ID
                        Random rnd = new Random();
                        long USGSID = rnd.Next();
                        
                        // Fetch the file
                        Fetcher.download_file(USGSID);
                        
                        // Catch Overflow Exception 
                        if (step == int.MaxValue)
                        {
                            step = 0; // Re-set the loop counter
                        }
                    }
                    else
                    {
                        break;
                    }
                }


                if (!startNewOperation())
                {
                    // Noop
                }
                else
                {
                    break;
                }

            }

            UserInterfaceController.WriteToConsole("\nAre you ready to run the RTI database updater again? y/n");
            startApplication(UserInterfaceController.ReadFromConsole());
        }

        public static bool startNewOperation()
        {
            UserInterfaceController.WriteToConsole("Do you want go back to the main menu or start a new update process?  \nType y to start a new update process or n to go to the main menu.");
            string input = UserInterfaceController.ReadFromConsole();

            if (input == "y" || input == "Y")
            {
                return false;
            }
            else if (input == "n" || input == "N")
            {
                return true; // Noop - Restart the Loop from the begining
            }
            else
            {
                UserInterfaceController.WriteToConsole("Error: Input was not recognized. ");
                return startNewOperation(); // Recursivly call method untill user enters a logical input
            }
        }

        public static bool breakCurrentOperation()
        {
            if (Console.KeyAvailable)
            {
                var consoleKey = Console.ReadKey(true);
                if (consoleKey.Key == ConsoleKey.Escape)
                {
                    UserInterfaceController.WriteToConsole("Do you want to stop the current process? \nType s to stop or c to continue.");
                    string input = Console.ReadLine();
                    if (input == "c" || input == "C")
                    {
                        return false; // Continue 
                    }
                    else if (input == "s" || input == "S")
                    {
                        return true; // Break the loop
                    }
                    else
                    {
                        UserInterfaceController.WriteToConsole("Error: Input was not recognized, the current process will now continue. Press Esc to stop the operation.");
                    }
                }
            }
            return false;
        }

    }
}
