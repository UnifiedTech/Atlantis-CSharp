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

    public class XcfKey
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        public XcfKey(String name, object value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the name of the current key</para>
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     <para>Gets or sets the owner of the current key</para>
        /// </summary>
        public XcfSection Owner { get; set; }

        /// <summary>
        ///     <para>Gets the value of the current key</para>
        /// </summary>
        public Object Value { get; set; }

        #endregion

        #region Methods
        // Put your methods here, alphabetize them; however, sort private methods to the bottom, but alphabetize them still.
        #endregion
    }
}