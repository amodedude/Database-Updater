using System;
using RTI.DataBase.Application.FileIO;
using RTI.DataBase.Application.UpdaterModel;

namespace RTI.DataBase.Application.Views
{
    /// <summary>
    /// Displays the progress of an 
    /// opperation to the console window. 
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

        /// <summary>
        /// Token flag used to reset the 
        /// progress bar back to the starting value. 
        /// </summary>
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

        private void RefreshDisplay()
        {
            

        }
    }
}
