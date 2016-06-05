using System;
using System.Globalization;
using System.Timers;
using RTI.DataBase.Application.Logger;

namespace RTI.Database.Updater.Util
{
    internal class RTI_Timer
    {
        /// <summary>
        /// Initialize mode settings.
        /// </summary>
        internal RTI_Timer()
        {
            TimerSettings.Mode = App.Settings.Mode;
        }
        /// <summary>
        /// Initialize new timer. To set timer duration,
        /// either set the "IntervalMinutes" app config 
        /// parameter, or pass in the duration timespan.
        /// </summary>
        /// <param name="time"></param>
        internal bool StartTimer(TimeSpan? duration = null)
        {
            TimerSettings.TimerFinished = false;
            TimeSpan runDuration = new TimeSpan();
            runDuration = duration == null ? GetTimerSpan() : default(TimeSpan);

            if (runDuration != default(TimeSpan))
            {
                Timer timer = new Timer(runDuration.TotalMilliseconds);
                timer.Enabled = true;
                timer.Start();
                timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                DateTime nextRun = DateTime.Now.Add(TimerSettings.TimerInterval);
                ApplicationLog.WriteMessageToLog("Next Scheduled database update at " + nextRun.ToLongDateString() + " @" + nextRun.ToLongTimeString(),false, false,false);
                while (!TimerSettings.TimerFinished) ;
                timer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);
            }
            return true;
        }

        /// <summary>
        /// Get duration to run the timer for.
        /// </summary>
        internal TimeSpan GetTimerSpan()
        {
            TimerSettings.Mode = App.Settings.Mode;
            DateTime scheduledTime = new DateTime();

            switch (TimerSettings.Mode)
            {
                case "Daily":
                    scheduledTime = DateTime.ParseExact(App.Settings.ScheduledTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                    if (scheduledTime > DateTime.Now)
                        TimerSettings.TimerInterval = scheduledTime - DateTime.Now;
                    else
                        TimerSettings.TimerInterval = (scheduledTime + TimeSpan.FromDays(1)) - DateTime.Now;
                    break;
                case "Interval":
                    double IntervalMin = double.TryParse(App.Settings.IntervalMinutes, out IntervalMin) ? IntervalMin : 15.00;
                    int IntervalSec = Convert.ToInt32(Math.Round(60 * IntervalMin));
                    TimeSpan RunInterval = new TimeSpan(0, 0, IntervalSec);
                    TimerSettings.TimerInterval = RunInterval;
                    break;
                case "Manual":
                    TimerSettings.TimerInterval = TimeSpan.FromMilliseconds(0);
                    break;
                default:
                    TimerSettings.TimerInterval = (DateTime.Today + TimeSpan.FromDays(1)) - DateTime.Now;
                    break;
            }
            return TimerSettings.TimerInterval;
        }

        /// <summary>
        /// Event handeler for when timer finishes countdown.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object obj, ElapsedEventArgs e)
        {
            TimerSettings.TimerFinished = true;
        }
    }

    internal static class TimerSettings
    {
        public static string Mode { get; set; }
        public static TimeSpan TimerInterval { get; set; }
        public static bool TimerFinished { get; set; }
    }
}
