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
    using System.Net.Sockets;

    public class DisconnectionEventArgs : System.EventArgs
    {
        #region Constructor(s)

        public DisconnectionEventArgs(SocketError errorId, Exception exception = null)
        {
            Error = errorId;
            Exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the exception that caused the disconnection event.</para>
        /// </summary>
        public Exception Exception { get; protected set; }

        /// <summary>
        ///     <para>Gets the SocketError ID for the error that triggered the disconnection</para>
        /// </summary>
        public SocketError Error { get; protected set; }

        #endregion
    }
}