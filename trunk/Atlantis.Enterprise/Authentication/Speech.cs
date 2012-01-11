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

namespace Atlantis.Enterprise.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Speech
    {

        #region Constants

        /// <summary>
        ///     <para>List of English alphabet characters, capitals only</para>
        /// </summary>
        public readonly static string[] ALPHABET = new string[26] {
            "A", "B", "C", "D", "E", "F",
            "G", "H", "I", "J", "K", "L",
            "M", "N", "O", "P", "Q", "R",
            "S", "T", "U", "V", "W", "X",
            "Y", "Z"
        };

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
            "Alfa", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot",
            "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima",
            "Mike", "November", "Oscar", "Papa", "Quebec", "Romeo",
            "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-Ray",
            "Yankee", "Zulu",
        };

        /// <summary>
        ///     <para>List of numbers from zero to nine</para>
        /// </summary>
        public readonly static int[] NUMBERS = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        #endregion

        #region Properties
        #endregion

        #region Methods

        /// <summary>
        ///     <para>Gets a random password that is meant to be voice printed with a particular individual's voice</para>
        ///     <para>Warning: Not suggested to be used as an actual password without voice printing nor text-input</para>
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static string GetPassword(string lastName)
        {
            StringBuilder pwd = new StringBuilder();
            pwd.Append(lastName);

            Int32 alpha = (new Random().Next(0, 25));
            pwd.AppendFormat(" {0}", NATO_ALPHABET[alpha]);

            Int32 numeric = (new Random().Next(0, 9));
            pwd.AppendFormat(" {0}", NUMBERS[numeric]);

            return pwd.ToString();
        }

        #endregion
    }
}
