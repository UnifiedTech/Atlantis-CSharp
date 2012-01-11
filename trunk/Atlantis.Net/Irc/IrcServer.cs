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
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class IrcServer : IrcClient
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Constructs an instance of an IrcServer</para>
        /// </summary>
        public IrcServer()
        {
#if DEBUG
            base.WriteLog = true;
#endif
        }

        #endregion

        #region Fields

        protected new IPEndPoint connection;

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets or sets the description of the pseudo-server</para>
        /// </summary>
        public string Description { get; set; }

        protected new string m_Host;
        /// <summary>
        ///     <para>Gets or sets the hostname for the pseudo-server</para>
        /// </summary>
        public override string Host
        {
            get { return m_Host; }
            set
            {
                // TODO: only allow it to be set once

                m_Host = value;
            }
        }

        /// <summary>
        ///     <para>Checks to see if the IrcServer is ready to begin connecting</para>
        /// </summary>
        public override bool IsInitialized
        {
            get
            {
                bool ret = base.IsInitialized; // Initialize the actual return value based upon the base's IsInitialized property ;)

                if (String.IsNullOrEmpty(Password)) ret = false;

                return ret;
            }
        }

        private string m_ServerName;
        /// <summary>
        ///     <para>Gets or sets the server's name when authenticating as a server</para>
        /// </summary>
        public string ServerName
        {
            get { return m_ServerName; }
            set
            {
                // TODO: Check whether we've called Start(), if so, cancel the set.

                m_ServerName = value;
            }
        }

        #endregion

        #region Methods

        protected override void OnDataReceive(string input)
        {
            // TODO: Parse server specific messages

            // We'll call "return" if we don't want a particular case to be validated through the base parser.
            base.OnDataReceive(input);
        }

        protected override void Register()
        {
            Send("PASS {0}", Password);

            Send("SERVER {0} 0 :[{1}] {2}", ServerName, Host, Description);

            // Introducing nicks.
            // Syntax (NICKv2+NICKIP): & nick hopcount timestamp username hostname server servicestamp +usermodes virtualhost nickipaddr :realname
        }

        /*public override bool Start()
        {
            if (!IsInitialized)
            {
                throw new NotImplementedException("The specified IrcClient has not yet been initialized yet.");
            }

            if (Port != null && Port.SslEnabled)
            {
                throw new NotImplementedException("SSL is not yet supported for server-to-server linking.");
            }

            try
            {
                connection = new IPEndPoint(Dns.GetHostEntry(Host).AddressList[0], Port.Port);
                socket.Connect(connection);
            }
            catch (SocketException)
            {
                // TODO: fire off disconnection event
            }

            thread = new Thread(new ParameterizedThreadStart(ParameterizedThreadCallback));
            thread.IsBackground = IsBackgroundThread;

            EventWaitHandle wait = new EventWaitHandle(false, EventResetMode.ManualReset);
            thread.Start(wait);
            wait.WaitOne(30000);

            return socket.Connected;
        }*/

        [Obsolete("IrcServer.Start and IrcServer.Stop are deprecated. Use the connect/disconnect methods to perform these options.")]
        public new bool Start()
        {
            return false;
        }

        #endregion
    }
}
