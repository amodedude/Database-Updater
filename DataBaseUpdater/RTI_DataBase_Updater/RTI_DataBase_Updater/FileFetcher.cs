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

        bool stop = false;
        bool pause = false;

        public void fetchFile()
        {
            try
            {
                while (true)
                {
                    for (int step = 0; step <= int.MaxValue; step++)
                    {
                        while (pause);
                        if (!stop) // !breakCurrentOperation() Checks if the user has halted the current opperation by pressing ESC
                        {

                            UserInterfaceController.WriteToConsole("\nDownloaded " + step.ToString() + " file(s) out of " + int.MaxValue.ToString());

                            // Generate a USGS ID
                            Random rnd = new Random();
                            long USGSID = rnd.Next();

                            // Fetch the file
                            download_file(USGSID);

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

                    if (stop)//startNewOperation()
                    {
                        break;
                    }
                }
            }
            finally
            {
                // Close the file
            }
        }

        private void download_file(long USGSID)
        {
            UserInterfaceController.WriteToConsole("Downloading File  " + Convert.ToString(USGSID) + "...");
            Thread.Sleep(3000);
            UserInterfaceController.WriteToConsole("File download complete! \n");
        }

        public void Stop()
        {
            stop = true;
        }

        public void Pause()
        {
            if (!pause)
                pause = true;
            else
                pause = false;
        }
    }
}
