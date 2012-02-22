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

    public partial class XcfBuilder
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Initializes a new instance of the Atlantis.Enterprise.Xcf.XcfBuilder</para>
        /// </summary>
        public XcfBuilder()
            : this(Encoding.UTF8, 0.5F)
        {
        }

        /// <summary>
        ///     <para>Initializes a new instance of the Atlantis.Enterprise.Xcf.XcfBuilder</para>
        /// </summary>
        private XcfBuilder(Encoding encoding, double version = 0.5)
        {
            m_Buffer = new StringBuilder();
            m_CurrentDepthLevel = 0;
            m_UnclosedSections = new Stack<string>();

            Encoding = encoding;
            Version = version;
        }

        #endregion

        #region Constants

        private const int INDENT_DEPTH = 4;
        private const double DefaultVersion = 0.5;

        #endregion

        #region Fields

        private StringBuilder m_Buffer;
        private int m_CurrentDepthLevel;
        private Stack<string> m_UnclosedSections;

        #endregion

        #region Properties

        private Encoding m_Encoding = Encoding.UTF8;
        /// <summary>
        ///     <para>Gets or sets a parameter indicating the encoding format of this XcfBuilder</para>
        /// </summary>
        public Encoding Encoding
        {
            get { return m_Encoding; }
            set { m_Encoding = value; }
        }


        private double m_Version = DefaultVersion;
        /// <summary>
        ///     <para>Gets the version of Xcf being built.</para>
        /// </summary>
        public double Version
        {
            get { return m_Version; }
            internal set { m_Version = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Converts the current XcfBuilder to a System.String</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_Buffer.ToString();
        }

        /// <summary>
        ///     <para>Writes the specified <see cref="XcfKey" /> to the buffer</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteKey(string name, object value)
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
