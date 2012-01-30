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

namespace Atlantis.Windows.Forms.Linq
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    public static class WinFormsExtensions
    {

        /// <summary>
        ///     <para>Opens a file from an OpenFileDialog allowing for extended IO Access to the underlying file being opened.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <see cref="http://msdn.microsoft.com/en-us/library/system.windows.forms.openfiledialog.openfile(VS.71).aspx"/>
        public static FileStream OpenFileWithAccess(this OpenFileDialog source)
        {
            source.ShowReadOnly = true;

            if (source.ShowDialog() == DialogResult.OK)
            {
                if (source.ReadOnlyChecked)
                {
                    return (FileStream)source.OpenFile();
                }
                else
                {
                    String path = source.FileName;
                    return new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                }
            }

            return null;
        }

    }
}
