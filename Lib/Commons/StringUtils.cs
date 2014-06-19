using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Commons
{
    /// <summary>
    /// Classe de utilidades para String
    /// </summary>
    public static class StringUtils
    {
        readonly static char[] withAccents = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç".ToCharArray();
        readonly static char[] withoutAccents = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc".ToCharArray();
        readonly static char[] caractersSpecials = "'!@#$%¨&*()_-§=`´{[^~}]ºª?/\\|:;".ToCharArray();

        /// <summary>
        /// Remove os acentos de um texto
        /// </summary>
        /// <param name="text">Texto para remoção dos acentos</param>
        /// <returns>Texto sem acentos</returns>
        public static string removeAccents(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            char[] _ca = text.ToCharArray();
            for (int _c = _ca.Length; _c-- > 0; )
            {
                int _a = Array.IndexOf(withAccents, _ca[_c]);
                if (_a >= 0)
                {
                    _ca[_c] = withoutAccents[_a];
                }
            }

            return new string(_ca);
        }

        /// <summary>
        /// Remove caracteres esperciais de um texto
        /// </summary>
        /// <param name="text">Texto para remoção dos caracteres especiais</param>
        /// <returns>Texto sem caracteres especiais</returns>
        public static string removeSpecialCaracters(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            char[] _ca = text.ToCharArray();
            for (int _c = _ca.Length; _c-- > 0; )
            {
                int _a = Array.IndexOf(caractersSpecials, _ca[_c]);
                if (_a >= 0)
                {
                    _ca[_c] = ' ';
                }
            }

            return new string(_ca);
        }
    }
}
