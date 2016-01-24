using System;
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;


namespace RTI.Database.Updater
{
    /// <summary>
    /// Uploads retrieved data into the RTI database. 
    /// </summary>
    class Uploader
    {
        public void Upload(List<water_data> data)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            // Split the datalist into chucks to significantly increase insert performance. 
            var splitData =  SplitList.Chunk(data,1000);
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["rtidevEntities"].ConnectionString;
            var result = from Match match in Regex.Matches(ConnectionString, "\"([^\"]*)\"")
                         select match.ToString();

            ConnectionString = result.First().ToString().TrimEnd('"').TrimStart('"');
            
            // Manually commit to the database (no EF due to speed issues)
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                List<string> Rows = new List<string>();

                foreach (var chunk in splitData)
                {
                    StringBuilder sCommand = new StringBuilder("INSERT INTO water_data (cond, temp, measurment_date, sourceid) VALUES ");
                    foreach (var date in chunk)
                    {
                        Rows.Add(string.Format("({0}, {1}, '{2}', '{3}')", MySqlHelper.EscapeString(date.cond.ToString()), MySqlHelper.EscapeString("NULL"), MySqlHelper.EscapeString(date.measurment_date), MySqlHelper.EscapeString(date.sourceid)));
                    }
                    sCommand.Append(string.Join(",", Rows));
                    sCommand.Append(";");
                    string query = sCommand.ToString();

                    using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), connection))
                    {
                        myCmd.CommandType = CommandType.Text;
                        myCmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            timer.Stop();
            Console.WriteLine("Upload Complteded in {0} seconds.\nPress any key to Continue...", timer.Elapsed.ToString());
            ApplicationLog.WriteMessageToLog("Upload Complteded in " + timer.Elapsed.ToString() + " seconds.\n\n", false, false, true);

            //Debug:
            //Console.ReadKey();
        }     
    }

    static class SplitList
    {
        /// <summary>
        /// Break a list of items into chunks of a specific size
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }
    }
}
