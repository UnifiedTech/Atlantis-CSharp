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

namespace Atlantis.TestApp.IrcClient
{
    using Atlantis.Linq;
    using Atlantis.Net.Irc;

    using System;

    public static class IrcTest
    {
        #region Methods

        public static void Main(string[] args)
        {
            Console.Title = "Atlantis.Net.Irc.IrcClient TestApp";

            Framework.Initialize();

            Console.WriteLine("[*] Setting IrcClient up.");
            IrcClient c = new IrcClient();
            c.Host = "irc.unifiedtech.org";
            c.Port = new PortInfo(6667, false);

            c.Nick = "irctest";

            c.IsBackgroundThread = false;
            c.WriteLog = true;

            c.ConnectionEstablishedEvent += (s, e) =>
            {
                Console.WriteLine("[*] Connected to IRC - Joining #unifiedtech");
                ((IrcClient)s).Send("JOIN #unifiedtech");
            };
            c.ChannelMessageReceivedEvent += (s, e) =>
            {
                Framework.Console.WriteLine("Received message. {0}", e.Message);

                var client = ((IrcClient)s);

                string[] toks = e.Message.Split(' ');

                if (toks[0].EqualsIgnoreCase("!test"))
                {
                    Console.WriteLine("[*] Test command initiated - Starting 100 lines of text using Queue.");

                    for (int i = 0; i < 25; ++i)
                    {
                        var chan = client.GetChannel("#unifiedtech");
                        chan.Message("Test #{0} - Timestamp: {1} - User Count: {2}", i, DateTime.Now.ToShortTimeString(), chan.Users.Count);
                    }
                }
            };

            c.Start();

            Console.ReadLine();
        }

        #endregion
    }
}