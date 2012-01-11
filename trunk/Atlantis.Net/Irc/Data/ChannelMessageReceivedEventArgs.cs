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

    public class ChannelMessageReceivedEventArgs : EventArgs
    {

        #region Constructor(s)

        public ChannelMessageReceivedEventArgs(Channel channel, string message, string user, bool notice = false, bool ctcp = false)
        {
            m_IsCtcp = ctcp;
            m_IsNotice = notice;
            m_Message = message;
            m_Target = channel;
            m_User = user;
        }

        #endregion

        #region Properties

        private bool m_IsCtcp;
        /// <summary>
        ///     <para>Gets a value indicating whether this was a CTCP sent to the channel</para>
        /// </summary>
        public bool IsCTCP
        {
            get { return m_IsCtcp; }
        }

        private bool m_IsNotice;
        /// <summary>
        ///     <para>Gets a value indicating whether the message was sent as a Channel Notice</para>
        /// </summary>
        public bool IsNotice
        {
            get { return m_IsNotice; }
        }

        private string m_Message;
        /// <summary>
        ///     <para>Gets the message of the event</para>
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }

        private Channel m_Target;
        /// <summary>
        ///     <para>Gets the target of the channel message</para>
        /// </summary>
        public Channel Target
        {
            get { return m_Target; }
        }

        private string m_User;
        /// <summary>
        ///     <para>Gets the user that triggered the event</para>
        /// </summary>
        public string User
        {
            get { return m_User; }
        }

        #endregion

    }
}
