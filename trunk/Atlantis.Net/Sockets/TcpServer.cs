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

namespace Atlantis.Net.Sockets
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class TcpServer
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Constructs an instance of a TcpServer</para>
        /// </summary>
        public TcpServer()
            : this(new IPEndPoint(IPAddress.Any, 0))
        {
        }

        public TcpServer(IPEndPoint ep)
        {
            listener = new TcpListener(ep);
        }

        public TcpServer(IPAddress host, int port)
            : this(new IPEndPoint(host, port))
        {
        }

        #endregion

        #region Fields

        protected TcpListener listener;

        #endregion

        #region Properties

        private int m_BackLog;
        /// <summary>
        ///     <para>Gets or sets the maximum number of connections the server is able to hold pending at a time</para>
        /// </summary>
        public int BackLog
        {
            get { return m_BackLog; }
            set { m_BackLog = value; }
        }

        #endregion

        #region Methods

        public void Start()
        {
        }

        public void Stop()
        {
        }

        #endregion
    }
}
