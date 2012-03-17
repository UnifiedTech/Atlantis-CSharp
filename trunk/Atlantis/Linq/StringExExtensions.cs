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
    using System.Text.RegularExpressions;

    public partial class Extensions
    {
        #region Properties

        internal static Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Checks whether the current string matches the specified pattern</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool Matches(this string source, string pattern, RegexOptions options = RegexOptions.None)
        {
            var cache = RegexCache.Where(dict => dict.Key.EqualsIgnoreCase(pattern)).Select(dict => dict.Value).FirstOrDefault();

            if (cache != null)
            {
                return cache.IsMatch(source);
            }
            else
            {
                cache = new Regex(pattern);
                RegexCache.Add(pattern, cache);

                return cache.IsMatch(source);
            }
        }

        #endregion
    }
}