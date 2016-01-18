using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RTI.DataBase.Application.Controllers;
using RTI.Database.Updater;

namespace RTI.DataBase.Application
{
    /// <summary>
    /// Handles downloading of USGS text files into the file repository.
    /// </summary>
    class UpdateRoutine
    {
        private bool paused;
        public bool download_finished = true;
        private object pauseToken = new object();

        /// <summary>
        /// Creates an new thread to download 
        /// USGS text files asynchronously. 
        /// </summary>
        public void fetchFile(CancellationTokenSource cancle)
        {
            List<string> failedSiteIDs = new List<string>();
            try
            {
                // Get the list of sources from the RTI database
                Console.Clear();
                UserInterface.WriteToConsole("Fetching the list of sources from the RTI database.\nPlease wait...");
                RTIDBContext RTIContext = new RTIDBContext();
                FileParser parseFile = new FileParser();
                var sourceList = RTIContext.sources.ToList();
                int numberOfFilesToDownload = sourceList.Count() - 1;
                int filesDownloaded = 0;
                
                // Begin downloading from the USGS
                while (!cancle.IsCancellationRequested && filesDownloaded < numberOfFilesToDownload) // Cancle if requested
                {
                    foreach (var source in sourceList) // Loop through each USGS source 
                    {
                        lock (pauseToken) { } // Pause download while pauseToken is set

                        Console.Clear();
                        double percentage = ((double)filesDownloaded / numberOfFilesToDownload);
                        UserInterface.WriteToConsole(
                            "Progress:                                                      {0:P}" +
                            "\n--------------------------------------------------------------------" +
                            "\nDownloaded {1} file(s) out of {2}", percentage, filesDownloaded, numberOfFilesToDownload);

                        if (failedSiteIDs.Count > 0)
                            UserInterface.WriteToConsole("{0} files were unable to be downloaded.", failedSiteIDs.Count);

                        // Get the USGSID
                        try
                        {
                            string USGSID = source.agency_id;
                            string filePath = download_file(USGSID); // Fetch the file
                            parseFile.ReadFile(filePath); // Read the fetched file contents 
                        }
                        catch (Exception ex)
                        {
                            ApplicationLog.WriteMessageToLog("Error: " + ex.Message + " Inner" + ex.InnerException, true, true);
                            System.Diagnostics.Debugger.Break();
                            UserInterface.WriteToConsole("\nError: Unable to download file {0} of {1}.\n\nSite ID = {2:N}, \nName = {3}", 
                                                          filesDownloaded+1, numberOfFilesToDownload, source.agency_id, source.full_site_name);
                            failedSiteIDs.Add(source.agency_id);
                        }
                        finally
                        {
                            filesDownloaded++;
                        }

                    }
                }

                UserInterface.WriteToConsole("\nFle download(s) complete!\n\nInitializing upload process...");

            }
            catch (Exception ex)
            {
                ApplicationLog.WriteMessageToLog("Error: " + ex.Message + " Inner" + ex.InnerException, true, true);
                System.Diagnostics.Debugger.Break();
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// Pauses the FileFetcher process
        /// </summary>
        public void Pause()
        {
            if (paused == false)
            {
                Monitor.Enter(pauseToken); // Set the pause token
                paused = true;
            }
        }

        /// <summary>
        /// Resumes the FileFetcher process
        /// </summary>
        public void Resume()
        {
            if (paused)
            {
                paused = false;
                Monitor.Exit(pauseToken); // Clear the pause token
            }
        }


        /// <summary>
        /// Downloads the USGS text files 
        /// contaning conductivity information. 
        /// </summary>
        /// <param name="USGSID"></param>
        /// <returns>
        /// Retucns the downloaded files full path.
        /// </returns>
        private string download_file(string USGSID)
        {
            UserInterface.WriteToConsole("Downloading File with USGSID =  " + Convert.ToString(USGSID));
            ApplicationLog.WriteMessageToLog("Downloading File with USGSID =  " + Convert.ToString(USGSID), true, true);

            download_finished = false;
            string full_file_path;
            using (var client = new WebClient())
            {
                string USGS_URL = "http://nwis.waterdata.usgs.gov/nwis/uv?cb_00095=on&format=rdb&site_no=" + USGSID + "&period=1095";
                Uri USGS_URI = new Uri(USGS_URL, UriKind.Absolute);
                string file_name = USGSID + ".txt";
                string folder_path = @"C:\Users\John\Desktop\RTI File Repository\";
                full_file_path = folder_path + file_name;
                client.DownloadFile(USGS_URI.AbsoluteUri, full_file_path);
            }
            download_finished = true;
            return full_file_path;
        }
    }
}
