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
    using Atlantis.IO;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class XcfDocument : DataFileBase
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        public XcfDocument()
            : this(String.Empty)
        {
        }

        public XcfDocument(string filepath)
            : base(filepath)
        {
            m_Reader = new XcfReader(Name, Encoding);
        }

        #endregion

        #region Constants

        private const double DefaultVersion = 0.5;

        #endregion

        #region Fields

        private XcfReader m_Reader;

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

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public void Load(string filepath)
        {
            if (String.IsNullOrEmpty(Name))
            {
                Initialize(filepath);
            }

            Load();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        // TODO: WriteDocumentStart to write the start of the document (encoding + version)

        #endregion
    }
}
