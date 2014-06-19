using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    /// <summary>
    /// Classe de utilidades para SQL
    /// </summary>
    public class SQLUtils
    {
        public static string formatStringToSQL(string text)
        {
            if (text == null)
                return "";

            text = text.Replace("\\", "");
            text = text.Replace("'", "''");
            text = text.Replace("’", " ");
            text = text.Replace("′", "''");

            return text;
        }

        public static string formatDateTimeToSQL(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static bool isTableDoesNotExist(Exception ex)
        {
            bool isTableNotExist = false;
            if (ex != null)
            {

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("doesn't exist"))
                        isTableNotExist = true;
                }
                else
                {
                    if (ex.Message.Contains("doesn't exist"))
                        isTableNotExist = true;
                }
            }

            return isTableNotExist;

        }
    }
}
