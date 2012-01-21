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
 * Contributor(s): Zack "Genesis2001" Loveless, Benjamin "aca20031" Buzbee,
 *      Mark "SniperFodder" Gunnett
 * 
 */

namespace Atlantis.Enterprise.Xcf
{
    using System;

    /// <summary>
    ///     <para>  An Enumeration of Events that the {@link XCFParser} uses to parse. Each</para>
    ///     <para>Enum consists of an English readable version of the name of the event,</para>
    ///     <para>the Event ID, as well as a Keyword if there is one associated with it or</para>
    ///     <para>null if one is not.</para>
    /// </summary>
    public enum XcfEvent : int
    {
        [XcfEventData("Unexpected EOF", "")]
        UNEXPECTED_EOF = -1,

        [XcfEventData("Header Start", "xcf")]
        START_HEAD = 0,

        [XcfEventData("Header Stop", "xcf")]
        STOP_HEAD = 1,

        [XcfEventData("Document Begin", "")]
        BEGIN_DOCUMENT = 2,

        [XcfEventData("Document Body", "")]
        DOCUMENT_BODY = 3,

        [XcfEventData("Document End", "")]
        END_DOCUMENT = 4,

        [XcfEventData("Xcf Version Attribute", "version")]
        XCF_VERSION = 5,

        [XcfEventData("Xcf Encoding Attribute", "encoding")]
        XCF_ENCODING = 6,

        [XcfEventData("Xcf Include", "include")]
        XCF_INCLUDE = 7,

        [XcfEventData("Single-line Document Comment", "//")]
        DOC_COMMENT_SINGLE = 8,

        [XcfEventData("Multiline Document Comment Start", "/*")]
        DOC_COMMENT_MULTI_START = 9,

        [XcfEventData("Multiline Document Comment Stop", "*/")]
        DOC_COMMENT_MULTI_STOP = 10,

        [XcfEventData("Key Found", "")]
        KEY_FOUND = 11,

        [XcfEventData("Value Found", "")]
        VALUE_FOUND = 12,

        [XcfEventData("New Child", "")]
        NEW_CHILD = 13,

        [XcfEventData("Whitespace", "")]
        WHITE_SPACE = 14,

        [XcfEventData("Open Brace", "{")]
        OPEN_BRACE = 15,

        [XcfEventData("Close Brace", "}")]
        CLOSE_BRACE = 16,

        [XcfEventData("String Start", "\"")]
        STRING_START = 17,

        [XcfEventData("String End", "\"")]
        STRING_END = 18,

        [XcfEventData("Escape Character", "\\")]
        ESCAPE_CHARACTER = 19,

        [XcfEventData("Equal Sign", "=")]
        EQUAL_SIGN = 20,
    }
}