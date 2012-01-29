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

namespace Atlantis.Net.Irc.Linq
{
    using Atlantis.Net.Irc.Data;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum AddressPart { NICK, IDENTD, HOST }

    [Flags]
    public enum ControlFlags
    {
        BOLD = 1,
        UNDERSCORE = 2,
        REVERSE = 4,
        COLOR = 8,
        ITALICS = 16,
        ALL = BOLD | UNDERSCORE | REVERSE | COLOR | ITALICS
    }

    public static class StringExtensions
    {

        /// <summary>
        ///     <para>The control code for generic messages</para>
        ///     <para>Examples this is used in: CTCP requests/responses</para>
        /// </summary>
        public const Char CONTROL_GENERIC = (Char)1;

        /// <summary>
        /// Makes all text between CONTROL_BOLD and CONTROL_BOLD or CONTROL_CANCEL bold in mIRC clients.
        /// </summary>
        public const Char CONTROL_BOLD = (Char)2;

        /// <summary>
        /// Makes all text between CONTROL_COLOR and CONTROL_COLOR or CONTROL_CANCEL the color represented by the numbers which follow it in mIRC clients.
        /// </summary>
        public const Char CONTROL_COLOR = (Char)3;

        /// <summary>
        /// Ends all CONTROL Character effects.
        /// </summary>
        public const Char CONTROL_CANCEL = (Char)15;

        /// <summary>
        /// Makes all text between CONTROL_REVERSE and CONTROL_REVERSE or CONTROL_CANCEL reversed in color (foreground = background, background = foreground) in mIRC clients.
        /// </summary>
        public const Char CONTROL_REVERSE = (Char)22;

        /// <summary>
        /// Makes all text between CONTROL_ITALICS and CONTROL_ITALICS or CONTROL_CANCEL italicized in mIRC clients.
        /// </summary>
        public const Char CONTROL_ITALICS = (Char)29;

        /// Makes all text between CONTROL_UNDERSCORE and CONTROL_UNDERSCORE or CONTROL_CANCEL underscored in mIRC clients.
        public const Char CONTROL_UNDERSCORE = (Char)31;

        /// <summary>
        ///     <para>Returns the specified portion of a full address in format nick!user@host.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="part"></param>
        /// <exception cref="MalformedAddressException" />
        /// <returns></returns>
        public static String ClipAddress(this String source, AddressPart part)
        {
            int index1 = source.IndexOf('!');
            int index2 = source.IndexOf('@');

            if (index1 > index2)
            {
                throw new MalformedAddressException(source, "'!' is located after '@'");
            }
            else if (index1 + 1 == source.Length || index2 + 1 == source.Length)
            {
                throw new MalformedAddressException(source, "No identd or host body");
            }
            else if (index1 == 0)
            {
                throw new MalformedAddressException(source, "No nick body");
            }

            string identd = source.Substring(index1 + 1, index2 - index1 - 1);
            if (identd[0] == '~' && identd.Length == 1)
            {
                throw new MalformedAddressException(source, "Identd contains only the identd marker (~) and is therefore invalid");
            }


            switch (part)
            {
                case AddressPart.HOST:
                    return source.Substring(index2 + 1);
                case AddressPart.IDENTD:
                    return identd;
                case AddressPart.NICK:
                    return source.Substring(0, index1);
                default:
                    throw new Exception("Error in ClipAddress: AddressPart enumeration not handled.");

            }
        }

        /// <summary>
        ///     <para>Strips mIRC control codes from line in O(n) time. Note: This method ALWAYS strips CONTROL_CANCEL.</para>
        /// </summary>
        /// <param name="line">Line from which to remove control codes.</param>
        /// <param name="flags">Type of codes to remove. These are bitwise flags and may be bitwise OR'd, default ALL.</param>
        /// <returns>Returns tripped version of line.</returns>
        public static String Strip(this String source, ControlFlags flags = ControlFlags.ALL)
        {
            StringBuilder sb = new StringBuilder(source.Length);
            int colorStage = 0; // 0 means not in a color code
                                // 1 means we have gotten the code and no digits
                                // 2 means we have gotten the code and 1 digit (need , or digit)
                                // 3 means we have gotten the code and 2 digits (need ,)
                                // 4 means we are up to a comma (need 1-2 digits)
                                // 5 means we have gotten a comma and a digit (need 1 digit)

            for (int i = 0; i < source.Length; ++i)
            {
                if (source[i] == CONTROL_CANCEL ||
                    source[i] == CONTROL_BOLD && (flags & ControlFlags.BOLD) == ControlFlags.BOLD ||
                    source[i] == CONTROL_UNDERSCORE && (flags & ControlFlags.UNDERSCORE) == ControlFlags.UNDERSCORE ||
                    source[i] == CONTROL_REVERSE && (flags & ControlFlags.REVERSE) == ControlFlags.REVERSE ||
                    source[i] == CONTROL_ITALICS && (flags & ControlFlags.ITALICS) == ControlFlags.ITALICS)
                {
                    colorStage = 0;
                    continue;
                }
                else if ((flags & ControlFlags.COLOR) == ControlFlags.COLOR && (source[i] == CONTROL_COLOR || Char.IsDigit(source[i]) || source[i] == ','))
                {
                    if (source[i] == CONTROL_COLOR)
                        colorStage = 1;
                    else if (source[i] == ',' && i + 1 < source.Length && Char.IsDigit(source[i + 1]) && colorStage > 1 && colorStage < 4)
                        colorStage = 4;
                    else if (Char.IsDigit(source[i]))
                    {
                        if (colorStage > 0 && colorStage < 6 && colorStage != 3)
                            ++colorStage;
                        else
                        {
                            colorStage = 0;
                            sb.Append(source[i]);
                        }
                    }
                    else
                    {
                        colorStage = 0;
                        sb.Append(source[i]);
                    }
                }
                else
                {
                    colorStage = 0;
                    sb.Append(source[i]);
                }
            }

            return sb.ToString();
        }

    }
}
