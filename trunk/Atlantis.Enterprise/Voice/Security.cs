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

namespace Atlantis.Enterprise.Voice
{
    using Atlantis.Linq;
    using Atlantis.Security;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class Security
    {
        #region Constants

        /// <summary>
        ///     <para>List of the Greek Alphabet names</para>
        /// </summary>
        /// <devdoc>
        ///     <para>These need to be double checked and made sure there are all of them and that they're in phonetic form</para>
        /// </devdoc>
        public readonly static string[] GREEK_ALPHABET = new string[22] {
            "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta",
            "Eta", "Theta", "Iota", "Kappa", "Lambda", "Mu",
            "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma",
            "Tau", "Upsilon", "Psi", "Omega"
        };

        /// <summary>
        ///     <para>The English alphabet in phonetic form</para>
        /// </summary>
        public readonly static string[] NATO_ALPHABET = new string[26] {
            "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot",
            "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima",
            "Mike", "November", "Oscar", "Papa", "Quebec", "Romeo",
            "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-Ray",
            "Yankee", "Zulu",
        };

        #endregion

        #region Methods


        /// <summary>
        ///     <para>Generates a random password using the specified pass phrase as a base</para>
        ///     <para>This provides a voice printed password that is to be used by the voice printing library</para>
        /// </summary>
        /// <param name="name">Required. Secure phrase that indetifies</param>
        /// <param name="strength">Recommended. The strength of the password and complexity it will be.</param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>This function's concept was based on Star Trek command codes.</para>
        /// </devdoc>
        /// <returns></returns>
        public static string GeneratePassword(String name)
        {
            StringBuilder passwd = new StringBuilder();
            passwd.Append(name);
            passwd.Append(' ');// space, derp.
            // ex. Janeway Pi-Alpha 4-3-4
            byte[] tmp = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(tmp);

            int seed = (tmp[0] & 0x7f) << 24 | tmp[1] << 16 | tmp[2] << 8 | tmp[3]; // TODO: Learn what this means.
            Random rnd = new Random(seed);

            int countGreek = rnd.Next(0, 3);
            for (int i = 0; i <= countGreek; ++i)
            {
                passwd.Append(GREEK_ALPHABET[rnd.Next(0, GREEK_ALPHABET.Length - 1)]);
                if (i != countGreek)
                    passwd.Append('-');
                else
                    passwd.Append(' ');
            }

            int countNum = rnd.Next(2, 4);
            for (int i = 0; i < countNum; ++i)
            {
                passwd.Append(Password.NUMBERS[rnd.Next(0, Password.NUMBERS.Length - 1)]);
                if (i != (countNum - 1))
                    passwd.Append('-');
            }
            
            return passwd.ToString();
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <returns></returns>
        public static string GenerateClearancePassword()
        {
            StringBuilder passwd = new StringBuilder();
            // example: Alpha-Sigma-Alpha 3-1-4

            return passwd.ToString();
        }

        public static int GenerateSecureNumber()
        {
            RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();

            return 0;
        }

        #endregion
    }
}
