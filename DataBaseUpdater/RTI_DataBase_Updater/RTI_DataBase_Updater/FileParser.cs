using System;
using System.IO;

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
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    Console.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

    }
}
