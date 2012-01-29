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

namespace Atlantis.Net.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    public static class StringExtensions
    {
        #region Methods

        /// <summary>
        ///     <para>Converts a string IP and Port combination into an IPEndPoint</para>
        ///     <para>IP-Port string needs to be in format: 0.0.0.0:0000</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="System.FormatException" />
        public static IPEndPoint ToIPEndPoint(this String source)
        {
            string[] parts = source.Split(':');

            if (parts.Length != 2)
            {
                throw new FormatException("Invalid format for IP String.");
            }

            return new IPEndPoint(IPAddress.Parse(parts[0]), Convert.ToInt32(parts[1]));
        }

        #endregion
    }
}
