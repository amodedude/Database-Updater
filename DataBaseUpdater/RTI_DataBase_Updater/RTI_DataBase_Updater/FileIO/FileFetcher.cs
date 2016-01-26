using System;
using System.Net;
using System.Linq;
using System.Threading;
using RTI.DataBase.Application.UpdaterModel;
using System.Collections.Generic;
using RTI.DataBase.Application.Controllers;
using RTI.DataBase.Application.FileIO;
using RTI.DataBase.Application.Logger;
using System.Text;
using System.Threading.Tasks;

namespace RTI.DataBase.Application.FileIO
{
    /// <summary>
    /// Handles downloading of USGS text files into the file repository.
    /// </summary>
    class UpdateRoutine
    {
        private bool paused;
        public bool download_finished = true;
        private object pauseToken = new object();
        string temp;

        /// <summary>
        /// Creates an new thread to download 
        /// USGS text files asynchronously. 
        /// </summary>
        public async void fetchFile(CancellationTokenSource cancle)
        {
            List<string> failedSiteIDs = new List<string>();
            try
            {
                // Get the list of sources from the RTI database
                UserInterface.ClearUI();
                UserInterface.WriteToConsole("Fetching the list of sources from the RTI database.\nPlease wait...");
                rtidevEntities RTIContext = new rtidevEntities();
                FileParser parseFile = new FileParser();
                var sourceList = RTIContext.sources.ToList();
                int numberOfFilesToDownload = sourceList.Count() - 1;
                int filesDownloaded = 0;
                
                // Begin downloading from the USGS
                while (!cancle.IsCancellationRequested && filesDownloaded < numberOfFilesToDownload) // Cancle if requested
                {
                    foreach (var source in sourceList) // Loop through each USGS source 
                    {
                        if (download_finished)
                        {
                            lock (pauseToken) { } // Pause download while pauseToken is set
                            UserInterface.ClearUI();
                            double percentage = ((double)filesDownloaded / numberOfFilesToDownload);
                            UserInterface.WriteToConsole("Total Progress:                                   {0:P}" +
                                "\n--------------------------------------------------------" +
                                "\nDownloaded {1} file(s) out of {2}", percentage, filesDownloaded, numberOfFilesToDownload);

                            if (failedSiteIDs.Count > 0)
                                UserInterface.WriteToConsole("{0} files were unable to be downloaded.", failedSiteIDs.Count);


                            //UserInterface.ClearUI();
                            //UserInterface.WriteToConsole(outputMain.ToString());

                            // Get the USGSID
                            try
                            {
                                string USGSID = source.agency_id;
                                string file_name = USGSID + ".txt";
                                string folder_path = @"C:\Users\John\Desktop\RTI File Repository\";
                                string filePath = folder_path + file_name;
                                await download_file(USGSID, filePath); // Fetch the file
                                parseFile.ReadFile(filePath); // Read the fetched file contents 
                            }
                            catch (Exception ex)
                            {
                                ApplicationLog.WriteMessageToLog("Error: " + ex.Message + " Inner" + ex.InnerException, true, true, true);
                                System.Diagnostics.Debugger.Break();
                                UserInterface.WriteToConsole("\nError: Unable to download file {0} of {1}.\n\nSite ID = {2:N}, \nName = {3}",
                                                              filesDownloaded + 1, numberOfFilesToDownload, source.agency_id, source.full_site_name);
                                failedSiteIDs.Add(source.agency_id);
                            }
                            finally
                            {
                                filesDownloaded++;
                            }
                        }
                    }
                }

                UserInterface.WriteToConsole("\nFle download(s) complete!\n\nInitializing upload process...");

            }
            catch (Exception ex)
            {
                ApplicationLog.WriteMessageToLog("Error: " + ex.Message + " Inner" + ex.InnerException, true, true, true);
                System.Diagnostics.Debugger.Break();
                //UserInterface.WriteToConsole(ex.Message);
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
        private async Task download_file(string USGSID, string filePath)
        {
            UserInterface.WriteToConsole("\nDownloading File with USGSID =  " + Convert.ToString(USGSID));
            ApplicationLog.WriteMessageToLog("Downloading File with USGSID =  " + Convert.ToString(USGSID), true, false, false);
            download_finished = false;
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                string USGS_URL = "http://nwis.waterdata.usgs.gov/nwis/uv?cb_00095=on&format=rdb&site_no=" + USGSID + "&period=1095";
                Uri USGS_URI = new Uri(USGS_URL, UriKind.Absolute);
                await client.DownloadFileTaskAsync(USGS_URI, filePath);
                counter = 0;
            }
            download_finished = true;
        }

        private int counter { get; set; }
        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            counter++;
            // Prevents this event from triggering so quickly that the console window
            // glitches and does not have time to clear before printing the next messge. 
            if (counter % 15 == 0)
            {
                if (counter == 15)
                    temp = UserInterface.GetCurrentOutput();
                else
                    temp = UserInterface.GetCurrentOutput(1);

                var KB = (e.BytesReceived / 1000);
                UserInterface.ClearUI();
                UserInterface.WriteToConsole(temp + "\nDownloaded (kB): " + KB.ToString());
            }
        }
}

    
}
