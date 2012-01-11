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

namespace Atlantis.Net.Sockets.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;

    public static class SocketExtensions
    {

        /// <summary>
        ///     Determines whether a socket is connected by probing the socket for available data
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Boolean IsConnected(this Socket x)
        {
            try
            {
                return !(x.Poll(1, SelectMode.SelectRead) && x.Available == 0);
            }
            catch (SocketException) { return false; }
        }

    }
}