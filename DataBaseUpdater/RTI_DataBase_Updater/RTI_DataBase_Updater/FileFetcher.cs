using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RTI_DataBase_Updater.Controllers;

namespace RTI_DataBase_Updater
{
    class FileFetcher
    {
        public void download_file(long USGSID)
        {
            UserInterfaceController.WriteToConsole("Downloading File  " + Convert.ToString(USGSID) + "...");
            Thread.Sleep(3000);
            UserInterfaceController.WriteToConsole("File download complete! \n");
        }
    }
}
