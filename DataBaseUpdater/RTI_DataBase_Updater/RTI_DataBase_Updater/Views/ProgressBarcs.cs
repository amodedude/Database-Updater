using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI.Database.Updater.Views
{
    /// <summary>
    /// Displays the progress of an 
    /// opperation to the console. 
    /// </summary>
    class ProgressBar
    {
        public ProgressBar(long Max, long Min, int CurrentLine)
        {
            StartingValue = Min;
            CompletedValue = Max;
            LineNumber = CurrentLine;
        }

        private long StartingValue { get; set; }
        private long CompletedValue { get; set; }
        private long CurrentValue { get; set; }
        private readonly int LineNumber;
        public bool ReSet { get; set; } = false;

        /// <summary>
        /// Updates progressbar status
        /// </summary>
        /// <param name="CurrentValue"></param>
        public void UpdateProgress(long CurrentValue)
        {
            this.CurrentValue = CurrentValue;
        }

        /// <summary>
        /// Returns the console window linenumber 
        /// position of the selected ProgressBar oject. 
        /// </summary>
        /// <returns></returns>
        public int GetProgressBarLineNumber()
        {
            return LineNumber;
        }
    }
}
