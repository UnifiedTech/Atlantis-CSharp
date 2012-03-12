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

namespace Atlantis.TestApp.IniTests
{
    using Atlantis.IO;

    using System;

    public class IniFileTest
    {
        #region Methods

        public static void Main(string[] args)
        {
            IniFile ini = IniFile.Load("test.ini");

            Console.WriteLine("Total Sections: {0:d}", ini.Count);

            var display = ini["display"];
            foreach (var item in display)
            {
                Console.WriteLine("\t{0} => {1}", item.Key, item.Value);
            }

            Console.ReadKey(true);
        }

        #endregion
    }
}