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

namespace Atlantis.Net.Irc.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class JoinPartEventArgs : EventArgs
    {

        #region Constructor(s)

        public JoinPartEventArgs(string channel, string nick)
        {
            m_Channel = channel;
            m_Nick = nick;
        }

        #endregion

        #region Properties

        private string m_Channel;
        /// <summary>
        ///     <para>Gets the channel that was joined or parted</para>
        /// </summary>
        public string Channel
        {
            get { return m_Channel; }
        }

        private string m_Nick;
        /// <summary>
        ///     <para>Gets the nick that joined or parted the channel</para>
        /// </summary>
        public string Nick
        {
            get { return m_Nick; }
        }

        #endregion

    }
}
