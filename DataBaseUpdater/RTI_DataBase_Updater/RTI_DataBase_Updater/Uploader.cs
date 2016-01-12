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
            foreach (var instant in data)
            {
               //TODO - Add data to the RTI DB here~!
            }
        }
    }
}
