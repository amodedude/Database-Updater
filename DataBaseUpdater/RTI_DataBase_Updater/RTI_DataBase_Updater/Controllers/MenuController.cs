﻿using System;
using System.Threading;
using System.Threading.Tasks;
using RTI.DataBase.Application.Logger;
using RTI.DataBase.Application.FileIO;
using RTI.Database.Updater.Util;
using System.Collections.Generic;

namespace RTI.DataBase.Application.Controllers
{
    /// <summary>
    /// Provides menu functionality.
    /// </summary>
    class MenuController
    {
        /// <summary>
        /// Asks the user if they want to start running the AutoUpdater.
        /// </summary>
        /// <param name="start"></param>
        public void startApplication(string start)
        {
            UserInterface.ClearUI();
            ApplicationLog.InsertLineSeporator();
            ApplicationLog.WriteMessageToLog("***Application Start****", true, true, true);
            ApplicationLog.InsertLineSeporator();
            try
            {
                if (start.ToUpper() == "N") // Case No
                {
                    UserInterface.WriteToConsole("Do you want to exit?  y/n");
                    exitApplication(UserInterface.ReadFromConsole(true));
                }
                else if (start.ToUpper() == "Y") // Case Yes
                {
                    UserInterface.WriteToConsole("\n\nPress the 'Esc' key at any time to exit.");
                    runNewUpdater(); // Start the updater 
                
                }
                else // Case Error
                {
                    UserInterface.WriteToConsole("Error: Unkonwn input string. Please Try Again.");
                    UserInterface.WriteToConsole("\nWould you like to restart the RTI database updater? y/n");
                    startApplication(UserInterface.ReadFromConsole());
                }
            }
            catch (Exception e)
            {
                EmailService email = new EmailService();
                List<string> emailAdressList = new List<string>();
                emailAdressList.Add("amodedude@gmail.com");
                email.SendMail(emailAdressList, "RTI Database Updater Critical System Alert", "The database updater has encountered a fatal error. \r\n\r\nExeption message: \r\n" + e.Message + "\r\n\r\nInner Exception: \r\n" + e.InnerException + "\r\nPlease contact your system administrator immediately.\r\nSystem Time Stamp: " + DateTime.Now.ToString());
                UserInterface.WriteToConsole("Error: Database updater appliction experianced a fatal exception. ");
                UserInterface.WriteToConsole("\nWould you like to restart the RTI database updater? y/n");
                startApplication(UserInterface.ReadFromConsole());
            }
        }

        /// <summary>
        /// Restarts the application. 
        /// </summary>
        public void restart()
        {
            if (App.Settings.Mode.ToUpper() == "Manual")
            {
                UserInterface.WriteToConsole("\nWould you like to re-start the RTI database updater? y/n");
                ApplicationLog.WriteMessageToLog("***Application Re-Start****", true, true, true);
                startApplication(UserInterface.ReadFromConsole());
            }
            else
            {
                // Start a new timer and force restart after time expires:
                RTI_Timer timer = new RTI_Timer();
                timer.StartTimer();
                while (TimerSettings.TimerFinished) { /*NOOP*/ }
                startApplication("Y"); // Bypass user input
            }
        }

        /// <summary>
        /// Asks the user if they want to exit the AutoUpdater.
        /// </summary>
        /// <param name="exit"></param>
        public void exitApplication(string exit)
        {
            if (exit.ToUpper() == "Y") // Case Yes
            {
                UserInterface.WriteToConsole("\nGoodbye!");
                Thread.Sleep(50); //Pause for 50ms before closing 
                Environment.Exit(0);
            }
            else if (exit.ToUpper() == "N") // Case No 
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

        /// <summary>
        /// Runs the AutoUpdater application 
        /// </summary>
        public void runNewUpdater()
        {
            UpdateRoutine fileFetcher = new UpdateRoutine();
            CancellationTokenSource fileFetcher_cancelationToken = new CancellationTokenSource();
            Task fileFetcherTask = Task.Factory.StartNew(() => fileFetcher.fetchFile(fileFetcher_cancelationToken));

            // Check if the user wants to cancle
            while (!cancleFileFetcher(fileFetcher));
            
            // Stop the file fetcher
            fileFetcher_cancelationToken.Cancel();
            UserInterface.WriteToConsole("Canceling current process, please wait...");
            while (!fileFetcher.download_finished) { }// Wait for the file fetcher to finish
            UserInterface.WriteToConsole("Process terminated.");
            restart();
        }

        /// <summary>
        /// Asks the user if they want to cancle the file fetcher process.
        /// </summary>
        /// <param name="fileFetcher"></param>
        /// <returns></returns>
        public bool cancleFileFetcher(UpdateRoutine fileFetcher)
        {
            var consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.Escape)
                {
                    fileFetcher.Pause();
                    UserInterface.WriteToConsole("\n\nDo you want to stop the current process? \nType s to stop or c to continue.");
                    string input = UserInterface.ReadFromConsole();
                    if (input.ToUpper() == "C")
                    {
                        fileFetcher.Resume();
                        return false; // Continue 
                    }
                    else if (input.ToUpper() == "S")
                    {
                        return true; // Break the loop
                    }
                    else
                    {
                        UserInterface.WriteToConsole("Error: Input was not recognized, the current process will now continue. Press Esc to stop the operation.");
                    }
                }
            return false;
        }
    }
}

