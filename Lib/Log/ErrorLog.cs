using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Log
{
    public class ErrorLog
    {
        public static void saveError(string path, Exception ex)
        {
            //TODO: Logar o erro como desejar!
        }

        public static void saveError(string path, string content)
        {
            //TODO: Logar o erro como desejar!
        }

        public static string getStringErrorFromException(Exception exception)
        {

            StringBuilder sb = new StringBuilder();
            if (exception != null)
            {
                Exception exAux = exception.InnerException;
                while (exAux != null)
                {
                    sb.Append(getStringErrorFromException(exAux));

                    exAux = exAux.InnerException;
                }

                sb.AppendLine(String.Format("Msg: {0}", exception.Message));
                sb.AppendLine(String.Format("Stack: {0}", exception.StackTrace));

                if (exception.TargetSite != null)
                    sb.AppendLine(String.Format("TargetSite: {0}", exception.TargetSite.ToString()));

                sb.AppendLine("---------------------------------------------");
            }
            return sb.ToString();
        }
    }
}
