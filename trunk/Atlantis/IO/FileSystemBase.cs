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

        /// <summary>
        ///     <para>Constructs a new FileSystemBase object with generic file I/O operations</para>
        /// </summary>
        /// <param name="filepath">Required. Complete file path of the file being modified. If Empty, call Initialize() in a derived class.</param>
        public FileSystemBase(string filepath)
        {
            if (!String.IsNullOrEmpty(filepath))
            {
                Directory = Path.GetDirectoryName(filepath);
                Name = Path.GetFileName(filepath);
                FullName = Path.GetFullPath(filepath);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the directory the file is located</para>
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        ///     <para>Gets whether the file exists or not</para>
        /// </summary>
        public bool Exists
        {
            get { return File.Exists(FullName); }
        }

        /// <summary>
        ///     <para>Gets the full path of the file</para>
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        ///     <para>Gets the name of the file</para>
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Copies the current file to the specified directory, overwriting if required.</para>
        /// </summary>
        /// <param name="directory">Required. Specifies the directory to copy the current file.</param>
        /// <param name="overwrite">Recommended. In case the file already exists at the specified directory, specify whether to overwrite it.</param>
        public void Copy(string directory, bool overwrite = false)
        {
            File.Copy(FullName, Path.Combine(Directory, Name), overwrite);
        }

        /// <summary>
        ///     <para>Deletes the current file from the file system</para>
        /// </summary>
        public void Delete()
        {
            if (Exists)
            {
                File.Delete(FullName);
            }
        }

        /// <summary>
        ///     <para>Available in case String.Empty is passed to the constructor</para>
        /// </summary>
        /// <param name="filepath"></param>
        protected void Initialize(string filepath)
        {
            if (!String.IsNullOrEmpty(filepath) && String.IsNullOrEmpty(Name))
            {
                Directory = Path.GetDirectoryName(filepath);
                Name = Path.GetFileName(filepath);
                FullName = Path.GetFullPath(filepath);
            }
        }

        /// <summary>
        ///     <para>Moves the current file to the specified directory, performing a rename if specified</para>
        /// </summary>
        /// <param name="directory">Required. Specifies the directory of the file move.</param>
        /// <param name="name">Optional. If a rename operation is desired, will perform a rename at the same time.</param>
        /// <param name="overwrite">Recommended. If a rename is desired, this specifies whether to overwrite any existing file in the new directory.</param>
        /// <exception cref="System.IO.IOException" />
        public void Move(string directory, string name = "", bool overwrite = false)
        {
            string newFile = Path.Combine(directory, (String.IsNullOrEmpty(name) ? (Name = name) : Name));
            if (File.Exists(newFile) && overwrite)
            {
                File.Delete(newFile);
            }
            else if (File.Exists(newFile))
            {
                throw new IOException("File already exists at specified directory and overwrite flag not specified.");
            }

            File.Move(FullName, newFile);
            FullName = newFile;
        }

        /// <summary>
        ///     <para>Renamed the current file to the specified name</para>
        /// </summary>
        /// <param name="name">Required. The name to rename the current file.</param>
        public void Rename(string name)
        {
            File.Copy(FullName, Path.Combine(Directory, name));
            File.Delete(FullName);

            Name = name;
            FullName = Path.Combine(Directory, name);
        }

        #endregion
    }
}
