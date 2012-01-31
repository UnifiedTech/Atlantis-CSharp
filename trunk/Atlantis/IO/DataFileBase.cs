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

    public abstract class DataFileBase : FileSystemBase
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Creates a new DataFileBase instance fromt he specified file path</para>
        /// </summary>
        /// <param name="filepath">Required. </param>
        protected DataFileBase(String filepath)
            : base(filepath)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para></para>
        /// </summary>
        public abstract void Load();

        /// <summary>
        ///     <para></para>
        /// </summary>
        public abstract void Save();

        #endregion
    }
}