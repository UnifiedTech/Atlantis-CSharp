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

    public enum AlphaType : int
    {
        ALPHA_ALL,
        ALPHA_UP,
        ALPHA_LOW,
    }

    /// <summary>
    ///     <para>Contains methods relating to password management</para>
    /// </summary>
    public static class Password
    {

        #region Constants

        /// <summary>
        ///     <para>Represents the English Alphabet in both upper- and lowercase forms</para>
        /// </summary>
        public readonly static String ALPHABET_ALL = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        ///     <para>Represents the English Alphabet in uppercase form</para>
        /// </summary>
        public readonly static String ALPHABET_UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        ///     <para>Represents the English Alphabet in lowercase form</para>
        /// </summary>
        public readonly static String ALPHABET_LOWER = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        ///     <para>List of numbers from zero to nine</para>
        /// </summary>
        public readonly static Int32[] NUMBERS = new Int32[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Generates a secure password of a random length using the specified allowed characters and symbols</para>
        /// </summary>
        /// <param name="passPhrase">Required. Secure string used to ______________</param>
        /// <param name="minLength">Required. Minimum length for the password</param>
        /// <param name="maxLength">Required. Maximum length for the password</param>
        /// <param name="allowedAlpha">
        ///     <para>Recommended. Specifies what type of characters to use.</para>
        ///     <para>Defaults to any alphabet character, both upper- and lowercases.</para>
        /// </param>
        /// <param name="inclNums">
        ///     <para>Recommended. Specifies whether to include numbers in the password.</para>
        ///     <para>Defaults to including numbers in the password.</para>
        /// </param>
        /// <param name="allowedSymbols">Recommended. Specifies whether to allow symbols in the password, and if so, what symbols to include.</param>
        /// <param name="minChars">Optional. Number of minimum symbols to include in the password</param>
        /// <returns></returns>
        public static Char[] Generate(String passPhrase, Int32 minLength, Int32 maxLength, AlphaType allowedAlpha = AlphaType.ALPHA_ALL,
            Boolean inclNums = true, String allowedSymbols = "", Int32 minChars = 0)
        {
            StringBuilder sb = new StringBuilder();
            Int32 length = (new Random().Next(minLength, maxLength));

            StringBuilder charString = new StringBuilder();
            switch (allowedAlpha)
            {
                case AlphaType.ALPHA_ALL: { charString.Append(Password.ALPHABET_ALL); } break;
                case AlphaType.ALPHA_LOW: { charString.Append(Password.ALPHABET_LOWER); } break;
                case AlphaType.ALPHA_UP: { charString.Append(Password.ALPHABET_UPPER); } break;
            }

            Boolean symbols = false;
            if (!String.IsNullOrEmpty(allowedSymbols))
            {
                charString.Append(allowedSymbols);
                symbols = true;
            }

            if (inclNums)
            {
                // see: http://stackoverflow.com/a/145864/63609
                charString.Append(String.Join("", Password.NUMBERS.Select(x => x.ToString()).ToArray()));
            }

            String chars = charString.ToString();

            for (Int32 i = 0; i <= length; ++i)
            {
                // something
            }

            return sb.ToString().ToCharArray();
        }

        #endregion
    }
}