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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     <para>An enumeration of values specifying the type of SSL connection we're dealing with</para>
    /// </summary>
    public enum SslType
    {

        /// <summary>
        ///     <para>Represents an OpenSSL encryption scheme.</para>
        /// </summary>
        OpenSSL,

        /// <summary>
        ///     <para>Represents a GnuTLS encryption scheme.</para>
        /// </summary>
        GnuTLS,
    }

    public class PortInfo
    {

        #region Constructor(s)

        public PortInfo(Int32 port)
            : this(port, false)
        {
        }

        public PortInfo(Int32 port, bool ssl)
            : this(port, ssl, SslType.OpenSSL)
        {
        }

        public PortInfo(Int32 port, bool ssl, SslType sslType)
        {
            m_PortNumber = port;
            m_SslEnabled = ssl;
            m_SslType = sslType;
        }

        #endregion

        #region Properties

        private Int32 m_PortNumber;
        /// <summary>
        ///     <para>Gets the port number</para>
        /// </summary>
        public Int32 Port
        {
            get { return m_PortNumber; }
        }

        private bool m_SslEnabled;
        /// <summary>
        ///     <para>Gets a value indicating whether this is an SSL Port</para>
        /// </summary>
        public bool SslEnabled
        {
            get { return m_SslEnabled; }
        }

        private SslType m_SslType;
        /// <summary>
        ///     <para>Gets a value indicating the SSL type for this port</para>
        /// </summary>
        public SslType SslType
        {
            get { return m_SslType; }
        }

        #endregion

    }
}
