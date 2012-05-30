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
    using System;

    /// <summary>
    ///     <para>Represents a list of startup types for the framework.</para>
    /// </summary>
    public enum ApplicationUsage : int
    {
        /// <summary>
        ///     <para>Indicates that the application doesn't know what it's being ran</para>
        /// </summary>
        Unknown = 1,

        /// <summary>
        ///     <para>Indicates that the application is being ran as a Console Application.</para>
        /// </summary>
        Console,

        /// <summary>
        ///     <para>Indicates that the application is being ran as a Windows service.</para>
        /// </summary>
        Service,

        /// <summary>
        ///     <para>Indicates that the application is being ran as a Windows Forms/WPF application.</para>
        /// </summary>
        Window,
    }

    /// <summary>
    ///     <para>Allows Atlantis to initialize default variables, like paths, for ease of access at a later date.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ApplicationAttribute : System.Attribute
    {
        #region Constructor(s)

        public ApplicationAttribute(ApplicationUsage usage)
        {
            SetUsage(usage);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets a value indicating how Atlantis will be used.</para>
        /// </summary>
        public ApplicationUsage Usage { get; private set; }

        /// <summary>
        ///     <para>Gets or sets a value indicating whether to create the common application data directory.</para>
        /// </summary>
        public bool CreateCommonAppData { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value indicating whether to create the user-specific application data directory.</para>
        /// </summary>
        public bool CreateUserAppData { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value indicating the path to append to the system defined common application data path.</para>
        /// </summary>
        public string CommonApplicationDataPath { get; set; }

        /// <summary>
        ///     <para>Gets or sets a value indicating the path to append to the system defined user-specific application data path.</para>
        /// </summary>
        public string UserApplicationDataPath { get; set; }

        #endregion

        #region Methods

        private void SetUsage(ApplicationUsage usage)
        {
            Usage = usage;
        }

        #endregion
    }
}