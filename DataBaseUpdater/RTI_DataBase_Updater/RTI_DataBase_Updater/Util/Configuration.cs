using System.Configuration;

namespace RTI.Database.Updater.Util
{
    /// <summary>
    /// Defines class to hold
    /// General Application Settings
    /// from the app.config file.
    /// </summary>
    class App : ConfigurationSection 
    {
        private static App appSetup = ConfigurationManager.GetSection("ApplicationSettings") as App;

        public static App Settings
        {
            get { return appSetup; }
        }

        [ConfigurationProperty("Mode", IsRequired = true)]
        public string Mode
        {
            get { return (string)this["Mode"]; }
        }

        [ConfigurationProperty("IntervalMinutes", IsRequired = true)]
        public string IntervalMinutes
        {
            get { return (string)this["IntervalMinutes"]; }
        }

        [ConfigurationProperty("ScheduledTime", IsRequired = true)]
        public string ScheduledTime
        {
            get { return (string)this["ScheduledTime"]; }
        }

        [ConfigurationProperty("RunOnStartUp", IsRequired = true)]
        public string RunOnStartUp
        {
            get { return (string)this["RunOnStartUp"]; }
        }
    }

    /// <summary>
    /// Defines class to hold 
    /// email notification service settings 
    /// from the app.config file.
    /// </summary>
    //class Email : ConfigurationSection
    //{
    //    private static Email appsetup = ConfigurationManager.GetSection("EmailSettings") as Email;

    //    public static Email Settings
    //    {
    //        get { return appsetup; }
    //    }

    //    [ConfigurationProperty("SMTPClient", IsRequired = true)]
    //    public string SMTPClient
    //    {
    //        get { return (string)this["SMTPClient"]; }
    //    }

    //    [ConfigurationProperty("ToMail", IsRequired = true)]
    //    public string ToMail
    //    {
    //        get { return (string)this["ToMail"]; }
    //    }

    //    [ConfigurationProperty("CcMail", IsRequired = true)]
    //    public string CcMail
    //    {
    //        get { return (string)this["CcMail"]; }
    //    }

    //    [ConfigurationProperty("BccMail", IsRequired = true)]
    //    public string BccMail
    //    {
    //        get { return (string)this["BccMail"]; }
    //    }

    //    [ConfigurationProperty("FromMail", IsRequired = true)]
    //    public string FromMail
    //    {
    //        get { return (string)this["FromMail"]; }
    //    }

    //    [ConfigurationProperty("SendAlerts", IsRequired = true)]
    //    public string SendAlerts
    //    {
    //        get { return (string)this["SendAlerts"]; }
    //    }

    //    [ConfigurationProperty("SendOnStatusOK", IsRequired = true)]
    //    public string SendOnStatusOK
    //    {
    //        get { return (string)this["SendOnStatusOK"]; }
    //    }
    //}
}
