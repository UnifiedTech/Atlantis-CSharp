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
    using Atlantis.Security;

    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        /// <param name="passPhrase">Required. Secure phrase that indetifies</param>
        /// <param name="numCount">Optional. Indicates how many numbers will be appended to the end of the password</param>
        /// <param name="seedGen">Recommended. Used to generate a new random seed for better passwords.</param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>This function's concept was based on Star Trek command codes.</para>
        /// </devdoc>
        /// <returns></returns>
        public static String GenerateVoicePassword(String passPhrase, Int32 numCount = 1,
            Int32 seedGen = 1337)
        {
            StringBuilder passwd = new StringBuilder();
            passwd.Append(passPhrase);

            // Int32 seed = new Random(seedGen).Next(0, 999999999);
            Int32 alpha = (new Random().Next(0, NATO_ALPHABET.Length - 1));
            passwd.AppendFormat(" {0} ", NATO_ALPHABET[alpha]);

            if (numCount > 1)
            {
                for (Int32 i = 0; i < numCount; ++i)
                {
                    Int32 a = (new Random().Next(0, Password.NUMBERS.Length - 1));
                    System.Threading.Thread.Sleep(new Random().Next(2, 25));
                    passwd.AppendFormat("{0}-", Password.NUMBERS[a]);
                }

                passwd[passwd.Length - 1] = '\0';
            }
            else
            {
                Int32 a = (new Random().Next(0, Password.NUMBERS.Length - 1));
                passwd.Append(Password.NUMBERS[a]);
            }

            return passwd.ToString();
        }

        #endregion
    }
}
