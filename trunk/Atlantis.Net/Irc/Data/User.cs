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
    using Atlantis.Net.Irc.Linq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public struct User
    {

        #region Constructor(s)

        public User(string host, string name)
        {
            m_Host = host;
            m_Name = name;
        }

        #endregion

        #region Properties

        private string m_Host;
        /// <summary>
        ///     <para>Gets the host of the current user</para>
        /// </summary>
        public string Host
        {
            get { return m_Host; }
        }

        private string m_Name;
        /// <summary>
        ///     <para>Gets the name of the current User</para>
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Formats a full address of a user in a certain mask format as in the following list where m = masktype % 10 and fulladdress = nick!~user@some.host.com</para>
        ///     <list type="bullet">
        ///         <item>
        ///             <term>0</term>
        ///             <description>No nick: *!user@some.host.com</description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>No nick or identd marker (~): *!*user@some.host.com</description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>No nick or identd: *!*@some.host.com</description>
        ///         </item>
        ///         <item>
        ///             <term>3</term>
        ///             <description>No nick or identd marker, no first host token: *!*user@*.host.com (or *!*user@host if host contains no '.')</description>
        ///         </item>
        ///         <item>
        ///             <term>4</term>
        ///             <description>No nick or identd: *!*@*.host.com (or *!*@host if host contains no '.')</description>
        ///         </item>
        ///         <item>
        ///             <term>5</term>
        ///             <description>No change: nick!~user@some.host.com</description>
        ///         </item>
        ///         <item>
        ///             <term>6</term>
        ///             <description>No identd marker (~): nick!*user@some.host.com</description>
        ///         </item>
        ///         <item>
        ///             <term>7</term>
        ///             <description>No identd: nick!*@some.host.com</description>
        ///         </item>
        ///         <item>
        ///             <term>8</term>
        ///             <description>No identd marker (~) or first host token: nick!*user@*.host.com (or nick!*user@host if host contains no '.')</description>
        ///         </item>
        ///         <item>
        ///             <term>9</term>
        ///             <description>No identd or first host token: nick!*@*.host.com (or nick!*@host if host contains no '.')</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="masktype"></param>
        /// <param name="address">Optional. The address to return a mask of</param>
        /// <returns></returns>
        public string Mask(int masktype, string address = "")
        {
            if (String.IsNullOrEmpty(address))
            {
                address = Host;
            }

            string nick = address.ClipAddress(AddressPart.NICK);
            string identd = address.ClipAddress(AddressPart.IDENTD);
            string host = address.ClipAddress(AddressPart.HOST);

            int m = masktype % 10;
            switch (m)
            {
                case 0: // *!user@some.host.com
                    {
                        return String.Format("*!{0}@{1}", identd, host);
                    }
                case 1: //  *!*user@some.host.com
                    {
                        if (identd[0] == '~') // If starts with marker, clip marker
                        {
                            identd = identd.Substring(1);
                        }
                        else if (identd[0] == '*')
                        {
                            return String.Format("*!{0}@{1}", identd, host);
                        }
                        return String.Format("*!*{0}@{1}", identd, host);
                    }
                case 2: // *!*@some.host.com
                    {
                        return String.Format("*!*@{0}", host);
                    }
                case 3: // *!*user@*.host.com 
                    {
                        string[] hosttokens = host.Split('.');
                        if (hosttokens.Length > 1)
                        {
                            host = "*." + String.Join(".", hosttokens, 1, hosttokens.Length - 1);
                        }
                        return Mask(1, String.Format("{0}!{1}@{2}", nick, identd, host));
                    }

                case 4: //*!*@*.host.com
                    {
                        string[] hosttokens = host.Split('.');
                        if (hosttokens.Length > 1)
                        {
                            host = String.Format("*.{0}", String.Join(".", hosttokens, 1, hosttokens.Length - 1));
                        }
                        return Mask(2, String.Format("*!*@{0}", host));
                    }

                case 5: // No change
                    {
                        return address;
                    }

                case 6:  // nick!*user@some.host.com
                    {
                        if (identd[0] == '~')
                        {                            // If starts with marker, clip marker
                            identd = identd.Substring(1);
                        }
                        else if (identd[0] == '*')
                        {
                            return String.Format("{2}!{0}@{1}", identd, host, nick);
                        }
                        return String.Format("{2}!*{0}@{1}", identd, host, nick);
                    }
                case 7: // nick!*@some.host.com
                    {
                        return String.Format("{0}!*@{1}", nick, host);
                    }
                case 8: // nick!*user@*.host.com 
                    {
                        string[] hosttokens = host.Split('.');
                        if (hosttokens.Length > 1)
                        {
                            host = "*." + String.Join(".", hosttokens, 1, hosttokens.Length - 1);
                        }
                        string identdMasked = Mask(6, address);
                        return identdMasked.Substring(0, identdMasked.IndexOf('@') + 1) + host;
                    }
                case 9: // nick!*@*.host.com 
                    {
                        string hostMasked = Mask(8, address);
                        return String.Format("{0}!*@{1}", nick, hostMasked.Substring(hostMasked.IndexOf('@') + 1));
                    }

                default:
                    {
                        return "A";
                        throw new Exception("Logic fault. Impossible.");
                    }
            }
        }

        #endregion

    }
}
