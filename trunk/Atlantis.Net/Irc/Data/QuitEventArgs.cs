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


    public class QuitEventArgs : EventArgs
    {

        #region Constructor(s)

        public QuitEventArgs(string nick, string message)
        {
            m_Nick = nick;
            m_Message = message;
        }

        #endregion

        #region Properties

        private string m_Nick;
        /// <summary>
        ///     <para>Gets the nick of the user who quit</para>
        /// </summary>
        public string Nick
        {
            get { return m_Nick; }
        }

        private string m_Message;
        /// <summary>
        ///     <para>Gets the message associated with the quit</para>
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }

        #endregion

    }
}
