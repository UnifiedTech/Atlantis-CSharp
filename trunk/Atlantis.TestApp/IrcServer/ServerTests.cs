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

namespace Atlantis.TestApp.IrcServer
{
    using Atlantis.Net.Irc;
    using Atlantis.Net.Irc.Data;
    // using Atlantis.Net.Irc.Linq;

    using System;

    public class ServerTests
    {
        #region Methods

        public static void Main(string[] args)
        {
            Console.Title = "IrcServer Tests";
            Framework.Initialize();

            IrcServer server = new IrcServer();
            server.Host = "ares.cncfps.com";
            server.ServerName = "atlantis.unifiedtech.org";
            server.Port = new PortInfo(8067);
            server.Password = "Bjbr673j8D";

            server.IsBackgroundThread = true;
            server.StartAsync();

            server.Send("LINKS");

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            server.Stop();
        }

        #endregion
    }
}