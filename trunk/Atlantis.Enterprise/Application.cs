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

namespace Atlantis.Enterprise
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Application
    {

        #region Constructor(s)
        #endregion

        #region Properties

        private static string m_CompanyName;
        /// <summary>
        ///     <para>Gets or sets the CompanyName of the Application</para>
        /// </summary>
        public static string CompanyName
        {
            get { return m_CompanyName; }
            set { m_CompanyName = value; }
        }

        #endregion

        #region Methods
        #endregion

    }
}
