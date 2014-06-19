using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.General
{
    public class AppSettingsHelper
    {
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
