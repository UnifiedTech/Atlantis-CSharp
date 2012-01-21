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

    #region XcfEventData

    /// <summary>
    ///     <para>Represents returned metadata from the XcfEvent enumeration</para>
    /// </summary>
    public class XcfEventData
    {
        #region Constructor(s)
        public XcfEventData(String name, String keyword)
        {
            Name = name;
            Keyword = keyword;
        }
        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the keyword used to identify the current event</para>
        /// </summary>
        public String Keyword { get; protected set; }

        /// <summary>
        ///     <para>Gets the name of the current event</para>
        /// </summary>
        public String Name { get; protected set; }

        #endregion
    }

    #endregion

    /// <summary>
    ///     <para>Represents metadata about an XcfEvent</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class XcfEventMetaAttribute : Attribute
    {
        #region Constructor(s)

        public XcfEventMetaAttribute(String name, String keyword)
        {
            Name = name;
            Keyword = keyword;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the keyword used to identify the current event</para>
        /// </summary>
        public String Keyword { get; protected set; }

        /// <summary>
        ///     <para>Gets the name of the current event</para>
        /// </summary>
        public String Name { get; protected set; }

        #endregion
    }
}
