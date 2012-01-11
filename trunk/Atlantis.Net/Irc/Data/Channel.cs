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

    public class Channel
    {

        #region Constructor(s)

        public Channel(string name, IrcClient client)
        {
            this.client = client;
            m_Name = name;
            initialTopicSet = false;
        }

        #endregion

        #region Fields

        protected IrcClient client;

        protected bool initialTopicSet;

        #endregion

        #region Properties

        private List<ListMode> m_ListModes;
        /// <summary>
        ///     <para></para>
        /// </summary>
        public List<ListMode> ListModes
        {
            get
            {
                if (m_ListModes == null)
                {
                    m_ListModes = new List<ListMode>();
                }

                return m_ListModes;
            }
        }

        private Dictionary<char, string> m_Modes;
        /// <summary>
        ///     <para>Gets a list of modes set on the channel with their specified parameters</para>
        /// </summary>
        public Dictionary<char, string> Modes
        {
            get
            {
                if (m_Modes == null)
                {
                    m_Modes = new Dictionary<char, string>();
                }

                return m_Modes;
            }
        }


        private string m_Name;
        /// <summary>
        ///     Gets the name of the of the current Channel
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            protected set { m_Name = value; }
        }

        private string m_Topic;
        /// <summary>
        ///     <para>Gets or sets the topic of the channel</para>
        ///     <para>Will, if being set, send a TOPIC command to the server to change the topic.</para>
        /// </summary>
        public string Topic
        {
            get { return m_Topic; }
            set
            {
                if (!initialTopicSet)
                {
                    m_Topic = value;
                    initialTopicSet = true;
                }
                else
                {

                }
                // TODO: Logic to either send or update internally the topic
            }
        }


        private Dictionary<string, PrefixList> m_Users;
        /// <summary>
        ///     <para>Gets a list of users on the current channel</para>
        /// </summary>
        public Dictionary<string, PrefixList> Users
        {
            get
            {
                if (m_Users == null)
                {
                    m_Users = new Dictionary<string, PrefixList>();
                }

                return m_Users;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Sends a message to the current channel</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Message(string format, params object[] args)
        {
            if (client != null && client.Connected)
            {
                return client.Send("PRIVMSG {0} :{1}", Name, String.Format(format, args));
            }
            return false;
        }

        #endregion

    }
}
