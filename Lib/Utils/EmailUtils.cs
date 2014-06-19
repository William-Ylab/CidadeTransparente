using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Utils
{
    public class EmailUtils
    {
        private Entities.User _activeUser;

        public EmailUtils(Entities.User activeUser)
        {
            _activeUser = activeUser;
        }

        public void passwordRecovery(Entities.User user, string password)
        {
            if (user != null)
            {
                string body = String.Format(Resources.Email.body_password_recovery, user.Name, user.Login, password, Utils.AppSettingsUtils.SiteUrl);
                string subject = String.Format(Resources.Email.subject_password_recovery);

                body = body.Replace("\r\n", "<br/>");

                sendEmail(subject, body, user.Email);
            }
        }

        private void sendEmail(string subject, string message, string emailTo)
        {
            try
            {
                if (Utils.AppSettingsUtils.EmailSender == null)
                {
                    Log.ErrorLog.saveError("Lib.General.Email.sendEmail", new Exception("email.sender não configurado no web.config"));
                }

                Commons.EmailUtils emailUtils = new Commons.EmailUtils("", "Transparência governamental", Utils.AppSettingsUtils.EmailSender);

                System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(emailTo);

                emailUtils.enviaEmail(message, subject, mailAddress);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.General.Email.sendEmail", ex);
                throw new Exception("Lib.General.Email.sendEmail - " + ex.Message, ex);
            }
        }
    }
}
