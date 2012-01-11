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
    using Atlantis.Linq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    public class UdpServer
    {

        #region Constructor(s)

        public UdpServer()
            : this(8915)
        {
        }

        public UdpServer(IPEndPoint endPoint)
        {
            BindingEndpoint = endPoint;
        }

        public UdpServer(int port)
        {
            Port = port;
        }

        #endregion

        #region Fields

        private UdpClient m_Server;

        #endregion

        #region Events

        public event EventHandler<UdpServerReceiveEventArgs> DataReceive;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the network interface and/or port to be used when establishing the server.
        /// </summary>
        public IPEndPoint BindingEndpoint { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the server has been initialized yet.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Gets or sets the port to be used when listening
        /// </summary>
        public int Port { get; set; }

        #endregion

        #region Methods

        private void DataReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)((UdpState)ar.AsyncState).host;
            IPEndPoint e = (IPEndPoint)((UdpState)ar.AsyncState).endPoint;

            byte[] data = u.EndReceive(ar, ref e);
            DataReceive.Raise(this, new UdpServerReceiveEventArgs(data, e, ((UdpState)ar.AsyncState)));

            u.BeginReceive(new AsyncCallback(DataReceiveCallback), new UdpState(u, new IPEndPoint(IPAddress.Any, Port)));
        }

        public void Start()
        {
            if (IsInitialized)
            {
                return;
            }

            if (BindingEndpoint == null)
            {
                m_Server = new UdpClient(Port);
            }
            else
            {
                m_Server = new UdpClient(BindingEndpoint);
            }

            UdpState s = new UdpState();
            s.host = m_Server;
            s.endPoint = new IPEndPoint(IPAddress.Any, Port);

            m_Server.BeginReceive(new AsyncCallback(DataReceiveCallback), s);
            IsInitialized = true;
        }

        #endregion

    }
}