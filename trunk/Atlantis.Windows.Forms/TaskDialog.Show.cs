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

namespace Atlantis.Windows.Forms
{
    using System;
    using System.Windows.Forms;

    public partial class TaskDialog
    {
        #region Methods

        // TODO: Documentation.
        /// <summary>
        ///     <para>Shows a new TaskDialog and returns the modal result.</para>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="owner">Optional. Represents the parent window this modal dialog belongs to.</param>
        /// <returns></returns>
        public static DialogResult Show(String text, String caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information, IWin32Window owner = null)
        {
            throw new NotImplementedException();

            using (TaskDialog dialog = new TaskDialog())
            {
                if (owner != null)
                {
                    return dialog.ShowDialog(owner);
                }
                else
                {
                    return dialog.ShowDialog();
                }
            }
        }

        /// <summary>
        ///     <para>Shows a new TaskDialog and returns the modal result.</para>
        /// </summary>
        /// <param name="owner">Required. Represents the parent window this modal dialog belongs to.</param>
        /// <param name="text">Required. Primary text to display on the dialog.</param>
        /// <param name="caption">Recommended. Title of the modal dialog. Used on the dialog form itself and in the title bar.</param>
        /// <param name="buttons">Optional. Specifies what buttons to display.</param>
        /// <param name="icon">Optional. Represen</param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String text, String caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            return Show(text, caption, buttons, icon, owner);
        }

        #endregion
    }
}