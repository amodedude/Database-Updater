using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;


namespace RTI.Database.Updater
{
    /// <summary>
    /// Handles reading of downloaded files. 
    /// </summary>
    sealed class FileParser
    {
        /// <summary>
        /// Initializes the file read opperation. 
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadFile(string filePath)
        {
            CurrentLineNumber = 0;
            if (File.Exists(filePath))
                OpenFile(filePath);
        }

        /// <summary>
        /// Opens the file to be read. 
        /// </summary>
        /// <param name="filePath"></param>
        private void OpenFile(string filePath)
        {
            try
            {
                ApplicationLog.WriteMessageToLog("Opening File: " + filePath, true, true);

                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console
                    var data = extractData(sr, filePath);
                    Uploader rtiUploader = new Uploader();
                    if(data.Count() > 0) // Only upload if there is data to be uploaded. 
                        rtiUploader.beginUpload(data);
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteMessageToLog("Error: " + ex.Message + " Inner" + ex.InnerException, true, true);
                System.Diagnostics.Debugger.Break();
                Console.WriteLine("There was an error reading this file: ");
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Extracts conductivity and date values from each line.
        /// Returns a list of data records for each day. 
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        private int dateCol = 0, condCol = 0;
        private List<water_data> extractData (StreamReader fileContents, string filePath)
        {
            Dictionary<DateTime, int> conductivity = new Dictionary<DateTime, int>();
            DateTime date;
            int cond;
            List <water_data> data = new List<water_data>();
            char[] delimiter = new char[] { '\t' };
            int numHeaders = 2;
            bool isHeaderFound = false;

            try {
                // Read the file line by line 
                while (!fileContents.EndOfStream)
                {
                    string line = fileContents.ReadLine();

                    // Extract Data
                    if (!line.StartsWith("#"))
                    {
                        if ((!line.Contains("agency_cd") || !line.Contains("site_no") || !line.Contains("datetime")) && isHeaderFound)
                        {
                            var segments = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                            bool dateFormatOk = DateTime.TryParse(segments[dateCol], out date);
                            bool condFormatOk = int.TryParse(segments[condCol], out cond);

                            if (dateFormatOk && condFormatOk)
                            {
                                var todaysData = new water_data();
                                todaysData.measurment_date = date.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                                todaysData.cond = cond;

                                data.Add(todaysData);

                                //DEBUG
                                Console.WriteLine(date + " " + cond);
                            }
                        }
                        else
                        {
                            var header = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                            dateCol = header.Select((v, i) => new { Index = i, Value = v }).Where(p => p.Value == "datetime").Select(p => p.Index).ToList().DefaultIfEmpty(-999).FirstOrDefault();
                            condCol = header.Select((v, i) => new { Index = i, Value = v }).Where(p => p.Value.Contains("00095") && !p.Value.Contains("cd")).Select(p => p.Index).ToList().DefaultIfEmpty(-999).FirstOrDefault();

                            if (dateCol != -999 && condCol != -999) // If -999, than "datetime" and "00095" do not exist in this line. 
                            {
                                isHeaderFound = true;
                                for (int i = 0; i < numHeaders - 1; i++)
                                    fileContents.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("\n" + filePath + " is not formated properly. \nThis file and it's contents will not be parsed from line " + Convert.ToString(CurrentLineNumber) + ".");
                                break; // Stop reading the file uppon incorrect text format detection
                            }
                        }
                    }
                }
                return data;
            }
            catch(Exception ex)
            {
                ApplicationLog.WriteMessageToLog("Error: " + ex.Message + " Inner" + ex.InnerException, true, true);
                System.Diagnostics.Debugger.Break();
                throw ex;
            }
            finally
            {
                fileContents.Dispose();                
            }    
        }

        /// <summary>
        /// Stores the current line number being read. 
        /// </summary>
        private long CurrentLineNumber { get; set; }

        /// <summary>
        /// Reads the next line in the file stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private string ReadNextLine (StreamReader fileStream)
        {
            CurrentLineNumber++; // Advance to the next line
            return fileStream.ReadLine();
        }

    }   
}
