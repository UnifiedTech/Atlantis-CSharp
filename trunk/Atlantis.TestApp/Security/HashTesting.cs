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

namespace Atlantis.TestApp.Security
{
    using Atlantis.Security;
    using Atlantis.Security.Linq;

    using System;

    public class HashTesting
    {
        #region Methods

        public static void Main(string[] args)
        {
            string @string = "The quick brown fox jumps over the lazy dog";
            Console.WriteLine("Input String: {0}", @string);

            string md5 = @string.Hash(HashType.MD5);
            Console.WriteLine("MD5 Version: {0}", md5);

            string sha1 = @string.Hash(HashType.SHA1);
            Console.WriteLine("SHA1 Version: {0}", sha1);

            string sha256 = @string.Hash(HashType.SHA256);
            Console.WriteLine("SHA256 Version: {0}", sha256);

            string tiger = @string.Hash(HashType.Tiger);
            Console.WriteLine("Tiger Version: {0}", tiger);

            string chain = @string.Hash(HashType.MD5, HashType.Tiger);
            Console.WriteLine("Chain (MD5 + Tiger): {0}", chain);


            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        #endregion
    }
}