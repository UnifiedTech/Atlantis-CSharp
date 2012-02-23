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

namespace Atlantis.Enterprise.Xcf
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class XcfReader
    {
        #region Constructor(s)

        public XcfReader()
            : this(String.Empty, Encoding.UTF8)
        {
        }

        public XcfReader(string filepath, Encoding encoding)
        {
            // TODO: Complete member initialization
            m_File = filepath;
            m_Encoding = encoding;
        }

        #endregion

        #region Constants

        // groups: (1) name of block, (2) content of block.
        private const string RegexMatchBlocks = @"";

        // groups: (1) name of key, (2) value
        private const string RegexKeyValuePair = @"";

        #endregion

        #region Fields

        private string m_File;
        private Encoding m_Encoding;

        #endregion

        #region Properties
        // Put your public properties (keyword: PUBLIC)
        #endregion

        #region Methods

        protected void Read(string input)
        {
            // TODO: Regex match sections
            // TODO: Regex match key-value pairs

            MatchCollection matches = Regex.Matches(input, RegexMatchBlocks);
            foreach (Match m in matches)
            {
                // iterate over the matches.

                // if (Regex.IsMatch(m.Groups[2].Value, RegexMatchBlocks)) {
            }
        }



        #endregion
    }
}