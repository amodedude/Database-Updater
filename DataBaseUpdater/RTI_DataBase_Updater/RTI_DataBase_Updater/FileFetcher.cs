using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RTI.DataBase.Application.Controllers;

namespace RTI.DataBase.Application
{
    /// <summary>
    /// Handles downloading of USGS text files to the file repository.
    /// </summary>
    class FileFetcher
    {
        bool paused = false;
        bool stop = false;
        private ManualResetEvent _pause = new ManualResetEvent(false);

        /// <summary>
        /// Creates an new thread to download 
        /// USGS text files asynchronously. 
        /// </summary>
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
                            UserInterface.WriteToConsole("\nDownloaded " + step.ToString() + " file(s) out of " + int.MaxValue.ToString());

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
            

        /// <summary>
        /// Downloads the USGS text files 
        /// contaning conductivity information. 
        /// </summary>
        /// <param name="USGSID"></param>
        private void download_file(long USGSID)
        {
            UserInterface.WriteToConsole("Downloading File  " + Convert.ToString(USGSID) + "...");
            Thread.Sleep(3000); // Simulate the download process
            UserInterface.WriteToConsole("File download complete! \n");
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
