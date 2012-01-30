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

namespace Atlantis.Security.Linq
{
    using System;
    using System.Text;
    using System.Security.Cryptography;

    public static class Extensions
    {
        #region Methods

        /// <summary>
        ///     <para>Converts a System.String into an MD5 Hash</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>See: http://blog.stevex.net/c-code-snippet-creating-an-md5-hash-string/ for more details</para>
        /// </devdoc>
        public static String ToMD5(this String source)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            Byte[] uc = new Byte[source.Length * 2];
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

        #endregion
    }
}