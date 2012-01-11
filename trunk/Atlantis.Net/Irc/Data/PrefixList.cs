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

    /// <summary>
    /// Class Name: PrefixList
    /// Author: Ben Buzbee
    /// Description:
    ///     Represents a list of irc prefixes for a specific client. Generally associated with a specific user on a specific channel.
    /// </summary>
    public class PrefixList
    {

        /// <summary>
        /// Stores all prefixes this user has, however its length is the length of client.AccessPrefixes so it can store all valid prefixes without resizing.
        /// </summary>
        char[] prefixes;
        IrcClient client;
        public PrefixList(IrcClient client)
        {
            if (client == null)
            {
#if DEBUG2
                throw new Exception("PrefixList constructor null client passed. A valid client must be given!");
#endif
                prefixes = new char[0];
                return;
            }

            this.client = client;
            prefixes = new char[client.AccessPrefixes.Length];
        }

        public virtual bool AddPrefix(char prefix)
        {
            for (int i = 0; i < prefixes.Length; ++i)
            {
                if (prefixes[i] == 0 || prefixes[i] == prefix)
                {
                    bool success = prefixes[i] == 0;
                    prefixes[i] = prefix;
                    if (success)
                        ReSort();
                    return success;
                }
            }

            // If we get here, we found no empty spot (and no duplicate) which means we inserted more prefixes than this client says we can have!
#if DEBUG2
            throw new Exception("PrefixList AddPrefix attempted to add more prefixes to a user than the client supports.");
#endif
            return false;
        }

        public virtual bool RemovePrefix(char prefix)
        {
            for (int i = 0; i < prefixes.Length; ++i)
            {
                if (prefixes[i] == prefix)
                {
                    prefixes[i] = (char)0;
                    ReSort();
                    return true;
                }
            }
            return false;
        }

        protected virtual void ReSort()
        {
            Array.Sort(prefixes, (a, b) =>
            {
                if (a == 0 && b == 0)
                    return 0;
                else if (a == 0)
                    return 1;
                else if (b == 0)
                    return -1;
                int aIndex = client.AccessPrefixes.IndexOf(a);
                int bIndex = client.AccessPrefixes.IndexOf(b);
                if (aIndex < 0 || bIndex < 0)
                {
#if DEBUG2
                    throw new Exception("PrefixList ReSort one of the characters to be sorted is not in the client's prefix list.");
#endif
                    return 0;
                }
                return aIndex - bIndex;
            });
        }

        public char HighestPrefix { get { if (prefixes.Length > 0) return prefixes[0]; else return (char)0; } }
        /// <summary>
        /// Returns all prefixes this user has
        /// </summary>
        public char[] Prefixes
        {
            get
            {
                char[] retval;
                int prefixesc;
                for (prefixesc = 0; prefixesc < prefixes.Length && prefixes[prefixesc] != 0; ++prefixesc) ; // Calculate number of valid prefixes in array
                retval = new char[prefixesc];

                for (int i = 0; i < prefixesc; ++i)
                {
                    retval[i] = prefixes[i];
                }
                return retval;
            }

        }



        /// <summary>
        /// Converts a user access mode to a prefix.
        /// </summary>
        /// <param name="mode">Mode to convert.</param>
        /// <returns>Prefix corresponding to this mode, or 0 if error.</returns>
        public virtual char ModeToPrefix(char mode)
        {
            int i = client.AccessModes.IndexOf(mode);
            if (i < 0)
            {
#if DEBUG2
                throw new Exception("ModeToPrefix mode passed is not in accessModes.");
#endif
                return (char)0;
            }
            else if (i >= client.AccessPrefixes.Length)
            {
#if DEBUG2
                throw new Exception("ModeToPrefix accessPrefixes and accessModes are different lengths.");
#endif
                return (char)0;
            }
            else
                return client.AccessPrefixes[i];
        }
        /// <summary>
        /// Converts a user access prefix to a mode.
        /// </summary>
        /// <param name="prefix">Prefix to convert.</param>
        /// <returns>Mode corresponding to this prefix, or 0 if error.</returns>
        public virtual char PrefixToMode(char prefix)
        {
            int i = client.AccessPrefixes.IndexOf(prefix);
            if (i < 0)
            {
#if DEBUG2
                throw new Exception("PrefixtoMode prefix passed is not in accessPrefixes.");
#endif
                return (char)0;
            }
            else if (i >= client.AccessModes.Length)
            {
#if DEBUG2
                throw new Exception("PrefixToMode accessPrefixes and accessModes are different lengths.");
#endif
                return (char)0;
            }
            else
                return client.AccessModes[i];
        }

    }
}
