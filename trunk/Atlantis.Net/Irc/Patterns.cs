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

namespace Atlantis.Net.Irc
{
    using Atlantis.Net.Irc.Linq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;


    /// <summary>
    ///     A class containing a set of regular expressions available to be matched by the IRC Object
    /// </summary>
    public static class Patterns
    {
        // public static readonly Regex rAlphaNumericRegex = new Regex("[a-zA-Z0-9]", RegexOptions.Compiled);
        public static readonly Regex rChannelModes = new Regex("CHANMODES=(\\S+)"); // CHANMODES=beI,kfL,lj,psmntirRcOAQKVCuzNSMTGy
        public static readonly Regex rChannelRegex
            = new Regex(":?(#[^!]+)", RegexOptions.Compiled); // :#Channel
        public static readonly Regex rNumericStartRegex
            = new Regex("[^0-9][a-zA-Z0-9]");
        public static readonly Regex rUserHost
            = new Regex(@":?([^!]+)!([^@]+@\S+)", RegexOptions.Compiled);// :Global!services@phantomlink.net
        public static readonly Regex rUserPrefix
            = new Regex(@"PREFIX=\((\S+)\)(\S+)");  // PREFIX=(qaohv)~&@%+

        //public static readonly Regex rCtcpRequest = new Regex(String.Format(":?([^!]+)!([^@]+@\\S+) PRIVMSG (#?[^!]+) :{0}(\\s){0} (.+)", StringExtensions.CONTROL_CTCP), RegexOptions.Compiled);
        //public static readonly Regex rCtcpReply = new Regex(String.Format(":?([^!]+)!([^@]+@\\S+) NOTICE (#?[^!]+) :{0}(.+){0}", StringExtensions.CONTROL_CTCP), RegexOptions.Compiled);

        public static readonly Regex rPrivmsg
            = new Regex(":?([^!]+)!([^@]+@\\S+) PRIVMSG (#?[^!]+) :(.+)");
        public static readonly Regex rNotice = new Regex(":?([^!]+)!([^@]+@\\S+) NOTICE (#?[^!]+) :(.+)");
    }
}
