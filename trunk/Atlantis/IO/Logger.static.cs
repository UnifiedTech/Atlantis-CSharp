﻿/*
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
    using System.Collections.Generic;
    using System.IO;

    public partial class Logger
    {
        #region Types

        internal enum Types
        {
            ERROR,
            INFO,
            WARNING,
        }

        #endregion

        #region Constructor(s)

        static Logger()
        {
            // Checks whether we're running as a Console Application. If we are, register
            // the console's standard output stream with the logger.
            if (!Environment.UserInteractive)
            {
                Logger.Create("Console", Console.OpenStandardOutput());
            }

            m_Loggers = new Dictionary<String, Logger>();
        }

        #endregion

        #region Fields

        private static Dictionary<String, Logger> m_Loggers;

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Creates a new Logger class, registering it for later use.</para>
        /// </summary>
        /// <param name="identifier">Required. The name of the logger to create.</param>
        /// <param name="stream">Required. The logger's output stream.</param>
        /// <param name="newLine">Optional. </param>
        /// <returns></returns>
        public static Logger Create(String identifier, Stream stream, String newLine = "\r\n")
        {
            if (m_Loggers.ContainsKey(identifier))
            {
                throw new ArgumentException("The specified logger has already been created.", "identifier");
            }

            var logger = new Logger(stream, newLine);
            m_Loggers.Add(identifier, logger);

            return logger;
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Logger GetLogger(String identifier)
        {
            if (m_Loggers.ContainsKey(identifier))
            {
                return m_Loggers[identifier];
            }
            else
            {
                throw new KeyNotFoundException("The specified identifier is invalid. Check spelling and try again.");
            }
        }

        #endregion
    }
}