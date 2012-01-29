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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    public partial class IrcClient
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Constructs a default instance of the IrcClient.</para>
        /// </summary>
        public IrcClient()
        {
            m_Socket = new TcpClient();
            m_Thread = new Thread(new ParameterizedThreadStart(ParameterizedThreadCallback));
            m_SendQueue = new Queue<string>();
            queueThread = new Thread(new ThreadStart(QueueThreadCallback));
            m_Thread.IsBackground = true;

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

        public const Int32 QUEUE_DELAY = 750;

        #endregion

        #region Fields

        protected string m_AccessModes;
        protected string m_AccessPrefixes;
        protected string m_ChannelModes;
        protected IPEndPoint connection;
        protected readonly string m_LogFile = String.Format("ircd-{0}.log", DateTime.Now.ToString("MM-d-yyyy"));
        protected StreamReader reader;
        protected TcpClient m_Socket;
        protected NetworkStream stream;
        protected Thread m_Thread;
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
        ///     <para>Gets or sets the encoding for the specified IrcClient to send and receive data.</para>
        /// </summary>
        public Encoding Encoding
        {
            get { return m_Encoding; }
            set
            {
                if (!m_Socket.Connected)
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
            get { return m_Socket.Connected; }
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
                if (m_Socket.Connected)
                {
                    Send("QUIT");
                    Thread.Sleep(5);
                    connection = new IPEndPoint(Dns.GetHostEntry(value).AddressList[0], Port.Port);
                    m_Socket.Connect(connection);
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
            get { return m_Thread.IsBackground; }
            set
            {
                if ((m_Thread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted)
                {
                    m_Thread.IsBackground = value;
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

        private Int32 m_QueueDelay = QUEUE_DELAY;
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

                if (m_Socket.Connected)
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
            if (m_Socket != null && m_Socket.Connected)
            {
                if (String.IsNullOrEmpty(message))
                {
                    Send("QUIT");
                }
                else
                {
                    Send("QUIT :{0}", message);
                }

                m_Thread.Abort();
                m_Socket.Close();
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
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Mode '{0}' is being {1} with parameter: {2} (Type: {3})",mode, isSet ? "set" : "unset", parameter, type );
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
#if DEBUG
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
#if DEBUG
                            Console.WriteLine("debug: Regex Pattern: {0}", rRawNames.ToString());
                            Console.WriteLine("debug: Access Modes: {0} | Access Prefixes: {1}", m_AccessModes, m_AccessPrefixes);
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

        protected void QueueThreadCallback()
        {
            while (m_Socket != null && m_Socket.Connected)
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
            if (m_Socket != null && m_Socket.Connected)
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
            if (m_Socket != null && m_Socket.Connected)
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
                m_Socket.Connect(connection);
            }
            catch (SocketException)
            {
                // TODO: fire off disconnection event
            }

            m_Thread = new Thread(new ParameterizedThreadStart(ParameterizedThreadCallback));
            m_Thread.IsBackground = IsBackgroundThread;

            EventWaitHandle wait = new EventWaitHandle(false, EventResetMode.ManualReset);
            m_Thread.Start(wait);
            wait.WaitOne(30000);

            return m_Socket.Connected;
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
                m_Socket.Connect(connection);
            }
            catch (SocketException)
            {
                // TODO: fire off disconnection event
            }

            m_Thread.Start(null);

            return m_Socket.Connected;
        }

        /// <summary>
        ///     Disconnects the IrcClient from the server and closes the connection and all related resources are released.
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            if (m_Socket != null && m_Socket.Connected)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("QUIT :Disconnecting");
                byte[] data = Encoding.GetBytes(sb.ToString());

                stream.Write(data, 0, data.Length);
                stream.Flush();

                stream.Close();
                m_Socket.Close();

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
            if (m_Socket != null && m_Socket.Connected)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("QUIT :");
                sb.AppendFormat(format, args);
                sb.Append("\n");
                byte[] data = Encoding.GetBytes(sb.ToString());

                stream.Write(data, 0, data.Length);
                stream.Flush();

                stream.Close();
                m_Socket.Close();

                return true;
            }

            return false;
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
