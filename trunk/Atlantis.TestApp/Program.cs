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

namespace Atlantis.TestApp
{
    using Atlantis.Security;

    using System;
    using System.Text;

    public class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            const int PRODUCTKEY_SEGMENT_LENGTH = 5;
            const int PRODUCTKEY_SEGMENT_COUNT = 5;
            int seed = Password.GenerateSecureNumber();

            StringBuilder valid = new StringBuilder();
            valid.Append(Password.ALPHABET_UPPER);
            valid.Append(Password.NUMBERS);

            string alpha = Password.ALPHABET_LOWER;
            string numbs = Password.NUMBERS;

            string vseq = valid.ToString();

            StringBuilder productKey = new StringBuilder();

            Random rnd = new Random(seed);

            for (int o = 0; o < PRODUCTKEY_SEGMENT_COUNT; ++o)
            {
                for (int i = 0; i < PRODUCTKEY_SEGMENT_LENGTH; ++i)
                {
                    if (char.IsNumber(productKey[productKey.Length - 1]))
                    {

                    }


                    productKey.Append(vseq[rnd.Next(vseq.Length - 1)]);
                }

                productKey.Append('-');
            }

            Console.WriteLine("Product Key: {0}", productKey.ToString());
        }

        #endregion
    }
}