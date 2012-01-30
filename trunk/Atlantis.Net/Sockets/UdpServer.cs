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

        public UdpServer(Int32 port)
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
        ///     <para>Gets or sets the network interface and/or port to be used when establishing the server.</para>
        /// </summary>
        public IPEndPoint BindingEndpoint { get; set; }

        /// <summary>
        ///     <para>Gets a value indicating whether the server has been initialized yet.</para>
        /// </summary>
        public Boolean IsInitialized { get; private set; }

        /// <summary>
        ///     <para>Gets or sets the port to be used when listening</para>
        /// </summary>
        public Int32 Port { get; set; }

        #endregion

        #region Methods

        private void DataReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)((UdpState)ar.AsyncState).Client;
            IPEndPoint e = (IPEndPoint)((UdpState)ar.AsyncState).EndPoint;

            byte[] data = u.EndReceive(ar, ref e);
            DataReceive.Raise(this, new UdpServerReceiveEventArgs(data, e, ((UdpState)ar.AsyncState)));

            u.BeginReceive(new AsyncCallback(DataReceiveCallback), new UdpState(u, new IPEndPoint(IPAddress.Any, Port)));
        }

        /// <summary>
        ///     <para>Initializes the UdpServer listening for incoming packets asynchronously</para>
        /// </summary>
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

            UdpState s = new UdpState(m_Server, new IPEndPoint(IPAddress.Any, Port));

            m_Server.BeginReceive(new AsyncCallback(DataReceiveCallback), s);
            IsInitialized = true;
        }

        #endregion
    }
}
