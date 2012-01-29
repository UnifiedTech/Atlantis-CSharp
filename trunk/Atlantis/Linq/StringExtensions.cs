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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static partial class Extensions
    {
        /// <summary>
        ///     <para>Parses a string value as an enumeration.</para>
        ///     <para>This uses case-sensitivity to check.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T EnumParse<T>(this string value)
        {
            return EnumParse<T>(value, false);
        }

        /// <summary>
        ///     Parses a string value as an enumeration, allowing for case insensitivity if needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignorecase"></param>
        /// <returns></returns>
        public static T EnumParse<T>(this string value, bool ignorecase)
        {

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            value = value.Trim();

            if (value.Length == 0)
            {
                throw new ArgumentException("Must specify valid information for parsing in the string.", "value");
            }

            Type t = typeof(T);

            if (!t.IsEnum)
            {
                throw new ArgumentException("Type provided must be an Enum.", "T");
            }

            return (T)Enum.Parse(t, value, ignorecase);
        }

        /// <summary>
        ///     <para>Wraps the current string in double quotes ("")</para>
        ///     <para>Taken from mIRC "$qt()" alias</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Qt(this string source)
        {
            return String.Format("\"{0}\"", source);
        }

        /// <summary>
        ///     <para>Wraps the current string in single quotes ('')</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Sqt(this string source)
        {
            return String.Format("'{0}'", source);
        }

        /// <summary>
        ///     Compares a System.String to another System.String checking for equality, ignoring case.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string source, string value)
        {
            return source.Equals(value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        ///     Converts a System.String into an MD5 Hash
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <see cref="http://blog.stevex.net/c-code-snippet-creating-an-md5-hash-string/" />
        public static string ToMD5(this string source)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] uc = new byte[source.Length * 2];
            enc.GetBytes(source.ToCharArray(), 0, source.Length, uc, 0, true);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(uc);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; ++i)
            {
                sb.Append(result[i].ToString("X2"));
            }

            return sb.ToString();
        }

    }
}
