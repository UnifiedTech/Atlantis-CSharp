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

namespace Atlantis.Net.Irc
{
    using System;

    public partial class IrcClient
    {
        #region Constants

        /// <summary>
        ///     <para>The control code for generic messages</para>
        ///     <para>Examples this is used in: CTCP requests/responses</para>
        /// </summary>
        public const char CONTROL_GENERIC = (char)1;

        /// <summary>
        /// Makes all text between CONTROL_BOLD and CONTROL_BOLD or CONTROL_CANCEL bold in mIRC clients.
        /// </summary>
        public const char CONTROL_BOLD = (char)2;

        /// <summary>
        /// Makes all text between CONTROL_COLOR and CONTROL_COLOR or CONTROL_CANCEL the color represented by the numbers which follow it in mIRC clients.
        /// </summary>
        public const char CONTROL_COLOR = (char)3;

        /// <summary>
        /// Ends all CONTROL Character effects.
        /// </summary>
        public const char CONTROL_CANCEL = (char)15;

        /// <summary>
        /// Makes all text between CONTROL_REVERSE and CONTROL_REVERSE or CONTROL_CANCEL reversed in color (foreground = background, background = foreground) in mIRC clients.
        /// </summary>
        public const char CONTROL_REVERSE = (char)22;

        /// <summary>
        /// Makes all text between CONTROL_ITALICS and CONTROL_ITALICS or CONTROL_CANCEL italicized in mIRC clients.
        /// </summary>
        public const char CONTROL_ITALICS = (char)29;

        /// Makes all text between CONTROL_UNDERSCORE and CONTROL_UNDERSCORE or CONTROL_CANCEL underscored in mIRC clients.
        public const char CONTROL_UNDERSCORE = (char)31;

        #endregion
    }
}