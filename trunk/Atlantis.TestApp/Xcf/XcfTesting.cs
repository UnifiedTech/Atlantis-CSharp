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

namespace Atlantis.TestApp.Xcf
{
    using Atlantis.Security;
    using Atlantis.Security.Linq;
    using Atlantis.Enterprise.Xcf;
    using Atlantis.Enterprise.Xcf.Collections;

    using System;
    using System.IO;
    using System.Text;

    public class XcfTesting
    {
        #region Methods

        public static void Main(string[] args)
        {
            XcfBuilder builder = new XcfBuilder();

            Console.WriteLine("Starting XcfBuilder.");
            Console.WriteLine("Xcf Version: {0:0.0} - Encoding: {1}", builder.Version, builder.Encoding);
            for (int i = 0; i < 5; ++i)
            {
                int seed = Password.GenerateSecureNumber();
                Random rnd = new Random(seed);
                Console.WriteLine("\tRandom seed chosen: {0}", seed);


                XcfSection section = new XcfSection(rnd.Next().ToString().Hash(HashType.SHA256).Substring(0, 15));
                Console.WriteLine("\tCreating section {0}...", section.Name);

                for (int j = 0; j < 10; ++j)
                {
                    XcfKey key = new XcfKey(rnd.Next().ToString().Hash(HashType.SHA256).Substring(0, 10), true);
                    section.AddKey(key);
                    Console.WriteLine("\t\tCreating key ({1}) for section {0}...", section.Name, key.Name);
                }

                for (int k = 0; k < 5; ++k)
                {
                    XcfSection subSection = new XcfSection(rnd.Next().ToString().Hash(HashType.SHA256).Substring(0, 15));
                    Console.WriteLine("\t\tCreating section {0}...", subSection.Name);

                    for (int j = 0; j < 10; ++j)
                    {
                        XcfKey key = new XcfKey(rnd.Next().ToString().Hash(HashType.SHA256).Substring(0, 10), rnd.Next().ToString().Hash(HashType.MD5).Substring(0, 5));
                        subSection.AddKey(key);
                        Console.WriteLine("\t\t\tCreating key ({1}) for section {0}...", subSection.Name, key.Name);
                    }

                    section.AddSection(subSection);
                }

                Console.WriteLine("Writing {0} to buffer.", section.Name);
                builder.WriteSection(section);
            }

            Console.WriteLine("Writing builder to output file.");
            File.WriteAllText("data.conf", builder.ToString());

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();

            Environment.Exit(1);
        }

        #endregion
    }
}