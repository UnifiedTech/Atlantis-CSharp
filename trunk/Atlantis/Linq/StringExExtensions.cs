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

    internal class RegexCache
    {
        #region Constructor(s)

        public RegexCache(string pattern, RegexOptions options = RegexOptions.None)
        {
            Pattern = pattern;

            RegEx = new Regex(pattern, options);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the pattern that represents this regular expression</para>
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        ///     <para>Gets a pre-compiled regex object from the current pattern</para>
        /// </summary>
        public Regex RegEx { get; private set; }

        #endregion
    }

    public partial class Extensions
    {
        #region Properties

        internal static List<RegexCache> RegExCache = new List<RegexCache>();

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
            var cache = RegExCache.Find(c => c.Pattern.Equals(pattern));
            if (cache != null)
            {
                return cache.RegEx.IsMatch(source);
            }
            else
            {
                cache = new RegexCache(pattern, options);
                RegExCache.Add(cache);

                return cache.RegEx.IsMatch(source);
            }
        }

        #endregion
    }
}