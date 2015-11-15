using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTI_DataBase_Updater.Controllers;

// Starting point for the application on first run
namespace RTI_DataBase_Updater
{
    class Program
    {       
        static void Main(string[] args)
        {
            string welcomeMessage = "Welcome to the RTI database Auto-Updater. " +
                "\nThis application will run continuously and make updates to the RTI "
            + "MySQL database's water-data table located on the RTI server. The application "
            + "will update the database with the latest conductivity information taken from the"
            + " USGS website.";

            UserInterfaceController.WriteToConsole(welcomeMessage);
            UserInterfaceController.WriteToConsole("\nAre you ready to run the RTI database updater? y/n");
            MenuController.startApplication(UserInterfaceController.ReadFromConsole());
        }
    }
}
