/*
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Initial Developer of the Original Code is Unified Technologies.
 * Copyright (C) 2010 Unified Technologies. All Rights Reserved.
 * 
 * Contributor(s): Zack "Genesis2001" Loveless, Benjamin "aca20031" Buzbee.
 */

namespace Atlantis.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public partial class Extensions
    {
        #region Properties

        internal static Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Escapes the current System.String of all RegEx characters.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>This method may not escape every single regular expression character in a string.</para>
        /// </devdoc>
        public static string EscapeRegex(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            const string REGEX_SYMBOLS = "^$*\\/";

            StringBuilder sb = new StringBuilder(source.Length * 2);

            for (int i = 0; i < source.Length; ++i)
            {
                if (REGEX_SYMBOLS.Contains(source[i]))
                {
                    sb.Append('\\');
                }

                sb.Append(source[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     <para>Checks whether the current string matches the specified pattern</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pPattern"></param>
        /// <param name="pOptions"></param>
        /// <returns></returns>
        public static bool Matches(this string source, string pPattern, RegexOptions pOptions = RegexOptions.None)
        {
            var cache = RegexCache.Where(dict => dict.Key.Equals(pPattern)).Select(dict => dict.Value).FirstOrDefault();

            if (cache == null)
            {
                if ((pOptions & RegexOptions.Compiled) != RegexOptions.Compiled)
                {
                    pOptions |= RegexOptions.Compiled;
                }

                cache = new Regex(pPattern, pOptions);
                RegexCache.Add(pPattern, cache);
            }

            return cache.Match(source).Success;
        }

        /// <summary>
        ///     <para>Checks whether the current string matches the specified pattern and returns the pre-compiled regex as an out parameter.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pPattern"></param>
        /// <param name="pRegex"></param>
        /// <param name="pOptions"></param>
        /// <returns></returns>
        public static bool Matches(this string source, string pPattern, out Regex pRegex, RegexOptions pOptions = RegexOptions.None)
        {
            var cache = RegexCache.Where(dict => dict.Key.Equals(pPattern)).Select(dict => dict.Value).FirstOrDefault();

            if (cache == null)
            {
                if ((pOptions & RegexOptions.Compiled) != RegexOptions.Compiled)
                {
                    pOptions |= RegexOptions.Compiled;
                }


                cache = new Regex(pPattern, pOptions);
                RegexCache.Add(pPattern, cache);
            }

            pRegex = cache;
            return pRegex.Match(source).Success;
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pPattern"></param>
        /// <param name="pMatch"></param>
        /// <param name="pOptions"></param>
        /// <returns></returns>
        public static bool Matches(this string source, string pPattern, out Match pMatch, RegexOptions pOptions = RegexOptions.None)
        {
            var cache = RegexCache.Where(dict => dict.Key.Equals(pPattern)).Select(dict => dict.Value).FirstOrDefault();

            if (cache == null)
            {
                if ((pOptions & RegexOptions.Compiled) != RegexOptions.Compiled)
                {
                    pOptions |= RegexOptions.Compiled;
                }

                cache = new Regex(pPattern, pOptions);
                RegexCache.Add(pPattern, cache);
            }

            pMatch = cache.Match(source);
            return pMatch.Success;
        }

        #endregion
    }
}