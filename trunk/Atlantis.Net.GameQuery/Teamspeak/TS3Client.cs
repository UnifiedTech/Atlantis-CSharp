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

namespace Atlantis.Net.GameQuery.Teamspeak
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Diagnostics;

    public class TS3Client
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        public TS3Client()
        {
            socket = new TcpClient();
            stream = socket.GetStream();

            reader = new StreamReader(stream, Encoding);
            reader = TextReader.Synchronized(new StreamReader(stream, Encoding));
        }

        /// <summary>
        ///     <para>Constructs an instance of the TS3Client object by providing all details about the server</para>
        /// </summary>
        /// <param name="host"></param>
        /// <param name="udpPort"></param>
        /// <param name="queryPort"></param>
        /// <param name="autoConnect"></param>
        /// <param name="userName"></param>
        /// <param name="password">This option is required if username is set</param>
        public TS3Client(String host, Int32 udpPort, Int32 queryPort, Boolean autoConnect = false, String userName = "", String password = "")
            : this()
        {


            if (autoConnect == true)
            {
                // Call .Connect(); && .Select(udpPort);


                if (!String.IsNullOrEmpty(userName) /* && Connected == true*/)
                {
                    // Call .Authenticate(userName, password);
                }
            }
        }

        #endregion

        #region Constants

        public const Boolean DefaultListen = false;
        public const Int32 DefaultQPort = 10011;
        public const Int32 DefaultUdpPort = 9987;

        #endregion

        #region Fields

        protected IPEndPoint connection;
        protected Queue<String> m_Queue;
        protected TextReader reader;
        protected TcpClient socket;
        protected NetworkStream stream;
        protected Thread thread;

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets a value indicating the TS3Client has connected to the server</para>
        /// </summary>
        public Boolean Connected
        {
            get
            {
                // TODO: Check whether the Socket itself is connected and whether the server has accepted our connection (and hasn't banned us)
                return false;
            }
        }

        private Encoding m_Encoding = Encoding.UTF8;
        /// <summary>
        ///     <para>Gets or sets a value indicating the proper encoding for socket I/O</para>
        /// </summary>
        public Encoding Encoding
        {
            get { return m_Encoding; }
            set { m_Encoding = value; }
        }

        /// <summary>
        ///     <para>Gets or sets a value indicating the Host</para>
        ///     <para>This property is required.</para>
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        ///     <para>Gets a value indicating whether the TS3Client has been setup properly</para>
        /// </summary>
        public Boolean IsInitialized
        {
            get
            {
                var ret = true;

                if (!String.IsNullOrEmpty(QLogin) && String.IsNullOrEmpty(QPassword)) ret = false;
                else if (String.IsNullOrEmpty(Host)) ret = false;
                else if (QPort <= 0 || UdpPort <= 0) ret = false;

                return ret;
            }
        }

        private Boolean m_Listen = DefaultListen;
        /// <summary>
        ///     <para>Gets or sets a value indicating that the TS3Client is supposed to listen for server events, and not just command responses</para>
        /// </summary>
        public Boolean Listen { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value indicating the Server Query Login name</para>
        /// </summary>
        public String QLogin { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value indicating the Server Query Password for the specified username</para>
        ///     <para>This property is required if TS3Client.QLogin is defined.</para>
        /// </summary>
        public String QPassword { get; set; }

        private Int32 m_QPort = DefaultQPort;
        /// <summary>
        ///     <para>Gets or sets a value indicating the Server Query Port</para>
        ///     <para>This property is required, but has a default value of 10011</para>
        /// </summary>
        public Int32 QPort
        {
            get { return m_QPort; }
            set { m_QPort = value; }
        }

        private Int32 m_UdpPort = DefaultUdpPort;
        /// <summary>
        ///     <para>Gets or sets a value indicating the virtual server instance's client connect port</para>
        ///     <para>This property is required, but has a default value of 9987</para>
        /// </summary>
        public Int32 UdpPort
        {
            get { return m_UdpPort; }
            set { m_UdpPort = value; }
        }

        // public TS3Client(String host, Int32 udpPort, Int32 queryPort, Boolean autoConnect = false, String userName = "", String password = "")
        #endregion

        #region Events
        #endregion

        #region Methods

        /// <summary>
        ///     <para>Establishes a connection to the TS3 Server</para>
        /// </summary>
        /// <returns></returns>
        public Boolean Connect()
        {
            if (!IsInitialized)
            {
                throw new NotImplementedException("The specified TS3Client has not been properly setup.");
            }

            try
            {
                connection = new IPEndPoint(Dns.GetHostEntry(Host).AddressList[0], QPort);
                socket.Connect(connection);
            }
            catch (SocketException)
            {
                // TODO: Fire off disconnection event
            }


            thread = new Thread(new ParameterizedThreadStart(ThreadCallback));
            thread.IsBackground = true;         // TODO: Create TS3Client.IsBackground property

            return Connected;
        }

        /// <summary>
        ///     <para>Sends a raw query command to the TS3 Server</para>
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public TS3Response Execute(String queryString, params object[] args)
        {
            string query = String.Format(queryString, args);

            if (Send(query))
            {
                var response1 = GetResponse();
                var response2 = GetResponse();

                Debug.WriteLine(String.Format("response1: {0}", response1));
                Debug.WriteLine(String.Format("response2: {0}", response2));
            }

            return default(TS3Response);
        }

        protected String GetResponse()
        {
            if (socket != null && socket.Connected)
            {
                return reader.ReadLine();
            }

            return null;
        }

        private void ThreadCallback(object obj)
        {
            // This method is for processing the server events from servereventnotifyregister command.
        }

        private bool Send(String dataToSend)
        {
            // @TODO - This method will be expanded when Queuing is implemented.
            if (socket != null && socket.Connected)
            {
                byte[] data = Encoding.GetBytes(dataToSend);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                return true;
            }

            return false;
        }

        #endregion
    }
}
