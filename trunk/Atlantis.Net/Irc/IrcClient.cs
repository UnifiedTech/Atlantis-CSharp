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
    using Atlantis.Net.Irc.Linq;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class IrcClient
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Constructs a default instance of the IrcClient.</para>
        /// </summary>
        public IrcClient()
        {
            socket = new TcpClient();
            thread = new Thread(new ParameterizedThreadStart(ParameterizedThreadCallback));
            m_SendQueue = new Queue<string>();
            queueThread = new Thread(new ThreadStart(QueueThreadCallback));
            thread.IsBackground = true;

            StrictModes = false;
            StrictNames = false;
            m_FillLists = true;
            m_FillListsDelay = 0.0;
        }

        /// <summary>
        ///     <para>Constructs a new instance of the IrcClient using the specified properties</para>
        /// </summary>
        /// <param name="server"></param>
        public IrcClient(String server)
            : this()
        {
            //
            throw new NotImplementedException("Constructor is not finished.");
        }

        #endregion

        #region Constants

        public const char CONTROL_BOLD = (char)2;
        public const char CONTROL_COLOR = (char)3;
        public const char CONTROL_UNDERLINE = (char)31;
        public const char CONTROL_REVERSE = (char)22;
        public const char CONTROL_ITALICS = (char)29;

        public const Int32 DefaultQueueDelay = 750;

        #endregion

        #region Fields

        protected string m_AccessModes;
        protected string m_AccessPrefixes;
        protected string m_ChannelModes;
        protected IPEndPoint connection;
        protected readonly string m_LogFile = String.Format("ircd-{0}.log", DateTime.Now.ToString("MM-d-yyyy"));
        protected StreamReader reader;
        protected TcpClient socket;
        protected NetworkStream stream;
        protected Thread thread;
        protected Thread queueThread;

        protected Regex rRawNames;
        protected Queue<String> m_SendQueue;

        #endregion

        #region Properties

        protected List<Channel> m_Channels;
        /// <summary>
        ///     <para>Gets a list of channels</para>
        /// </summary>
        public List<Channel> Channels
        {
            get
            {
                if (m_Channels == null)
                {
                    m_Channels = new List<Channel>();
                }

                return m_Channels;
            }
        }

        protected Encoding m_Encoding = Encoding.ASCII;
        /// <summary>
        ///     Gets or sets the encoding for the specified IrcClient to send and receive data.
        /// </summary>
        public Encoding Encoding
        {
            get { return m_Encoding; }
            set
            {
                if (!socket.Connected)
                {
                    m_Encoding = value;
                }
            }
        }

        protected double m_FillListsDelay;
        /// <summary>
        ///     <para>Gets or sets the delay for setting list modes on join</para>
        ///     <para>Optional. Only works if FillListsOnJoin is enabled.</para>
        /// </summary>
        public double FillListsDelay
        {
            get { return m_FillListsDelay; }
            set { m_FillListsDelay = value; }
        }

        protected bool m_FillLists;
        /// <summary>
        ///     <para>Gets or sets whether to fill list modes for a channel on join</para>
        ///     <para>Note: If disabled, list modes will only get populated on channels if you manually send a +b, +e, and/or +I or whenever one is set</para>
        /// </summary>
        public bool FillListsOnJoin
        {
            get { return m_FillLists; }
            set { m_FillLists = value; }
        }

        /// <summary>
        ///     <para>Gets a value indicating whether the client is connected to the server</para>
        /// </summary>
        /// <devdoc>
        ///     <para>TODO: Possibly implement ping-pong system from IRCd to determine connectivitiy</para>
        /// </devdoc>
        public bool Connected
        {
            get { return socket.Connected; }
        }

        protected string m_Host;
        /// <summary>
        ///     Gets or sets the host that the IrcClient will be connecting to
        /// </summary>
        public virtual string Host
        {
            get { return m_Host; }
            set
            {
                if (socket.Connected)
                {
                    Send("QUIT");
                    Thread.Sleep(5);
                    connection = new IPEndPoint(Dns.GetHostEntry(value).AddressList[0], Port.Port);
                    socket.Connect(connection);
                }

                m_Host = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the ident for the current IrcClient</para>
        ///     <para>This property is optional and can be omitted when setting up the IrcClient</para>
        /// </summary>
        public string Ident { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value indicating that the thread is running in the foreground or background</para>
        /// </summary>
        public bool IsBackgroundThread
        {
            get { return thread.IsBackground; }
            set
            {
                if ((thread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted)
                {
                    thread.IsBackground = value;
                }
                else
                {
                    throw new ThreadStateException("Cannot change background status after the thread has already been started.");
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the IrcClient has been initilaized properly.
        /// </summary>
        public virtual bool IsInitialized
        {
            get
            {
                var ret = true;

                if (String.IsNullOrEmpty(Host)) ret = false;
                else if (String.IsNullOrEmpty(Nick)) ret = false;

                return ret;
            }
        }

        /// <summary>
        ///     <para>Sets the password to be used when connecting to the current IrcClient</para>
        ///     <para>This property is optional and can be omitted when setting up the IrcClient</para>
        /// </summary>
        public string Password { protected get; set; }

        protected PortInfo m_Port;
        /// <summary>
        ///     <para>Gets or sets a value telling the IrcClient to connect on a specific port</para>
        ///     <para>This property is optional and can be omitted when setting up the IrcClient</para>
        /// </summary>
        public PortInfo Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        private Int32 m_QueueDelay = DefaultQueueDelay;
        /// <summary>
        ///     <para>Gets or sets a value indicating what delay value to use for sending data to the IRC Server.</para>
        /// </summary>
        public Int32 QueueDelay
        {
            get { return m_QueueDelay; }
            set { m_QueueDelay = value; }
        }

        protected string m_Nick;
        /// <summary>
        ///     <para>Gets or sets the nick for the current IrcClient.</para>
        ///     <para>If connected, the IrcClient will send a NICK command to the IRC Server changing the client's nick.</para>
        /// </summary>
        public string Nick
        {
            get { return m_Nick; }
            set
            {
                // TODO: Validate whether the nick is of a valid Alphanumeric value with no numerics at the beginning.

                if (!Patterns.rAlphaNumericRegex.IsMatch(value))
                {
                    throw new Exception("Erroneous Nickname. Nick cannot start with a numerical value.");
                }

                if (socket.Connected)
                {
                    Send("NICK {0}", value);
                }

                m_Nick = value;
            }
        }

        protected List<char> m_Usermodes;
        /// <summary>
        ///     <para>Gets a list of usermodes that have been set on the current client</para>
        /// </summary>
        public List<char> Usermodes
        {
            get
            {
                if (m_Usermodes == null)
                {
                    m_Usermodes = new List<char>();
                }

                return m_Usermodes;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the real name of the current IrcClient</para>
        ///     <para>This property is optional and can be omitted when setting up the IrcClient</para>
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value determing whether to rely on numerics or parsing modes for filling various lists</para>
        ///     <para>Default: false</para>
        /// </summary>
        public bool StrictModes { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value determining whether to always request NAMES upon any action that involves users leaving, quitting, being kicked, etc.</para>
        ///     <para>Default is: false</para>
        /// </summary>
        public bool StrictNames { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to write debug information to an ircd.log file.
        /// </summary>
        public bool WriteLog { get; set; }

        /// <summary>
        ///     Gets the list of channel access prefixes sent in 001 (PREFIX=)
        /// </summary>
        public string AccessPrefixes { get { return m_AccessPrefixes; } }

        /// <summary>
        /// Gets the list of channel access modes sent in 001 (PREFIX=)
        /// </summary>
        public string AccessModes { get { return m_AccessModes; } }

        #endregion

        #region Events

        public event EventHandler                                   ConnectionEstablishedEvent;
        public event EventHandler<ChannelMessageReceivedEventArgs>  ChannelMessageReceivedEvent;
        public event EventHandler<ChannelMessageReceivedEventArgs>  ChannelNoticeReceivedEvent;
        public event EventHandler<JoinPartEventArgs>                ChannelJoinEvent;
        public event EventHandler<JoinPartEventArgs>                ChannelPartEvent;
        public event EventHandler<NickChangeEventArgs>              NickChangeEvent;
        public event EventHandler<PrivateMessageReceivedEventArgs>  PrivateMessageReceivedEvent;
        public event EventHandler<PrivateMessageReceivedEventArgs>  PrivateNoticeReceivedEvent;
        public event EventHandler<QuitEventArgs>                    QuitEvent;

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Disconnects the present client from the server.</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Disconnect(string message = "")
        {
            if (socket != null && socket.Connected)
            {
                if (String.IsNullOrEmpty(message))
                {
                    Send("QUIT");
                }
                else
                {
                    Send("QUIT :{0}", message);
                }

                thread.Abort();
                socket.Close();
                return true;
            }
            return false;
        }


        public Channel GetChannel(string name)
        {
            Channel ret = Channels.OfType<Channel>().SingleOrDefault(c => c.Name.EqualsIgnoreCase(name));

            if (ret == null)
            {
                ret = new Channel(name, this);
                Channels.Add(ret);
            }

            return ret;
        }

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

            Int32 raw = 000;
            if (Int32.TryParse(toks[1], out raw))
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
                    string message = s1.Substring(s1.IndexOf(" ")+1);
                    if (message.StartsWith(":"))
                        message = message.Substring(1);

                    OnQuit(m.Groups[1].Value, message);
                }
            }
            else if (toks[1].EqualsIgnoreCase("mode"))
            {
#if DEBUG2
                Console.WriteLine("debug: MODE: {0}", input);
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
                        string s1 = input.Substring(input.IndexOf(toks[3])).Substring(toks[3].Length +1);
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

        /*
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
                Console.WriteLine("(S)NOTICE: {0}", input.Substring(input.IndexOf(toks[2])));
            }
        }
         */

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
        /// This event is called internally when a mode is set on a channel
        /// </summary>
        /// <param name="channel">Channel on which mode was set</param>
        /// <param name="user">Source of the mode setting. Generally a client.</param>
        /// <param name="modes">List of mode changes. For example: +m-o Zack</param>
        /// <param name="targets">List of space delimited parameters for the mode string. For example: user names or limit sizes, etc.</param>
        protected virtual void OnRawChannelMode(string channel, string user, string modes, params string[] targets)
        {
#if DEBUG2
            Console.WriteLine("Raw channel mode received, displaying targets: ");
            foreach (string s in targets)
                Console.WriteLine(s);
#endif
            // There is no reason for these items to be null or empty. They are required by the IRC specification RFC 1459.
            if (String.IsNullOrEmpty(channel) || String.IsNullOrEmpty(user) || String.IsNullOrEmpty(modes))
            {
#if DEBUG2
                throw new ArgumentNullException("OnRawChannelMode parameter null or empty. Mode settings MUST contain a channel, user (entity which sent the command), and list of mode changes)");
#else 
                return;
#endif
            }
            Channel chan = GetChannel(channel);
            if (chan == null)
            {
#if DEBUG2
                        throw new Exception("OnRawChannelMode channel given does not exist. Null returned by GetChannel!");
#endif
                return;
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
#if DEBUG2
                        throw new Exception("Not enough MODE parameters for this mode string.");
#endif
                        continue;
                    }
                    HandleChannelMode(chan, c, targets[j], ChanmodeType.LIST, isSet);
                    j++; // Increment current parameter
                }
                else if (channelModesByTypes[1].Contains(c))
                {
                    if (j >= targets.Length)
                    {
#if DEBUG2
                        throw new Exception("Not enough MODE parameters for this mode string.");
#endif
                        continue;
                    }
                    HandleChannelMode(chan, c, targets[j], ChanmodeType.SETUNSET, isSet);
                    j++; // Increment current parameter
                }
                else if (channelModesByTypes[2].Contains(c))
                {

                    if (isSet && j >= targets.Length)
                    {

#if DEBUG2
                        throw new Exception("Not enough MODE parameters for this mode string.");
#endif
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
#if DEBUG2
                        throw new Exception("Not enough MODE parameters for this mode string.");
#endif
                        continue;
                    }
                    HandleChannelMode(chan, c, targets[j], ChanmodeType.ACCESS, isSet);
                    j++; // Increment current parameter
                }

            }


        }

        /// <summary>
        /// This event is called internally when a use sends QUIT
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
        /// <summary>
        /// Internally handles one specific mode change on a channel
        /// </summary>
        /// <param name="chan">The Channel on which the change happened.</param>
        /// <param name="mode">The mode the change happened with.</param>
        /// <param name="parameter">The parameter for this mode (or null)</param>
        /// <param name="type">The type of mode this is.</param>
        /// <param name="isSet">If this mode was set. (False if it was unset)</param>
        protected virtual void HandleChannelMode(Channel chan, char mode, string parameter, ChanmodeType type, bool isSet)
        {
#if DEBUG2
            Console.WriteLine("Mode '{0}' is being {1} with parameter: {2} (Type: {3})",mode, isSet ? "set" : "unset", parameter, type );
#endif

            if (type == ChanmodeType.ACCESS)
            {
                PrefixList list = null;
                foreach (var kvp in chan.Users)
                {
                    if (kvp.Key.EqualsIgnoreCase(parameter))
                    {
                        list = kvp.Value;
                    }
                }
                if (list == null)
                {
#if DEBUG2
//                    throw new Exception("HandleChannelMode Access mode was set on a user who was not in the channel's list (chan.Users).");
#endif
                    list = new PrefixList(this);
                    chan.Users.Add(parameter, list);
                }

                if (isSet)
                {
                    list.AddPrefix(list.ModeToPrefix(mode));
                }
                else
                {
                    list.RemovePrefix(list.ModeToPrefix(mode));
                }

                if (StrictNames)
                {
                    Send("NAMES {0}", chan.Name);
                }

            }
            else if (type != ChanmodeType.LIST)
            {
                if (isSet)
                {
                    chan.Modes.Remove(mode);  // If it is already there, it needs to be updated most likely. Or the IRCD is broken and this is irrelevent.
                    chan.Modes.Add(mode, parameter);
                }
                else
                {
                    chan.Modes.Remove(mode);
                }
            }
            else // if type is LIST
            {
                List<ListMode> list = chan.ListModes;

                ListMode lMode;

                if ((lMode = list.Find(x => x.Mask.EqualsIgnoreCase(parameter))) == null)
                {
                    if (!isSet)
                    {
                        return;
                    }

                    lMode = new ListMode(DateTime.Now, parameter, "", mode);
                    list.Add(lMode);
                }

                if (StrictModes)
                {
                    Send("MODE {0} +{1}", chan.Name, mode);
                }
 
                /*List<string> list;
                if (!chan.ListModes.TryGetValue(mode, out list))
                {
                    if (!isSet) // If we are unsetting a value but had no list...then clearly we had no value already stored :)
                        return;
                    list = new List<string>();
                    chan.ListModes.Add(mode, list);
                }
                // If we are here, we should have the list of this mode
                list.RemoveAll(x => x.EqualsIgnoreCase(parameter));
                if (isSet)
                {
                    list.Add(parameter);
                }*/

            }


        }


        /// <summary>
        /// List of types a channel mode has
        /// </summary>
        protected enum ChanmodeType
        {
            /// <summary>
            /// Channel mode has many seperate parameters which can be listed by requesting "+m" where m is the mode and no parameters are given.
            /// </summary>
            LIST,
            /// <summary>
            /// Channel mode requires parameters to be set and to be unset.
            /// </summary>
            SETUNSET,
            /// <summary>
            /// Channel mode requires parameters to be set, but must have none to be unset.
            /// </summary>
            SET,
            /// <summary>
            /// Channel mode should never have a parameter associated with it.
            /// </summary>
            NOPARAM,
            /// <summary>
            /// Mode grants a user access on a channel. The associated prefix should be stored with the user, and not the channel.
            /// </summary>
            ACCESS
        };



        protected virtual void OnRawNumeric(Int32 numeric, string line)
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
#if DEBUG2
                            Console.WriteLine("debug: Regex Pattern: {0}", rRawNames.ToString());
                            Console.WriteLine("debug: Access Modes: {0} | Access Prefixes: {1}", accessModes, accessPrefixes);
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
#if DEBUG2
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
#if DEBUG2
                        Console.WriteLine("debug: [ListMode:{0}] Set By: {1} | Mask: {2} | Channel: {3}", numeric, toks[5], toks[4], toks[3]);
                        Console.WriteLine("\t\t Set On: {0}", setOn);
#endif

                        OnListMode(numeric, toks[3], toks[5], setOn, toks[4]);
                    } break;

                case 332:
                    {
                        // TODO: channel topic
                    } break;

            }
        }

        protected void QueueThreadCallback()
        {
            while (socket != null && socket.Connected)
            {
                lock (m_SendQueue)
                {
                    if (m_SendQueue.Count > 0)
                    {
                        Send(m_SendQueue.Dequeue());
                    }
                }

                Thread.Sleep(QueueDelay);
            }
        }

        protected virtual void Register()
        {
            if (!String.IsNullOrEmpty(Password))
            {
                Send("PASS :{0}", Password);
            }

            Send("NICK {0}", Nick);
            Send("USER {0} 0 * :{1}", Ident, Realname);
        }

        private void Send(string toSend)
        {
            if (socket != null && socket.Connected)
            {
                byte[] data = Encoding.GetBytes(toSend);
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
        }

        /// <summary>
        ///     <para>Sends the specified formatted System.String to the IrcClient</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Send(string format, params object[] args)
        {
            if (socket != null && socket.Connected)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(format, args);
                sb.Append("\n");

                lock (m_SendQueue)
                {
                    m_SendQueue.Enqueue(sb.ToString());
                }

                return true;
            }

            return false;
        }

        private void SetDefaultValues()
        {
            if (String.IsNullOrEmpty(Ident)) Ident = Nick;
            if (String.IsNullOrEmpty(Realname)) Realname = Nick;

            if (m_Port == null)
            {
                m_Port = new PortInfo(6667);
            }
        }

        /// <summary>
        /// Starts the IRC Client using the properties specified, blocks until connected
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            if (!IsInitialized)
            {
                throw new NotImplementedException("The specified IrcClient has not yet been initialized yet.");
            }

            // connect the socket.
            SetDefaultValues();

            try
            {
                connection = new IPEndPoint(Dns.GetHostEntry(Host).AddressList[0], Port.Port);
                socket.Connect(connection);
            }
            catch (SocketException)
            {
                // TODO: fire off disconnection event
            }

            thread = new Thread(new ParameterizedThreadStart(ParameterizedThreadCallback));
            thread.IsBackground = IsBackgroundThread;

            EventWaitHandle wait = new EventWaitHandle(false, EventResetMode.ManualReset);
            thread.Start(wait);
            wait.WaitOne(30000);

            return socket.Connected;
        }

        /// <summary>
        ///     Starts the Irc Client using the properties specified, does not block.
        /// </summary>
        /// <returns></returns>
        public virtual bool StartAsync()
        {
            if (!IsInitialized)
            {
                throw new NotImplementedException("The specified IrcClient has not yet been initialized yet.");
            }

            // connect the socket.
            SetDefaultValues();

            try
            {
                connection = new IPEndPoint(Dns.GetHostEntry(Host).AddressList[0], Port.Port);
                socket.Connect(connection);
            }
            catch (SocketException)
            {
                // TODO: fire off disconnection event
            }

            thread.Start(null);

            return socket.Connected;
        }

        /// <summary>
        ///     Disconnects the IrcClient from the server and closes the connection and all related resources are released.
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            if (socket != null && socket.Connected)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("QUIT :Disconnecting");
                byte[] data = Encoding.GetBytes(sb.ToString());

                stream.Write(data, 0, data.Length);
                stream.Flush();

                stream.Close();
                socket.Close();

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Disconnects the IrcClient from the server and closes the connection and all related resources are released.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Stop(string format, params object[] args)
        {
            if (socket != null && socket.Connected)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("QUIT :");
                sb.AppendFormat(format, args);
                sb.Append("\n");
                byte[] data = Encoding.GetBytes(sb.ToString());

                stream.Write(data, 0, data.Length);
                stream.Flush();

                stream.Close();
                socket.Close();

                return true;
            }

            return false;
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

            stream = socket.GetStream();

            queueThread.Start();

            Register();

            // TODO: Allow for sending cerificates to the server.
            if (Port.SslEnabled)
            {
                reader = new StreamReader(new System.Net.Security.SslStream(stream, true), Encoding);
            }
            else
            {
                reader = new StreamReader(stream, Encoding);
            }

            string line = null;
            while (socket.Connected)
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

        private void WriteDebug(string format, params object[] args)
        {
#if DEBUG
            string temp = String.Format(format, args);
            System.Diagnostics.Debug.Write(String.Format("{0} {1}", DateTime.Now.ToString("[tt/h:mm]"), temp));
#endif
        }

        private void WriteToLog(string line)
        {
            string log = String.Format(@"{0}\{1}", "debug", m_LogFile);

            if (!Directory.Exists("debug"))
            {
                Directory.CreateDirectory("debug");
            }

            if (!File.Exists(log))
            {
                File.Create(log).Close();
            }

            File.AppendAllText(log, String.Format("{0} {1}\n", DateTime.Now.ToString("[tt/h:mm:ss]"), line));
        }

        #endregion
    }
}
