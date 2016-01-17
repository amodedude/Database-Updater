using System;
using RTI.Database.Updater;
using RTI.DataBase.Application.Controllers;

namespace RTI.DataBase.Application
{
    class Program
    {    
        /// <summary>
        /// Application start-up main method.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(Application_ProcessExit);
            //SetUpWindow(ConsoleColor.Green, 1, 1);
            string welcomeMessage =
             "\n ---------------------------------------------------------------------\n"
              +   "              Recirculation Technologies, LLC 2015"
              + "\n ---------------------------------------------------------------------\n" 
              + "\n                Welcome to the RTI Database Updater."  
              + "\n\n This application will run continuously and make updates to the RTI\n"
              + " database's water-data table located on the RTI server. The application\n"
              + " will update the database \n with the latest conductivity information\n"
              + " taken from the USGS website.";

            UserInterface.WriteToConsole(welcomeMessage);
            //SetUpWindow(ConsoleColor.White, 1, 1);

            UserInterface.WriteToConsole("\nAre you ready to run the RTI database updater? y/n");
            MenuController start = new MenuController();
            start.startApplication(UserInterface.ReadFromConsole());
        }

        /// <summary>
        /// Set the console window properties. 
        /// </summary>
        /// <param name="foregroundColor"></param>
        /// <param name="windowHeightScale"></param>
        /// <param name="windowWidthScale"></param>
        private static void SetUpWindow(ConsoleColor foregroundColor, double heightFactor, double widthFactor)
        {
            heightFactor = heightFactor > 1 ? heightFactor : 1;
            heightFactor = heightFactor <= 0.20 ? heightFactor : 0.20;
            heightFactor = widthFactor > 1 ? widthFactor : 1;
            heightFactor = widthFactor <= 0.20 ? widthFactor : 0.20;

            int height = Convert.ToInt32(heightFactor * Console.LargestWindowHeight);
            int width = Convert.ToInt32(widthFactor * Console.LargestWindowWidth);
            Console.WindowHeight = height;
            Console.WindowWidth = width;
            Console.SetBufferSize(width, height);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "RTI Database Auto-Updater";
        }

        /// <summary>
        /// On Exit event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("Application Termintated.");
            ApplicationLog.WriteMessageToLog("***Application Terminated****", true, true);          
        }
    }
}
