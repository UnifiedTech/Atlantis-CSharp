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

namespace Atlantis.Enterprise.UpdaterServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [AttributeUsage(AttributeTargets.Assembly)]
    public class AtlantisVersionAttribute : Attribute
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Assigns the specified version string to assembly</para>
        /// </summary>
        public AtlantisVersionAttribute(String versionString, String humanVersion)
        {
            m_VersionString = versionString;
            m_HumanVersion = humanVersion;
        }

        #endregion

        #region Properties

        private String m_HumanVersion;
        /// <summary>
        ///     <para>Gets the human-readable version for the current assembly</para>
        /// </summary>
        public String HumanVersion
        {
            get { return m_HumanVersion; }
        }

        private String m_VersionString;
        /// <summary>
        ///     <para>Gets the version string for the current assembly</para>
        /// </summary>
        public String VersionString
        {
            get { return m_VersionString; }
        }

        #endregion
    }
}
