using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Lib.Utils
{
    public class AppSettingsUtils
    {
        public static string SiteUrl
        {
            get { return ConfigurationManager.AppSettings["SiteUrl"]; }
        }

        public static string FerramentaUrl
        {
            get { return ConfigurationManager.AppSettings["FerramentaUrl"]; }
        }

        public static string EmailSender
        {
            get { return ConfigurationManager.AppSettings["email.sender"]; }
        }

        public static string EmailBugEmail
        {
            get { return ConfigurationManager.AppSettings["email.bugEmail"]; }
        }

        public static string GoogleAnalyticsCode
        {
            get { return ConfigurationManager.AppSettings["google.analyticscode"]; }
        }
    }
}
