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
    using Atlantis.Linq;
    using Atlantis.Net.Irc.Data;

    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;

    public partial class IrcClient
    {
        #region Methods

        protected virtual void OnDataReceive(string input)
        {
            if (WriteLog)
            {
                WriteToLog(input);
            }

            string[] toks = input.Split(' ');
            Match m, n;

            if (toks[0].EqualsIgnoreCase("ping"))
            {
                Send("PONG {0}", toks[1]);
            }

            int raw = 000;
            if (int.TryParse(toks[1], out raw))
            {
                OnRawNumeric(raw, input);
            }
            else if (toks[1].EqualsIgnoreCase("join"))
            {
                if ((m = Patterns.rUserHost.Match(toks[0])).Success && (n = Patterns.rChannelRegex.Match(toks[2])).Success)
                {
                    OnJoin(n.Groups[1].Value, m.Groups[1].Value);
                }
            }
            else if (toks[1].EqualsIgnoreCase("part"))
            {
                if ((m = Patterns.rUserHost.Match(toks[0])).Success && (n = Patterns.rChannelRegex.Match(toks[2])).Success)
                {
                    OnPart(n.Groups[1].Value, m.Groups[1].Value);
                }
            }
            else if (toks[1].EqualsIgnoreCase("quit"))
            {
                //ac QUIT :foo
                if ((m = Patterns.rUserHost.Match(toks[0])).Success)
                {
                    string s1 = input.Substring(input.IndexOf(toks[1]));
                    string message = s1.Substring(s1.IndexOf(" ") + 1);
                    if (message.StartsWith(":"))
                        message = message.Substring(1);

                    OnQuit(m.Groups[1].Value, message);
                }
            }
            else if (toks[1].EqualsIgnoreCase("mode"))
            {
#if DEBUG
                m_Logger.Debug("MODE: {0}", input);
#endif

                /*
                debug: MODE: :AtlantisTest MODE AtlantisTest :+iwx
                debug: MODE: :AtlantisTest MODE AtlantisTest :+oghaAsNWqt
                debug: MODE: :ChanServ!services@nite-serv.com MODE #Services +o AtlantisTest
                 */

                if ((m = Patterns.rUserHost.Match(toks[0])).Success && (n = Patterns.rChannelRegex.Match(toks[2])).Success)
                {
                    //Console.WriteLine("debug: {0}", input.Substring(input.IndexOf(toks[3])));
                    if (toks.Length > 4)
                    {
                        // chan-user mode
                        string s1 = input.Substring(input.IndexOf(toks[3])).Substring(toks[3].Length + 1);
                        OnRawChannelMode(n.Groups[1].Value, m.Groups[1].Value, toks[3], s1.Split(' '));
                    }
                    else
                    {
                        // generic channel mode
                        OnRawChannelMode(n.Groups[1].Value, m.Groups[1].Value, toks[3]);
                    }
                }
            }
            else if (toks[1].EqualsIgnoreCase("nick"))
            {
                if ((m = Patterns.rUserHost.Match(toks[0])).Success)
                {
                    OnNickChange(m.Groups[1].Value, (toks[2][0].Equals(':') ? toks[2].Remove(0, 1) : toks[2]));
                }
            }
            /*
                [PM/7:38:29] :Genesis2001!~ian@admin.nite-serv.com PRIVMSG #hangout :DERP
                [PM/7:38:42] :Genesis2001!~ian@admin.nite-serv.com NOTICE #hangout :derp
                [PM/7:38:43] :SSIhekill!s3xyspy@live.com PRIVMSG #hangout :you spammer
                [PM/7:38:47] :Genesis2001!~ian@admin.nite-serv.com NOTICE AtlantisTest :derp
                [PM/7:38:47] :SSIhekill!s3xyspy@live.com PRIVMSG #hangout :Gensesis
                [PM/7:38:48] :SSIhekill!s3xyspy@live.com PRIVMSG #hangout :Shhh
                [PM/7:38:53] :SSIhekill!s3xyspy@live.com PRIVMSG #hangout :People are trying to fuck.
                [PM/7:38:54] :Genesis2001!~ian@admin.nite-serv.com PRIVMSG #hangout :no, you.
                [PM/7:38:55] :BlueThen!Blue-mIRC7@BlueThen.com PRIVMSG #hangout :we're evenly matched
                [PM/7:46:22] :Genesis2001!~ian@admin.nite-serv.com PRIVMSG AtlantisTest :.
                */
            else if (toks[1].EqualsIgnoreCase("privmsg"))
            {
                OnMessageReceived(input);
            }
            else if (toks[1].EqualsIgnoreCase("notice"))
            {
                OnNoticeReceived(input);
            }
        }

        protected virtual void OnMessageReceived(string input)
        {
            string[] toks = input.Split(' ');
            Match u, c;

            string user, message;

            if ((u = Patterns.rUserHost.Match(toks[0])).Success)
            {
                Channel target;
                message = input.Substring(input.IndexOf(':', 2) + 1);
                user = u.Groups[1].Value;
                if ((c = Patterns.rChannelRegex.Match(toks[2])).Success)
                {
                    // TODO: Verify CTCP messages and fire off events for them
                    target = GetChannel(c.Groups[1].Value);
                    ChannelMessageReceivedEvent.Raise(this, new ChannelMessageReceivedEventArgs(target, message, user));
                }
                else
                {
                    // TODO: Verify CTCP messages and fire off events for them
                    PrivateMessageReceivedEvent.Raise(this, new PrivateMessageReceivedEventArgs(user, message));
                }
            }
            else
            {
                // TODO: InspIRCd's m_chanlog module support
            }

            /*if ((a = Patterns.rPrivmsg.Match(input)).Success)
            {
                user = a.Groups[1].Value;
                message = a.Groups[4].Value;
                target = a.Groups[3].Value;

                if ((b = Patterns.rChannelRegex.Match(a.Groups[3].Value)).Success)
                {
                    var c = GetChannel(target);
                    ChannelMessageReceivedEvent.Raise(this, new ChannelMessageReceivedEventArgs(c, message, user));
                }
                else
                {
                    PrivateMessageReceivedEvent.Raise(this, new PrivateMessageReceivedEventArgs(user, message));
                }
            }*/
        }

        protected virtual void OnJoin(string channel, string nick)
        {
            Channel c = GetChannel(channel);

            if (!c.Users.ContainsKey(nick))
            {
                c.Users.Add(nick, new PrefixList(this));
            }

            ChannelJoinEvent.Raise(this, new JoinPartEventArgs(channel, nick));

            if (StrictNames)
            {
                Send("NAMES {0}", channel);
            }

            if (FillListsOnJoin)
            {
                if (FillListsDelay != 0.0)
                {
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.AutoReset = false;
                    t.Interval = FillListsDelay;
                    t.Elapsed += (a, b) =>
                    {
                        Send("MODE {0} +b", c.Name);
                        Send("MODE {0} +e", c.Name);
                        Send("MODE {0} +I", c.Name);
                    };
                    t.Start();
                }
                else
                {
                    Send("MODE {0} +b", c.Name);
                    Send("MODE {0} +e", c.Name);
                    Send("MODE {0} +I", c.Name);
                }
            }
        }

        protected virtual void OnListMode(int numeric, string channel, string setBy, DateTime setOn, string mask)
        {
            Channel c = GetChannel(channel);
            //ListModeType type = ListModeType.BAN;
            char type = '\0';
            switch (numeric)
            {
                case 346:           // +I
                    {
                        type = 'I';
                    } break;
                case 348:           // +e
                    {
                        type = 'e';
                    } break;
                default:
                case 367:           // +b
                    {
                        type = 'b';
                    } break;
            }

            if (c.ListModes.Find(x => x.Mask.EqualsIgnoreCase(mask)) != null)
            {
                return;                 // Already set, why are we adding it again? o.O
            }

            ListMode l = new ListMode(setOn, mask, setBy, type);
            c.ListModes.Add(l);
        }

        protected virtual void OnNickChange(string oldNick, string newNick)
        {
            foreach (var channel in Channels)
            {
                if (channel.Users.ContainsKey(oldNick))
                {
                    channel.Users.Add(newNick, channel.Users[oldNick]);
                    channel.Users.Remove(oldNick);
                }
            }

            NickChangeEvent.Raise(this, new NickChangeEventArgs(oldNick, newNick));
        }

        protected virtual void OnNoticeReceived(string input)
        {
            string[] toks = input.Split(' ');
            Match m, n;
            string sender = "", message = "";
            if ((m = Patterns.rUserHost.Match(toks[0])).Success)
            {
                Channel target;
                message = input.Substring(input.IndexOf(':', 2) + 1);
                sender = m.Groups[1].Value;
                // priv. notice
                if ((n = Patterns.rChannelRegex.Match(toks[2])).Success)
                {
                    // channel
                    target = GetChannel(n.Groups[1].Value);
                    ChannelNoticeReceivedEvent.Raise(this, new ChannelMessageReceivedEventArgs(target, message, sender, true));
                }
                else
                {
                    // private notice
                    PrivateNoticeReceivedEvent.Raise(this, new PrivateMessageReceivedEventArgs(sender, message, true));
                }
            }
            else
            {
                // Server Notice
                //Console.WriteLine("(S)NOTICE: {0}", input.Substring(input.IndexOf(toks[2])));
            }
        }

        protected virtual void OnPart(string channel, string nick)
        {
            Channel c = GetChannel(channel);

            if (c.Users.ContainsKey(nick))
            {
                c.Users.Remove(nick);
            }

            ChannelPartEvent.Raise(this, new JoinPartEventArgs(channel, nick));

            if (StrictNames)
            {
                Send("NAMES {0}", channel);
            }
        }

        /// <summary>
        ///     <para>This event is called internally when a mode is set on a channel</para>
        /// </summary>
        /// <param name="channel">Channel on which mode was set</param>
        /// <param name="user">Source of the mode setting. Generally a client.</param>
        /// <param name="modes">List of mode changes. For example: +m-o Zack</param>
        /// <param name="targets">List of space delimited parameters for the mode string. For example: user names or limit sizes, etc.</param>
        protected virtual void OnRawChannelMode(string channel, string user, string modes, params string[] targets)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Raw channel mode received, displaying targets: ");
            foreach (string s in targets)
            {
                System.Diagnostics.Debug.WriteLine(s);
            }
#endif
            // There is no reason for these items to be null or empty. They are required by the IRC specification RFC 1459.
            if (String.IsNullOrEmpty(channel) || String.IsNullOrEmpty(user) || String.IsNullOrEmpty(modes))
            {
#if DEBUG
                throw new ArgumentNullException("OnRawChannelMode parameter null or empty. Mode settings MUST contain a channel, user (entity which sent the command), and list of mode changes)");
#else 
                return;
#endif
            }
            Channel chan = GetChannel(channel);
            if (chan == null)
            {
#if DEBUG
                throw new Exception("OnRawChannelMode channel given does not exist. Null returned by GetChannel!");
#else
                return;
#endif
            }

            bool isSet = false;

            string[] channelModesByTypes = m_ChannelModes.Split(',');

            for (int i = 0, j = 0; i < modes.Length; ++i)
            {

                char c = modes[i];
                if (c == '+')
                    isSet = true;
                else if (c == '-')
                    isSet = false;
                else if (channelModesByTypes[3].Contains(c))
                {
                    HandleChannelMode(chan, c, null, ChanmodeType.NOPARAM, isSet);
                }
                else if (channelModesByTypes[0].Contains(c))
                {
                    if (j >= targets.Length)
                    {
                        continue;
                    }
                    HandleChannelMode(chan, c, targets[j], ChanmodeType.LIST, isSet);
                    j++; // Increment current parameter
                }
                else if (channelModesByTypes[1].Contains(c))
                {
                    if (j >= targets.Length)
                    {
                        continue;
                    }
                    HandleChannelMode(chan, c, targets[j], ChanmodeType.SETUNSET, isSet);
                    j++; // Increment current parameter
                }
                else if (channelModesByTypes[2].Contains(c))
                {

                    if (isSet && j >= targets.Length)
                    {
                        continue;
                    }

                    HandleChannelMode(chan, c, isSet ? targets[j] : null, ChanmodeType.SET, isSet);
                    if (isSet) // Since parameters for modes in this group are only required when set
                        j++;   // Increment current parameter if this mode is being set
                }
                else if (m_AccessModes.Contains(c))
                {
                    if (j >= targets.Length)
                    {
                        continue;
                    }
                    HandleChannelMode(chan, c, targets[j], ChanmodeType.ACCESS, isSet);
                    j++; // Increment current parameter
                }

            }
        }

        /// <summary>
        ///     <para>This event is called internally when a use sends QUIT</para>
        /// </summary>
        /// <param name="nick">Nickname of user who quit</param>
        /// <param name="message">Message associated with quit ('reason')</param>
        protected virtual void OnQuit(string nick, string message)
        {
            foreach (Channel c in Channels)
            {
                c.Users.Remove(nick);
            }

            QuitEvent.Raise(this, new QuitEventArgs(nick, message));
        }

        protected virtual void OnRawNumeric(int numeric, string line)
        {
            // TODO: Trigger a RawNumericReceivedEvent event
            //Console.WriteLine("{0} {1}", numeric.ToString("[000]"), line);
            string[] toks = line.Split(' ');

            Match m;
            switch (numeric)
            {
                case 001:
                    {
                        ConnectionEstablishedEvent.Raise(this, EventArgs.Empty);
                    } break;
                case 005:
                    {
                        if ((m = Patterns.rUserPrefix.Match(line)).Success)
                        {
                            m_AccessModes = m.Groups[1].Value;
                            m_AccessPrefixes = m.Groups[2].Value;
                            rRawNames = new Regex(String.Format(@"([{0}]?)(\S+)", m_AccessPrefixes));
#if DEBUG
                            m_Logger.Debug("Regex Pattern: {0}", rRawNames.ToString());
                            m_Logger.Debug("Access Modes: {0} | Access Prefixes: {1}", m_AccessModes, m_AccessPrefixes);
#endif
                        }

                        if ((m = Patterns.rChannelModes.Match(line)).Success)
                        {
                            m_ChannelModes = m.Groups[1].Value;
                        }

                    } break;
                case 353:
                    {
                        Channel c = GetChannel(toks[4]);                                                // Get the channel from the collection of channels

                        string sub = line.Substring(line.IndexOf(":", 1)).Remove(0, 1);                 // Get the list of nicks out of the line.
                        string[] names = sub.Split(' ');                                                // Split the list into an array.

                        foreach (var name in names)
                        {
                            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(name))
                            {
                                continue;
                            }

                            if ((m = rRawNames.Match(name)).Success)
                            {                                                                           // Parse the nicks and match them via RegEx
                                var nick = m.Groups[2].Value;                                           // capture the nick
                                var prefix = (String.IsNullOrEmpty(m.Groups[1].Value) ? '\0' : m.Groups[1].Value[0]);       // and the prefix

                                if (c.Users.ContainsKey(nick))
                                {
                                    c.Users[nick].AddPrefix(prefix);                                             // Update the nick's prefix, or
                                }
                                else
                                {
                                    PrefixList l = new PrefixList(this);
                                    l.AddPrefix(prefix);
                                    c.Users.Add(nick, l);                                          // add it to the list if they don't exist.
                                }
#if DEBUG
                                Send("PRIVMSG {0} :Name: {1} - Access: {2}", c.Name, nick, prefix);     // Debug output for reference.
#endif
                            }

                        }
                    } break;

                case 367:               // +b
                case 348:               // +e
                case 346:               // +I
                    {
                        string setBy = toks[5];
                        DateTime setOn = (Double.Parse(toks[6]).ToDateTime());
                        string mask = toks[4];
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("debug: [ListMode:{0}] Set By: {1} | Mask: {2} | Channel: {3}", numeric, toks[5], toks[4], toks[3]);
                        System.Diagnostics.Debug.WriteLine("\t\t Set On: {0}", setOn);
#endif

                        OnListMode(numeric, toks[3], toks[5], setOn, toks[4]);
                    } break;

                case 332:
                    {
                        // TODO: channel topic
                    } break;

            }
        }

        protected virtual void ParameterizedThreadCallback(Object o)
        {
            WriteDebug("Entering ParameterizedThreadCallback...");
            if (o != null)
            {
                WriteDebug("Registring wait handle set event...");
                this.ConnectionEstablishedEvent += (s, e) =>
                {
                    ((EventWaitHandle)o).Set();
                };
            }

            stream = m_Socket.GetStream();

            queueThread.Start();

            Register();

            // TODO: Allow for sending cerificates to the server.
            // TODO: Research how to even do the above comment.
            if (Port.SslEnabled)
            {
                reader = new StreamReader(new System.Net.Security.SslStream(stream, true), Encoding);
            }
            else
            {
                reader = new StreamReader(stream, Encoding);
            }

            string line = null;
            while (m_Socket.Connected)
            {
                if (stream.DataAvailable)
                {
                    while (!reader.EndOfStream)
                    {
                        // Read from the buffer
                        line = reader.ReadLine();
                        // Trim the buffer of excess white space
                        line = line.Trim();

                        // check whether the line is empty/null after we trim.
                        if (!String.IsNullOrEmpty(line))
                        {
                            // fire off the parser
                            OnDataReceive(line);
                        }
                    }

                }
            }
        }

        #endregion
    }
}