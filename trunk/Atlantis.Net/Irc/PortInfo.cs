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

    public class PortInfo
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Initializes a PortInfo construct with default parameters</para>
        ///     <para>For SSL, use other constructor.</para>
        /// </summary>
        /// <param name="port">Required. Specifies what port this PortInfo construct represents.</param>
        public PortInfo(Int32 port)
            : this(port, false)
        {
        }

        /// <summary>
        ///     <para>Initializes a PortInfo construct specifying this port is SSL</para>
        /// </summary>
        /// <param name="port">Required. Specifies the port this PortInfo construct reprsents.</param>
        /// <param name="ssl">Optional. Specifies that this port is a secure port.</param>
        public PortInfo(Int32 port, Boolean ssl = false)
        {
            m_Port = port;
            m_SslEnabled = ssl;
        }

        #endregion

        #region Properties

        private Int32 m_Port;
        /// <summary>
        ///     <para>Gets the port number</para>
        /// </summary>
        public Int32 Port
        {
            get { return m_Port; }
        }

        private bool m_SslEnabled;
        /// <summary>
        ///     <para>Gets a value indicating whether this is an SSL Port</para>
        /// </summary>
        public bool SslEnabled
        {
            get { return m_SslEnabled; }
        }

        #endregion
    }
}
