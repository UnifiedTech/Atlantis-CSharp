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
 * Contributor(s): Zack "Genesis2001" Loveless, Benjamin "aca20031" Buzbee,
 *      Mark "SniperFodder" Gunnett
 * 
 */

namespace Atlantis.Enterprise.Xcf
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     <para>Represents a section in an Xcf Configuration file</para>
    /// </summary>
    public class XcfSection
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        public XcfSection(XcfSection parent)
        {
            m_Data = new Dictionary<String, String>();
            Parent = parent;
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="include"></param>
        /// <param name="includePath"></param>
        public XcfSection(XcfSection parent, Boolean include, String includePath)
            : this(parent)
        {
            m_Include = include;
            m_Path = includePath;
        }

        #endregion

        #region Constants
        // Put all your constant declarations here
        #endregion

        #region Fields

        // TODO: Refactor! Refactor! Refactor!

        private String m_Path;
        private Boolean m_Include;
        private List<XcfSection> m_Sections;
        private Dictionary<String, String> m_Data;

        #endregion

        #region Properties

        /// <summary>
        ///     <para></para>
        /// </summary>
        public Double Version { get; protected set; }

        /// <summary>
        ///     <para>Gets or sets the parent of this Section</para>
        /// </summary>
        public XcfSection Parent { get; set; }

        #endregion

        #region Methods
        // Put your methods here, alphabetize them; however, sort private methods to the bottom, but alphabetize them still.
        #endregion
    }
}