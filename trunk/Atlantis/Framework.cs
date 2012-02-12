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

namespace Atlantis
{
    using Atlantis.IO;
    using System;
    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.Diagnostics;

    /// <summary>
    ///     <para></para>
    /// </summary>
    /// <devdoc>
    ///     <para>@TODO: Rename this class to something more meaningful</para>
    /// </devdoc>
    public static class Framework
    {
        #region Constructor(s)

        #endregion

        #region Constants
        // Put all your constant declarations here
        #endregion

        #region Fields

        private static Boolean m_Initialized = false;

        #endregion

        #region Properties

        private static String m_ApplicationData;
        /// <summary>
        ///     <para>Returns the calling assembly's application data path variable</para>
        /// </summary>
        /// <exception cref="Atlantis.FrameworkUninitializedException" />
        public static String ApplicationData
        {
            get
            {
                if (!m_Initialized)
                {
                    throw new FrameworkUninitializedException();
                }

                return m_ApplicationData;
            }
        }

        private static Logger m_ConsoleLogger;
        /// <summary>
        ///     <para>Gets the logger associated with Console output</para>
        /// </summary>
        /// <exception cref="Atlantis.FrameworkUninitializedException" />
        public static Logger Console
        {
            get
            {
                if (!m_Initialized)
                {
                    throw new FrameworkUninitializedException();
                }

                return m_ConsoleLogger;
            }
            private set { m_ConsoleLogger = value; }
        }

        private static Logger m_ExceptionLogger;
        /// <summary>
        ///     <para>Gets the logger associated with exception handling</para>
        /// </summary>
        /// <exception cref="Atlantis.FrameworkUninitializedException" />
        public static Logger Exceptions
        {
            get
            {
                if (!m_Initialized)
                {
                    throw new FrameworkUninitializedException();
                }

                return m_ExceptionLogger;
            }
            private set { m_ExceptionLogger = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Initializes and setups the Framework class basic necessities</para>
        /// </summary>
        public static void Initialize()
        {
            FileVersionInfo fi = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            m_ApplicationData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fi.ProductName);

            if (!Directory.Exists(m_ApplicationData))
            {
                Directory.CreateDirectory(m_ApplicationData);
            }

            // Checks whether we're running as a Console Application. If we are, register
            // the console's standard output stream with the logger.
            if (Environment.UserInteractive)
            {
                Console = Logger.GetLogger("Console", System.Console.OpenStandardOutput(), Environment.NewLine);
            }

            if (!File.Exists(Path.Combine(m_ApplicationData, "exceptions.log")))
            {
                File.Create(Path.Combine(m_ApplicationData, "exceptions.log")).Close();
            }

            var eStream = new FileStream(Path.Combine(ApplicationData, "exceptions.log"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            Exceptions = Logger.GetLogger("exceptions", eStream);

            m_Initialized = true;
        }

        #endregion
    }
}