﻿/*
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

namespace Atlantis.Net.Irc.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NickChangeEventArgs : EventArgs
    {

        #region Constructor(s)

        public NickChangeEventArgs(string oldNick, string newNick)
        {
            m_OldNick = oldNick;
            m_NewNick = newNick;
        }

        #endregion

        #region Properties

        private string m_NewNick;
        /// <summary>
        ///     <para>Gets the new nick that the user changed to</para>
        /// </summary>
        public string NewNick
        {
            get { return m_NewNick; }
        }

        private string m_OldNick;
        /// <summary>
        ///     <para>Gets the old nick of the user</para>
        /// </summary>
        public string OldNick
        {
            get { return m_OldNick; }
        }

        #endregion

    }
}
