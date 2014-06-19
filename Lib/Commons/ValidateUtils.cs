using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Commons
{
    public class ValidateUtils
    {
        /// <summary>
        /// Valida o formato do e-mail
        /// </summary>
        /// <param name="email">Email a ser validado</param>
        /// <returns>Retorna se é valida ou não</returns>
        public static bool validateEmail(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                Regex _rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                if (_rg.IsMatch(email))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
