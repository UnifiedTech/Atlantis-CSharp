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
    using Atlantis.Security;

    using System;
    using System.Text;
    using System.Security.Cryptography;

    public enum HashType : int
    {
        MD5 = 1,
        SHA1,
        SHA256,
        Tiger,
    }

    public static partial class Extensions
    {
        #region Methods

        /// <summary>
        ///     <para>Applies the specified hash to the current System.String</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashType">Required. Defines which hasing algorithm to use. Defaults to MD5</param>
        /// <param name="salt">[Out] Returns the salt used</param>
        /// <returns></returns>
        public static string Hash(this string source, HashType hashType)
        {
            HashAlgorithm hash;
            switch (hashType)
            {
                case HashType.SHA1: { hash = new SHA1Managed(); } break;
                case HashType.SHA256: { hash = new SHA256Managed(); } break;
                case HashType.Tiger: { hash = new TigerManaged(); } break;
                case HashType.MD5:
                default: { hash = new MD5CryptoServiceProvider(); } break;
            }

            byte[] data = Encoding.UTF8.GetBytes(source);
            byte[] hashed = hash.ComputeHash(data);

            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < hashed.Length; ++i)
            {
                ret.Append(hashed[i].ToString("X2"));
            }

            return ret.ToString();
        }

        /// <summary>
        ///     <para>Applies the specified hash(es) to the current System.String, chaining multiple hashes</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashes">Required. List of hashes to applie to the current System.String.</param>
        /// <returns>Hashed System.String using the specified hash(es)</returns>
        public static string Hash(this string source, params HashType[] hashes)
        {
            string temp = String.Copy(source);

            foreach(var hash in hashes)
            {
                temp = temp.Hash(hash);
            }

            return temp;
        }


        [Obsolete("Keeping this, but it is now obsolete.")]
        /// <summary>
        ///     <para>Converts a System.String into an MD5 Hash</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>See: http://blog.stevex.net/c-code-snippet-creating-an-md5-hash-string/ for more details</para>
        /// </devdoc>
        internal static string ToMD5(this string source)
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