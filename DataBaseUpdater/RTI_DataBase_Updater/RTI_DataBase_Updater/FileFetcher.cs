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
        bool paused = false;
        bool stop = false;
        private ManualResetEvent _pause = new ManualResetEvent(false);

        // Begins the File Download
        public void fetchFile()
        {
            try
            {
                while (true)
                {
                    for (int step = 0; step <= int.MaxValue; step++)
                    {

                        _pause.WaitOne(Timeout.Infinite); // Pause File Download Process

                        if (!stop)
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
                }
            }
            catch
            {
                // Catch Stuff
            }
        }
            

        

        private void download_file(long USGSID)
        {
            UserInterfaceController.WriteToConsole("Downloading File  " + Convert.ToString(USGSID) + "...");
            Thread.Sleep(3000); // Simulate the download process
            UserInterfaceController.WriteToConsole("File download complete! \n");
        }

        // Triggers the START event 
        public void Start()
        {
            _pause.Set();
            paused = false;
        }

        // Triggers the STOP event 
        public void Stop()
        {
            stop = true;
        }

        // Toggles the Pause event 
        public void Pause()
        {
            if (!paused)
                _pause.Reset(); // Pause
            else
                _pause.Set();     // Un-Pause

            paused = !paused; // Toggle Pause State
        }
    }
}
