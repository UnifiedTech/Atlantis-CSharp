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

namespace Atlantis.Enterprise.Xcf
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class XcfBuilder
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Initializes a new instance of the Atlantis.Enterprise.Xcf.XcfBuilder</para>
        /// </summary>
        public XcfBuilder()
        {
            m_Buffer = new StringBuilder();
            m_CurrentDepthLevel = 0;
            m_UnclosedSections = new Stack<String>();
        }

        #endregion

        #region Constants

        private const int INDENT_DEPTH = 4;

        #endregion

        #region Fields

        private StringBuilder m_Buffer;
        private int m_CurrentDepthLevel;
        private Stack<String> m_UnclosedSections;

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Converts the current XcfBuilder to a System.String</para>
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return m_Buffer.ToString();
        }

        /// <summary>
        ///     <para>Writes the specified <see cref="XcfKey" /> to the buffer</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteKey(String name, object value)
        {
            int pos = 0;
            for (;  pos != (m_CurrentDepthLevel * XcfBuilder.INDENT_DEPTH); ++pos)
                m_Buffer.Append(' ');

            m_Buffer.AppendLine(String.Format("{0} = {1}", name, value));
        }

        /// <summary>
        ///     <para>Writes the specified <see cref="XcfKey" /> to the buffer</para>
        /// </summary>
        /// <param name="key"></param>
        public void WriteKey(XcfKey key)
        {
            WriteKey(key.Name, key.Value);
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="name"></param>
        public void WriteOpenSection(String name)
        {
            int pos = 0;
            for (; pos != (m_CurrentDepthLevel * XcfBuilder.INDENT_DEPTH); ++pos)
                m_Buffer.Append(' ');

            m_Buffer.AppendLine(String.Format("{0} {{", name));
            m_UnclosedSections.Push(name);
            m_CurrentDepthLevel++;
        }

        /// <summary>
        ///     <para>Writes the closing tag for the current open section to the buffer</para>
        /// </summary>
        public void WriteCloseSection()
        {
            if (m_UnclosedSections.Count > 0)
            {
                m_CurrentDepthLevel--;
                int pos = 0;
                for (; pos != (m_CurrentDepthLevel * XcfBuilder.INDENT_DEPTH); ++pos)
                    m_Buffer.Append(' ');

                m_UnclosedSections.Pop();
                m_Buffer.AppendLine("}");
            }
        }

        /// <summary>
        ///     <para>Writes the specified <see cref="XcfSection"/> to the buffer</para>
        /// </summary>
        /// <param name="section"></param>
        public void WriteSection(XcfSection section)
        {
            WriteOpenSection(section.Name);

            foreach (XcfKey item in section.Keys)
            {
                WriteKey(item);
            }

            foreach (XcfSection item in section.Children)
            {
                WriteSection(item);
            }

            WriteCloseSection();
        }

        #endregion
    }
}
