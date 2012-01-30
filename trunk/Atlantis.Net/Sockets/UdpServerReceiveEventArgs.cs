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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    public class UdpServerReceiveEventArgs : EventArgs
    {
        #region Constructor(s)

        public UdpServerReceiveEventArgs(byte[] data, IPEndPoint endPoint)
        {
            Data = data;
            RemoteEndPoint = endPoint;
        }

        public UdpServerReceiveEventArgs(byte[] data, IPEndPoint endPoint, object state)
            : this(data, endPoint)
        {
            State = state;
        }

        #endregion

        #region Properties

        private byte[] m_Data = null;
        /// <summary>
        ///     Gets the byte data that was received from the TcpServer
        /// </summary>
        public byte[] Data
        {
            get { return m_Data; }
            private set { m_Data = value; }
        }

        private IPEndPoint m_RemoteEndPoint = null;
        /// <summary>
        ///     Gets the Remote Endpoint that sent the data
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return m_RemoteEndPoint; }
            private set { m_RemoteEndPoint = value; }
        }

        private object m_State = null;
        /// <summary>
        ///     Gets an artubary, generic object
        /// </summary>
        public object State
        {
            get { return m_State; }
            private set { m_State = value; }
        }

        #endregion
    }
}
