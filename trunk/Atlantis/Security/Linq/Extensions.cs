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

    public enum HashType : int
    {
        MD5 = 1,
        SHA1 = 2,
        SHA256 = 3,
    }

    public static partial class Extensions
    {
        #region Methods

        /// <summary>
        ///     <para>Applies the specified hash to the current System.String</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashType">Required. Defines which hasing algorithm to use. Defaults to MD5</param>
        /// <returns></returns>
        public static String Hash(this String source, HashType hashType)
        {
            switch (hashType)
            {
                case HashType.MD5: { return source.ToMD5(); }
                case HashType.SHA1:
                    {
                        throw new NotImplementedException("SHA256 hashing has not been implemented");
                    }
                case HashType.SHA256:
                    {
                        throw new NotImplementedException("SHA256 hashing has not been implemented");
                    }
                default:
                    {
                        return String.Empty;
                    }
            }
        }

        /// <summary>
        ///     <para>Applies the specified hash(es) to the current System.String, chaining multiple hashes</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashes">Recommended. List of hashes to applie to the current System.String.</param>
        /// <returns>Hashed System.String using the specified hash(es)</returns>
        public static String Hash(this String source, params HashType[] hashes)
        {
            String temp = String.Copy(source);

            foreach (var hash in hashes)
            {
                temp = temp.Hash(hash);
            }

            return temp;
        }

        /// <summary>
        ///     <para>Converts a System.String into an MD5 Hash</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>See: http://blog.stevex.net/c-code-snippet-creating-an-md5-hash-string/ for more details</para>
        /// </devdoc>
        internal static String ToMD5(this String source)
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