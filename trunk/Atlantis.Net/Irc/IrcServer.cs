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
    using Atlantis.IO;
    using Atlantis.Linq;

    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     <para>Represents the basic IrcServer according to RFC1459</para>
    /// </summary>
    public partial class IrcServer : IrcClient
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Constructs an instance of an IrcServer</para>
        /// </summary>
        public IrcServer()
            : base()
        {
#if DEBUG
            base.WriteLog = true;
            Framework.Console.Info("IrcServer constructor");
#else
            base.WriteLog = false;
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
        public String Description { get; set; }

        /// <summary>
        ///     <para>Gets or sets the hostname for the pseudo-server</para>
        /// </summary>
        public new String Host
        {
            get { return base.Host; }
            set
            {
                // TODO: only allow it to be set once
                base.Host = value;
            }
        }

        /// <summary>
        ///     <para>Checks to see if the IrcServer is ready to begin connecting</para>
        /// </summary>
        public override Boolean IsInitialized
        {
            get
            {
                Boolean ret = true;     // Normally, this would be base.IsIntialized - but we don't need to worry about Nick being null/empty
                if (String.IsNullOrEmpty(Host)) ret = false;
                else if (String.IsNullOrEmpty(Password)) ret = false;

                return ret;
            }
        }

        private String m_ServerName;
        /// <summary>
        ///     <para>Gets or sets the server's name when authenticating as a server</para>
        /// </summary>
        public String ServerName
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

        protected override void OnDataReceive(String input)
        {
            // TODO: Parse server specific messages
            Match m, n;
            String[] toks = input.Split(' ');

            if (toks[0].EqualsIgnoreCase("NICK"))
            {
                m_Logger.Debug("nick -> {0}", input);
                /*
                 * -06:09:05- DEBUG nick -> NICK OperServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Operator Server
                 * -06:09:05- DEBUG nick -> NICK NickServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Nickname Server
                 * -06:09:05- DEBUG nick -> NICK ChanServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Channel Server
                 * -06:09:05- DEBUG nick -> NICK HostServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :vHost Server
                 * -06:09:05- DEBUG nick -> NICK MemoServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Memo Server
                 * -06:09:05- DEBUG nick -> NICK BotServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Bot Server
                 * -06:09:05- DEBUG nick -> NICK HelpServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Help Server
                 * -06:09:05- DEBUG nick -> NICK Global 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Global Noticer
                 * -06:09:05- DEBUG nick -> NICK UnifiedTech 2 1324341431 irc unfiiedtech.org services.unifiedtech.org 0 :UnifiedTech.org
                 * 
                 * -06:09:05- DEBUG nick -> NICK CIA-1 1 1328655612 ~CIA 204.152.223.100 irc.unifiedtech.org 1 :CIA Bot (http://cia.vc)
                 * -06:09:05- DEBUG nick -> NICK SniperFodder 1 1328858574 ~UnifiedFo mail.silicateillusion.org irc.unifiedtech.org 1328858574 :Debra FromIT
                 * -06:09:05- DEBUG nick -> NICK Lone0001 1 1329005321 ford CPE00240143cbdd-CM602ad06c64f7.cpe.net.cable.rogers.com irc.unifiedtech.org 1329005321 :Ford
                 * -06:09:05- DEBUG nick -> NICK Lone 1 1329007296 lone0001 ares.cncfps.com irc.unifiedtech.org 1 :Ford
                 */
            }

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

        #endregion
    }
}
