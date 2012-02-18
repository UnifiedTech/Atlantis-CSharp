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

namespace Atlantis.Win32
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    /* TO-DO LIST
     * 1. Implement net.exe API calls in this class http://msdn.microsoft.com/en-us/library/aa370675%28VS.85%29.aspx
     */

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct USER_INFO_0
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri0_name;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct USER_INFO_2
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String password;
        [MarshalAs(UnmanagedType.U4)]
        public int password_age;
        public IntPtr priv;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String home_dir;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String comment;
        public IntPtr flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String script_path;
        public IntPtr auth_flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String full_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usr_comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String parms;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String workstations;
        [MarshalAs(UnmanagedType.U4)]
        public int last_logon;
        [MarshalAs(UnmanagedType.U4)]
        public int last_logoff;
        [MarshalAs(UnmanagedType.U4)]
        public int acct_expires;
        [MarshalAs(UnmanagedType.U4)]
        public int max_storage;
        [MarshalAs(UnmanagedType.U4)]
        public int units_per_week;
        public IntPtr logon_hours;
        [MarshalAs(UnmanagedType.U4)]
        public int bad_pw_count;
        [MarshalAs(UnmanagedType.U4)]
        public int num_logons;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String logon_server;
        [MarshalAs(UnmanagedType.U4)]
        public int country_code;
        [MarshalAs(UnmanagedType.U4)]
        public int code_page;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct USER_INFO_3
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_password;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_password_age;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_priv;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_home_dir;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_comment;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_script_path;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_auth_flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_full_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_usr_comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_parms;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_workstations;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_last_logon;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_last_logoff;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_acct_expires;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_max_storage;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_units_per_week;
        public IntPtr usri3_logon_hours;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_bad_pw_count;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_num_logons;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_logon_server;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_country_code;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_code_page;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_user_id;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_primary_group_id;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_profile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri3_home_dir_drive;
        [MarshalAs(UnmanagedType.U4)]
        public int usri3_password_expired;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct USER_INFO_4
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_password;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_password_age;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_priv;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_home_dir;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_comment;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_script_path;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_auth_flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_full_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_usr_comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_parms;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_workstations;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_last_logon;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_last_logoff;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_acct_expires;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_max_storage;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_units_per_week;
        public IntPtr usri4_logon_hours;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_bad_pw_count;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_num_logons;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_logon_server;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_country_code;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_code_page;
        public IntPtr usri4_user_sid;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_primary_group_id;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_profile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri4_home_dir_drive;
        [MarshalAs(UnmanagedType.U4)]
        public int usri4_password_expired;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct USER_INFO_10
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri10_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri10_comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri10_usr_comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri10_full_name;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct USER_INFO_11
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_usr_comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_full_name;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_priv;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_auth_flags;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_password_age;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_home_dir;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_parms;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_last_logon;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_last_logoff;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_bad_pw_count;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_num_logons;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_logon_server;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_country_code;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String usri11_workstations;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_max_storage;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_units_per_week;
        public IntPtr usri11_logon_hours;
        [MarshalAs(UnmanagedType.U4)]
        public int usri11_code_page;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct USER_INFO_20
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri20_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri20_full_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri20_comment;
        [MarshalAs(UnmanagedType.U4)]
        public int usri20_flags;
        [MarshalAs(UnmanagedType.U4)]
        public int usri20_user_id;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct USER_INFO_23
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri23_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri23_full_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri23_comment;
        [MarshalAs(UnmanagedType.U4)]
        public int usri23_flags;
        public IntPtr usri23_user_sid;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WKSTA_USER_INFO_0
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String wkui0_username;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WKSTA_USER_INFO_1
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public String wkui1_username;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String wkui1_logon_domain;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String wkui1_oth_domains;
        [MarshalAs(UnmanagedType.LPWStr)]
        public String wkui1_logon_server;
    }

    public static class Network
    {
        #region Methods

        /// <summary>
        ///     Lists information about all users currently logged on to the workstation.
        /// </summary>
        /// <param name="servername"></param>
        /// <param name="level"></param>
        /// <param name="bufptr"></param>
        /// <param name="prefmaxlen"></param>
        /// <param name="entriesread"></param>
        /// <param name="totalentries"></param>
        /// <param name="resume_handle"></param>
        /// <returns></returns>
        [DllImport("netapi32.dll", EntryPoint = "NetWkstaUserEnum", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetUserName(
           string servername,
           int level,
           out IntPtr bufptr,
           int prefmaxlen,
           out int entriesread,
           out int totalentries,
           ref int resume_handle);

        [DllImport("Advapi32.dll", EntryPoint = "GetUserName", ExactSpelling = false, SetLastError = true)]
        public static extern bool GetUserName(
                [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
                [MarshalAs(UnmanagedType.LPArray)] int[] nSize);

        /// <summary>
        ///     The NetApiBufferFree function frees the memory that the NetApiBufferAllocate function allocates.
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        [DllImport("Netapi32.dll", SetLastError = true)]
        public static extern int NetApiBufferFree(IntPtr Buffer);

        [CLSCompliant(false)]
        /// <summary>
        ///     <para>The NetUserGetInfo function retrieves information about a particular user account on a server.</para>
        ///     <para>For more Information, see <see cref="http://www.pinvoke.net/default.aspx/netapi32/NetUserGetInfo.html" /></para>
        /// </summary>
        /// <param name="servername"></param>
        /// <param name="username"></param>
        /// <param name="level"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        [DllImport("netapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int NetUserGetInfo(
            [MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string username,
            uint level,
            out IntPtr buffer);

        /// <summary>
        ///     Broadcast a message over the network using the native Win32 Messenger Service
        /// </summary>
        /// <param name="servername">null</param>
        /// <param name="msgname"></param>
        /// <param name="fromname">null</param>
        /// <param name="buf"></param>
        /// <param name="buflen"></param>
        /// <returns></returns>
        [DllImport("netapi32.dll", EntryPoint = "NetMessageBufferSend")]
        public static extern int Send(
            [MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string msgname,
            [MarshalAs(UnmanagedType.LPWStr)] string fromname,
            [MarshalAs(UnmanagedType.LPWStr)] string buf,
            int buflen);

        #endregion
    }
}