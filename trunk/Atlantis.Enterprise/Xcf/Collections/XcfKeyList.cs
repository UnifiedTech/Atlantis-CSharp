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

namespace Atlantis.Enterprise.Xcf.Collections
{
    using Atlantis.Linq;

    using System;
    using System.Collections.Generic;

    public class XcfKeyList : List<XcfKey>
    {
        #region Constructor(s)

        /// <summary>
        ///     <para>Creates a new list of XcfKeyList elements</para>
        /// </summary>
        public XcfKeyList()
            : base()
        {
        }

        /// <summary>
        ///     <para>Creates a new list of XcfKeyList elements</para>
        /// </summary>
        /// <param name="capacity"></param>
        public XcfKeyList(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        ///     <para>Creates a new list of XcfKeyList elements</para>
        /// </summary>
        /// <param name="collection"></param>
        public XcfKeyList(IEnumerable<XcfKey> collection)
            : base(collection)
        {
        }

        #endregion

        #region Constants
        // Put all your constant declarations here
        #endregion

        #region Fields
        // Put your private/protected fields here
        #endregion

        #region Properties

        /// <summary>
        ///     <para></para>
        /// </summary>
        public new int Count
        {
            get { return base.Count; }
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XcfKey this[String name]
        {
            get { return this.Find(s => s.Name.EqualsIgnoreCase(name)); }
            set
            {
                this[this.IndexOf(this.Find(s => s.Name.EqualsIgnoreCase(name)))] = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the owner of this current key list</para>
        /// </summary>
        public XcfSection Owner { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="item"></param>
        public new void Add(XcfKey item)
        {
            item.Owner = Owner;
            base.Add(item);
        }

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="collection"></param>
        public new void AddRange(IEnumerable<XcfKey> collection)
        {
            foreach (var item in collection)
            {
                item.Owner = Owner;
            }

            base.AddRange(collection);
        }

        #endregion
    }
}
