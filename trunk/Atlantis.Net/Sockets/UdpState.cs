﻿/*
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
    using System.Net.Sockets;
    using System.Text;

    public class UdpState
    {
        #region Constructor(s)

        public UdpState()
        {
        }

        public UdpState(UdpClient client, IPEndPoint ep)
        {
            Client = client;
            EndPoint = ep;
        }

        public UdpState(UdpClient Host, IPEndPoint ep, Encoding enc)
            : this(Host, ep)
        {
            Encoding = enc;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets an instance of the underlying UdpClient</para>
        /// </summary>
        public UdpClient Client { get; private set; }

        /// <summary>
        ///     <para>Gets the IPEndPoint </para>
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        private Encoding m_Encoding = Encoding.ASCII;
        /// <summary>
        ///     <para></para>
        /// </summary>
        public Encoding Encoding
        {
            get { return m_Encoding; }
            set { m_Encoding = value; }
        }

        #endregion
    }
}
