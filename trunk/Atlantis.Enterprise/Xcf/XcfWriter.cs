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

    [Obsolete("This could be cut - Try using XcfDocument.Save() instead")]
    public partial class XcfWriter
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="stream"></param>
        public XcfWriter(Stream stream)
        {
            // TODO: Possibly encrypt 
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="builder"></param>
        public XcfWriter(Stream stream, XcfBuilder builder)
            : this(stream)
        {
            // 
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="document"></param>
        public XcfWriter(Stream stream, XcfDocument document)
            : this(stream)
        {
            // TODO: Take an XcfDocument and save it

            throw new NotImplementedException("XcfDocument not yet implemented thus cannot pass to an XcfWriter at this time");
        }

        #endregion

        #region Fields

        private XcfBuilder m_Builder;

        #endregion

        #region Properties

        // 

        #endregion

        #region Methods

        /// <summary>
        ///     <para></para>
        /// </summary>
        public void WriteDocType()
        {
        }

        #endregion
    }
}
