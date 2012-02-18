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

namespace Atlantis.Security
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;

    /// <summary>
    ///     <para>Contains methods relating to password management</para>
    /// </summary>
    public static class Password
    {

        #region Constants

        /// <summary>
        ///     <para>Represents the English Alphabet in lowercase form</para>
        /// </summary>
        public readonly static string ALPHABET_LOWER = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        ///     <para>Represents the English Alphabet in uppercase form</para>
        /// </summary>
        public readonly static string ALPHABET_UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        ///     <para>Represents the list of special characters on a traditional keyboard</para>
        /// </summary>
        public readonly static string SYMBOLS = "~`!@#$%^&*()_+=-";

        /// <summary>
        ///     <para>List of numbers from zero to nine</para>
        /// </summary>
        public readonly static string NUMBERS = "1234567890";

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Generates a secure password of a random length using the specified allowed characters and symbols</para>
        /// </summary>
        /// <param name="passPhrase">Required. Secure string &lt;something&gt;</param>
        /// <param name="minLength">Required. Minimum length for the password</param>
        /// <param name="maxLength">Required. Maximum length for the password</param>
        /// <returns></returns>
        public static char[] Generate(string passPhrase, int minLength, int maxLength)
        {
            StringBuilder passwd = new StringBuilder();

            int seed = Password.GenerateSecureNumber();
            Random rnd = new Random(seed);

            char[][] passwordChars = new char[][]
            {
                ALPHABET_LOWER.ToCharArray(),
                ALPHABET_UPPER.ToCharArray(),
                NUMBERS.ToCharArray(),
                // SYMBOLS.ToCharArray(),
            };

            int length = rnd.Next(minLength, maxLength);
            for (int i = 0; i <= length; ++i)
            {

            }

            return passwd.ToString().ToCharArray();
        }

        /// <summary>
        ///     <para>Generates a secure number that can be used as a random seed</para>
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>This was generally taken from another website, based on what they suggested for a strong password.</para>
        /// </devdoc>
        public static int GenerateSecureNumber()
        {
            byte[] tmp = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(tmp);

            return (int)((tmp[0] & 0x7f) << 24 | tmp[1] << 16 | tmp[2] << 8 | tmp[3]); // TODO: Learn what this means.
        }

        #endregion
    }
}