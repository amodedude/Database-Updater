using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI.Database.Updater
{
    /// <summary>
    /// Uploads retrieved data into the RTI database. 
    /// </summary>
    class Uploader
    {
        public void beginUpload(List<water_data> data)
        {
            using (var db = new RTIDBContext())
            {
                var waterData = db.Set<water_data>();
                waterData.AddRange(data);
                //Console.WriteLine("From EF DB:    " + waterData.cond + "     date: " + waterData.Last().measurment_date);
                //foreach (var day in waterData)
                //{
                //    Console.WriteLine("From EF DB:    " + day.cond + "     date: " + day.measurment_date);
                //}

                db.SaveChanges();
            }
        }
    }
}
