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

    public class XcfParserException : Exception
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        public XcfParserException(String message)
            : base(message)
        {
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public XcfParserException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     <para>Constructs a new XcfParserException with the specified exception</para>
        /// </summary>
        /// <param name="xEvent"></param>
        /// <param name="token"></param>
        /// <param name="line"></param>
        public XcfParserException(XcfEvent xEvent, String token, String file = "", Int32 line = 0)
            : this(String.Format("Expected '{0}' but found '{1}' instead. File {2}. Line {3}.", "", token, file, line))
        {
        }

        #endregion

        #region Constants

        // TODO: Learn Serializing
        public const Int64 SERIAL_VERSION_UID = 201109290648L;

        #endregion
    }
}