using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RTI.Database.Updater
{
    /// <summary>
    /// Handles reading of downloaded files. 
    /// </summary>
    class FileParser
    {
        /// <summary>
        /// Initializes the file read opperation. 
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadFile(string filePath)
        {
            //TODO - Verify that the file/path actually exist...
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
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console
                    extractData(sr);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Extracts conductivity and date values from each line
        /// </summary>
        /// <returns></returns>
        public void extractData (StreamReader fileContents)
        {
            Dictionary<DateTime, int> conductivity = new Dictionary<DateTime, int>();

            DateTime date;
            double cond;
            char[] delimiter = new char[] { '\t' };
            int dateCol = 0, condCol = 0, numHeaders = 2;


            try {
                // Read the file line by line 
                while (!fileContents.EndOfStream)
                {
                    string line = fileContents.ReadLine();

                    // Extract Data
                    if (!line.StartsWith("#"))
                    {
                        if (!line.Contains("agency_cd") || !line.Contains("site_no")  || !line.Contains("datetime"))
                        {
                            var segments = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                            date = Convert.ToDateTime(segments[dateCol]);
                            cond = Convert.ToDouble(segments[condCol]);

                            //DEBUG
                            Console.WriteLine(date + " " + cond);
                        }
                        else 
                        {
                            var header = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                            dateCol = header.Select((v, i) => new { Index = i, Value = v }).Where(p => p.Value == "datetime").Select(p => p.Index).ToList().First();
                            condCol = header.Select((v, i) => new { Index = i, Value = v }).Where(p => p.Value.Contains("00095") && !p.Value.Contains("cd")).Select(p => p.Index).ToList().First();

                            for (int i = 0; i < numHeaders-1; i++)
                                fileContents.ReadLine();
               
                            //dateCol = headerList.IndexOf("datetime");
                            
                        }       
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileContents.Dispose();
            }
            
        }

    public Dictionary<DateTime, double> cond { get; set; }


}
}
