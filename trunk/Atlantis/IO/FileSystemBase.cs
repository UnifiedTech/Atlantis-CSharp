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
    using System;
    using System.IO;

    public abstract class FileSystemBase
    {
        #region Constructor(s)

        public FileSystemBase(String filepath)
        {
            Directory = Path.GetDirectoryName(filepath);
            Name = Path.GetFileName(filepath);
            FullName = Path.GetFullPath(filepath);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the directory the file is located</para>
        /// </summary>
        public String Directory { get; private set; }

        /// <summary>
        ///     <para>Gets whether the file exists or not</para>
        /// </summary>
        public Boolean Exists
        {
            get { return File.Exists(FullName); }
        }

        /// <summary>
        ///     <para>Gets the full path of the file</para>
        /// </summary>
        public String FullName { get; private set; }

        /// <summary>
        ///     <para>Gets the name of the file</para>
        /// </summary>
        public String Name { get; private set; }

        #endregion

        #region Methods

        public void Copy(String directory, Boolean overwrite)
        {
            File.Copy(FullName, Path.Combine(Directory, Name), overwrite);
        }

        #endregion
    }
}
