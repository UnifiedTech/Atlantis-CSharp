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

namespace Atlantis.IO
{
    using System;
    using System.IO;
    using System.Text;

    public partial class Logger
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="stream"></param>
        public Logger(Stream stream)
        {
            m_Stream = stream;
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="newLine"></param>
        public Logger(Stream stream, String newLine)
        {
            m_Stream = stream;
            m_NewLine = newLine;
        }

        #endregion

        #region Fields

        private Stream m_Stream;

        #endregion

        #region Properties

        private Boolean m_AutoFlush = false;
        /// <summary>
        ///     <para>Gets or sets a value indicating whether to auto-flush data written to the stream</para>
        /// </summary>
        public Boolean AutoFlush
        {
            get { return m_AutoFlush; }
            set { m_AutoFlush = value; }
        }

        private Encoding m_Encoding = Encoding.UTF8;
        /// <summary>
        ///     <para>Gets or sets a value indicating the Encoding format to be used when writing to the this Logger.</para>
        /// </summary>
        public Encoding Encoding
        {
            get { return m_Encoding; }
            set { m_Encoding = value; }
        }

        private String m_NewLine;
        /// <summary>
        ///     <para>Gets or sets a value indicating what the line terminator is for this Logger.</para>
        /// </summary>
        public String NewLine
        {
            get { return m_NewLine; }
            set { m_NewLine = value; }
        }

        private Boolean m_PrefixDate;
        /// <summary>
        ///     <para>Gets or sets a value indicating whether to prefix log messages with the current date and time</para>
        /// </summary>
        public Boolean PrefixDate
        {
            get { return m_PrefixDate; }
            set { m_PrefixDate = value; }
        }

        private String m_PrefixDateFormat = "- hh:mm:ss -";
        /// <summary>
        ///     <para>Gets or sets a value specifying the prefix format of the date</para>
        ///     <para>This has no effect if PrefixDate is disabled.</para>
        /// </summary>
        public String PrefixDateFormat
        {
            get { return m_PrefixDateFormat; }
            set { m_PrefixDateFormat = value; }
        }

        private Boolean m_PrefixLogs = true;
        /// <summary>
        ///     <para>Gets or sets a value indicating whether to prefix log messages</para>
        /// </summary>
        public Boolean PrefixLogs
        {
            get { return m_PrefixLogs; }
            set { m_PrefixLogs = value; }
        }

        #endregion

        #region Methods

        private void ApplyPrefixes(ref StringBuilder sb, Types type = Types.INFO)
        {
            if (PrefixDate)
            {
                sb.Append(DateTime.Now.ToString(PrefixDateFormat));
                sb.Append(' ');
            }

            if (PrefixLogs)
            {
                sb.Append(type.ToString());
                sb.Append(' ');
            }
        }

        /// <summary>
        ///     <para>Logs an error message to the current Logger</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Error(String format, params Object[] args)
        {
            StringBuilder sb = new StringBuilder();
            ApplyPrefixes(ref sb, Types.ERROR);

            sb.AppendFormat(format, args);
            sb.Append(NewLine);

            Write(sb.ToString());
        }

        /// <summary>
        ///     <para>Logs an informational warning to the current Logger</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Info(String format, params Object[] args)
        {
            StringBuilder sb = new StringBuilder();
            ApplyPrefixes(ref sb);

            sb.AppendFormat(format, args);
            sb.Append(NewLine);

            Write(sb.ToString());
        }

        /// <summary>
        ///     <para>Logs a warning to the current Logger</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Warning(String format, params Object[] args)
        {
            StringBuilder sb = new StringBuilder();
            ApplyPrefixes(ref sb, Types.WARNING);

            sb.AppendFormat(format, args);
            sb.Append(NewLine);

            Write(sb.ToString());
        }

        /// <summary>
        ///     <para>Writes the specified text to the current logger.</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLine(String format, params Object[] args)
        {
            StringBuilder sb = new StringBuilder();

            ApplyPrefixes(ref sb);

            sb.AppendFormat(format, args);
            sb.Append(NewLine);

            Write(sb.ToString());
        }

        private void Write(String line)
        {
            Byte[] data = Encoding.GetBytes(line);
            m_Stream.Write(data, 0, data.Length);

            if (AutoFlush)
            {
                m_Stream.Flush();
            }
        }

        #endregion
    }
}