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
        public void beginUpload(List<WaterData> data)
        {
            using (var db = new RTIDBContext())
            {
                foreach (var day in data)
                {
                    //TODO - Ensure that correct SOURCEID gets added to rowdata entry!
                    var rowdata = new water_data { measurment_date = day.date.ToString(), cond = Convert.ToInt32(day.waterConductivity), temp = null };
                    var waterData = db.Set<water_data>();
                    waterData.Add(rowdata);
                }
            }
        }
    }
}
