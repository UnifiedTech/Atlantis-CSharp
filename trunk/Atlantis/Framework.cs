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
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    ///     <para></para>
    /// </summary>
    /// <devdoc>
    ///     <para>@TODO: Rename this class to something more meaningful</para>
    /// </devdoc>
    public static class Framework
    {
        #region Fields

        private static bool m_IsInitialized = false;

        #endregion

        #region Constants

        private const string DefaultLogDatePrefix = "-MM/dd/yyyy hh:mm:ss-";

        #endregion

        #region Properties
        private static string m_ApplicationData;
        /// <summary>
        ///     <para>Returns the calling assembly's application data path variable</para>
        /// </summary>
        /// <remarks>
        ///     <para>Typically this will be C:\Users\(current user)\AppData\Roaming\(assembly:CompanyName)\(assembly:ProductName)</para>
        /// </remarks>
        /// <exception cref="Atlantis.FrameworkUninitializedException" />
        public static string ApplicationData
        {
            get
            {
                if (!m_IsInitialized)
                    throw new FrameworkUninitializedException();

                return m_ApplicationData;
            }
        }

        private static string m_CommonAppData;
        /// <summary>
        ///     <para>returns the calling assembly's common appliation data path variable</para>
        /// </summary>
        /// <remarks>
        ///     <para>Typically this will be C:\ProgramData\(assembly:CompanyName)\(assembly:ProductName)</para>
        ///     <para>If you intend to use this, you will need to check whether the directory exists and create it if it doesn't.</para>
        /// </remarks>
        /// <exception cref="Atlantis.FrameworkUninitializedException" />
        public static string CommonApplicationData
        {
            get
            {
                if (!m_IsInitialized)
                    throw new FrameworkUninitializedException();

                return m_CommonAppData;
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
                if (!m_IsInitialized)
                    throw new FrameworkUninitializedException();

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
                if (!m_IsInitialized)
                    throw new FrameworkUninitializedException();

                return m_ExceptionLogger;
            }
            private set { m_ExceptionLogger = value; }
        }

        /// <summary>
        ///     <para>Gets a value indicating whether the Framework class has been initialized</para>
        /// </summary>
        public static bool IsInitialized
        {
            get { return m_IsInitialized; }
        }

        private static string m_StartupPath;
        /// <summary>
        ///     <para>Returns the current appliation's startup location without referencing <see cref="System.Windows.Forms.Application" />.</para>
        /// </summary>
        public static string StartupPath
        {
            get
            {
                if (!m_IsInitialized)
                    throw new FrameworkUninitializedException();

                return m_StartupPath;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Initializes Atlantis and sets up the framework class for application use.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="System.UnauthorizedException">Thrown when attempting to create directories.</exception>
        public static void Run<T>()
        {
            Type entry = typeof(T);

            object[] apps = entry.GetCustomAttributes(false);
            if (apps.Any())
            {
                ApplicationAttribute app = (ApplicationAttribute)apps[0];
                string location = entry.Assembly.Location;
                FileVersionInfo fi = FileVersionInfo.GetVersionInfo(location);

                string baseAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string baseCAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                m_ApplicationData = Path.Combine(baseAppData, fi.CompanyName, fi.ProductName);
                m_CommonAppData = Path.Combine(baseCAppData, fi.CompanyName, fi.ProductName);
                m_StartupPath = location.Substring(0, location.LastIndexOf('\\') + 1);

                if (!String.IsNullOrEmpty(app.CommonApplicationDataPath))
                {
                    m_CommonAppData = Path.Combine(baseAppData, app.CommonApplicationDataPath);
                }

                if (!String.IsNullOrEmpty(app.UserApplicationDataPath))
                {
                    m_ApplicationData = Path.Combine(baseAppData, app.UserApplicationDataPath);
                }

                if (app.CreateCommonAppData)
                {
                    Directory.CreateDirectory(m_CommonAppData);
                }

                if (app.CreateUserAppData)
                {
                    Directory.CreateDirectory(m_ApplicationData);
                }

                ApplicationUsage usage = app.Usage;
                // Check whether the app usage flag has been set. If not, throw an exception.
                if (usage.HasFlag(ApplicationUsage.Unknown))
                {
                    throw new InvalidFrameworkAttributeException();
                }

                string logpath = String.Empty;
                if (app.CreateUserAppData)
                    logpath = m_ApplicationData;
                else if (app.CreateCommonAppData)
                    logpath = m_CommonAppData;
                else
                    logpath = m_StartupPath;

                //  Check whether we're running as a console app.
                if (usage.HasFlag(ApplicationUsage.Console))
                {
                    Console = Logger.GetLogger("Console", System.Console.OpenStandardOutput(), Environment.NewLine);
                }
                else
                {
                    string appLog = String.Empty;
                    FileStream stream = null;
                    if (usage.HasFlag(ApplicationUsage.Window))
                    {
                        appLog = Path.Combine(logpath, @"app.log");
                    }
                    else if (usage.HasFlag(ApplicationUsage.Service))
                    {
                        appLog = Path.Combine(m_StartupPath, @"service.log");
                    }

                    stream = new FileStream(appLog, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                    Console = Logger.GetLogger("Console", stream, Environment.NewLine);
                }

                string errorLog = Path.Combine(logpath, @"error.log");
                FileStream errorStream = new FileStream(errorLog, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                Exceptions = Logger.GetLogger("Exceptions", errorStream, Environment.NewLine);

                m_IsInitialized = true;

                Exceptions.PrefixDateFormat = DefaultLogDatePrefix;
                Console.PrefixDateFormat = DefaultLogDatePrefix;
                Console.Info("Atlantis initialized.");
            }
            else
            {
                throw new InvalidFrameworkAttributeException();
            }
        }

        [Obsolete]
        /// <summary>
        ///     <para>Initializes and setups the Framework class basic necessities</para>
        /// </summary>
        public static void Initialize()
        {
            string location = Assembly.GetEntryAssembly().Location;
            FileVersionInfo fi = FileVersionInfo.GetVersionInfo(location);
            m_ApplicationData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fi.CompanyName, fi.ProductName);
            m_CommonAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), fi.CompanyName, fi.ProductName);
            m_StartupPath = location.Substring(0, location.LastIndexOf("\\") + 1);

            // Checks whether we're running as a Console Application. If we are, register
            // the console's standard output stream with the logger.
            if (Environment.UserInteractive)
            {
                Console = Logger.GetLogger("Console", System.Console.OpenStandardOutput(), Environment.NewLine);
            }

            string exceptionLog = Path.Combine(m_StartupPath, "exceptions.log");
            if (!File.Exists(exceptionLog))
            {
                File.Create(exceptionLog).Close();
            }

            var eStream = new FileStream(exceptionLog, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            Exceptions = Logger.GetLogger("exceptions", eStream);

            m_IsInitialized = true;
        }

        #endregion
    }
}