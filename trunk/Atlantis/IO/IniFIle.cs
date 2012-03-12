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

namespace Atlantis.IO
{
    using Atlantis.Linq;

    using System;
    using System.Collections.Generic;
    using System.IO;

    public class IniFile : DataFileBase
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Creates a new abstraction of the IniFile class</para>
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <param name="pAutoLoad"></param>
        public IniFile(string pFilePath, bool pAutoLoad = true)
            : base(pFilePath)
        {
            m_FileBits = new Dictionary<string, Dictionary<string, string>>();

            if (pAutoLoad)
            {
                Load();
            }
        }

        #endregion

        #region Constants
        // Put all your constant declarations here
        #endregion

        #region Fields

        private Dictionary<string, Dictionary<string, string>> m_FileBits;

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Returns an entire INI section of Key-Value pairs</para>
        /// </summary>
        /// <param name="pSection"></param>
        /// <returns></returns>
        public Dictionary<string, string> this[string pSection]
        {
            get
            {
                if (m_FileBits.ContainsKey(pSection))
                {
                    return m_FileBits[pSection];
                }

                throw new KeyNotFoundException("The specified INI Section was not found.");
            }
        }

        /// <summary>
        ///     <para>Gets a value from the specified section of the IniFile</para>
        /// </summary>
        /// <param name="pSection"></param>
        /// <param name="pKey"></param>
        /// <returns></returns>
        public string this[string pSection, string pKey]
        {
            get
            {
                Dictionary<string, string> section = m_FileBits[pSection];
                if (section != null && section.ContainsKey(pKey))
                {
                    return section[pKey];
                }

                throw new KeyNotFoundException("The specified INI Section was not found.");
            }
        }

        /// <summary>
        ///     <para>Gets a value indicating how many sections are present.</para>
        /// </summary>
        public int Count
        {
            get { return m_FileBits.Count; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Loads the specified INI file into memory.</para>
        ///     <para>Reads all lines, except comments, into memory. When saving, comments will be destroyed.</para>
        /// </summary>
        /// <devdoc>
        ///     <para>TODO: Allow comments to be saved.</para>
        /// </devdoc>
        /// <exception cref="System.IO.FileNotFoundException" />
        public override void Load()
        {
            StringReader reader = new StringReader(File.ReadAllText(FullName));

            if (reader != null)
            {
                string line = null;
                Dictionary<string, string> currentSection = null;
                string section = null;
                while ((line = reader.ReadLine().Trim()) != null)
                {
                    if (line.Matches(@"\[([^ ]+)\]"))
                    {       // Matches the section header
                        if (currentSection != null && !String.IsNullOrEmpty(section)
                            && !m_FileBits.ContainsKey(section))
                        {       // we found another header and we already read the kvp's for the prev. section.
                                // write to buffer and clear.
                            m_FileBits.Add(section, currentSection);
                            currentSection.Clear();
                        }
                        else if (currentSection == null)
                        {
                            currentSection = new Dictionary<string, string>();
                        }
                        else if (m_FileBits.ContainsKey(section))
                        {
                            throw new DuplicateIniSectionException(section);
                        }

                        section = line.Substring(1, line.Length - 1);
                    }
                    else if (line.Matches(@"(.+)=(.+)"))
                    {       // Matches at least one character for key=value
                            // won't match empty keys or values.
                        string key = line.Substring(0, line.IndexOf("=") - 1).Trim();
                        string value = line.Substring(line.IndexOf("=") + 1).Trim();

                        if (currentSection == null)
                        {
                            currentSection = new Dictionary<string, string>();
                        }

                        currentSection.Add(key, value);
                    }
                }
            }
        }

        /// <summary>
        ///     <para>Loads a new IniFile from the specified file.</para>
        /// </summary>
        /// <param name="pFile"></param>
        /// <returns></returns>
        public static IniFile Load(string pFile)
        {
            return new IniFile(pFile, true);
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}