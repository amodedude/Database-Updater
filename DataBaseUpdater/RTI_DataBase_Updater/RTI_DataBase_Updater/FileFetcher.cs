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
    /// Handles downloading of USGS text files into the file repository.
    /// </summary>
    class FileFetcher
    {
        bool paused = false;
        bool _stop = false;
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

                        if (!_stop)
                        {
                            Console.Clear();
                            double percentage = (step / int.MaxValue) * 100;
                            Console.WriteLine(
                                "Progress:                                                      {0}%" +
                                "\n--------------------------------------------------------------------" +
                                "\nDownloaded " + step.ToString() + " file(s) out of " + int.MaxValue.ToString(), step);

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
            UserInterface.WriteToConsole("Downloading File with USGSID =  " + Convert.ToString(USGSID));
            Thread.Sleep(500); // Simulate the download process
        }

        /// <summary>
        /// Resumes the asynchronous FileFetcher 
        /// download thread.  
        /// </summary>
        public void start()
        {
            _pause.Set();
            paused = false;
        }

        /// <summary>
        /// Stops the asynchronous FileFetcher 
        /// download thread.  
        /// </summary>
        public void stop()
        {
            _stop = true;
        }

        /// <summary>
        /// Either pauses or unpauses the 
        /// asynchonous FileFetcher download 
        /// thread based on the current state. 
        /// </summary>
        public void pause()
        {
            if (!paused)
                _pause.Reset(); // Pause
            else
                _pause.Set();     // Un-Pause

            paused = !paused; // Toggle Pause State
        }

        /// <summary>
        /// Returns the current state of the 
        /// asynchronous FileFetcher download 
        /// thred. 
        /// </summary>
        /// <returns></returns>
        public bool isPaused()
        {
            return paused;
        }


    }
}
