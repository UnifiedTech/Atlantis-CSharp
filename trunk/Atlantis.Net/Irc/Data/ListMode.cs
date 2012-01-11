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

namespace Atlantis.Net.Irc.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public class ListMode
    {

        #region Constructor(s)

        public ListMode(DateTime date, string mask, string setBy, char type)
        {
            m_Date = date;
            m_Mask = mask;
            m_SetBy = setBy;
            m_Type = type;
        }

        #endregion

        #region Properties

        private DateTime m_Date;
        /// <summary>
        ///     <para>Gets the date when the ListMode was set</para>
        /// </summary>
        public DateTime Date
        {
            get { return m_Date; }
        }

        private string m_Mask;
        /// <summary>
        ///     <para>Gets the mask of the current ListMode</para>
        /// </summary>
        public string Mask
        {
            get { return m_Mask; } 
        }

        private string m_SetBy;
        /// <summary>
        ///     <para>Gets the setter of the current ListMode</para>
        /// </summary>
        public string SetBy
        {
            get { return m_SetBy; }
        }

        private char m_Type;
        /// <summary>
        ///     <para>Gets the type of ListMode this represents</para>
        /// </summary>
        public char Type
        {
            get { return m_Type; }
        }

        #endregion

    }
}
