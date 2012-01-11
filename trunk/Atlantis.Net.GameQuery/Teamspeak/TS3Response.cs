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

namespace Atlantis.Net.GameQuery.Teamspeak
{
    using System;
    using System.Collections.Generic;

    public class TS3Response
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Constructs a response message containing data received from the TS3 Server</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorId"></param>
        /// <param name="kvp"></param>
        public TS3Response(String message, Int32 errorId, IDictionary<String, String> kvp = null)
        {
            Message = message;
            ErrorId = errorId;

            if (kvp != null)
            {
                Data = kvp;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets the code associated with the response</para>
        /// </summary>
        public Int32 ErrorId { get; protected set; }

        /// <summary>
        ///     <para>Gets a value indicating the message from the result set</para>
        /// </summary>
        public String Message { get; protected set; }

        /// <summary>
        ///     <para>Represents the optional meta data that the server returns</para>
        ///     <para>Warning: This will be undefined if a command doesn't return any meta data</para>
        /// </summary>
        public IDictionary<String, String> Data { get; protected set; }

        #endregion
    }
}