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
    using Atlantis.Enterprise.Xcf.Collections;
    using Atlantis.Linq;

    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class XcfSection
    {
        #region Constructor(s)

        /// <summary>
        ///     <para></para>
        /// </summary>
        public XcfSection(string name)
        {
            Name = name;

            Children = new XcfSectionList();
            Children.Owner = this;

            Keys = new XcfKeyList();
            Keys.Owner = this;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <para>Gets or sets the collection of XcfSection elements</para>
        /// </summary>
        public XcfSectionList Children { get; set; }

        /// <summary>
        ///     <para>Gets or sets the collection of XcfKey elements</para>
        /// </summary>
        public XcfKeyList Keys { get; set; }

        /// <summary>
        ///     <para>Gets or sets the name of the section</para>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     <para>Gets or sets the name of the parent</para>
        /// </summary>
        public XcfSection Parent { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     <para>Adds a key to the current section</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddKey(string name, object value)
        {
            Keys.Add(new XcfKey(name, value));
        }

        /// <summary>
        ///     <para>Adds a key to the current section</para>
        /// </summary>
        /// <param name="key"></param>
        public void AddKey(XcfKey key)
        {
            Keys.Add(key);
        }

        /// <summary>
        ///     <para>Adds a child section to the current section</para>
        /// </summary>
        /// <param name="name"></param>
        public void AddSection(string name)
        {
            Children.Add(new XcfSection(name));
        }

        /// <summary>
        ///     <para>Adds a child section to the current section</para>
        /// </summary>
        /// <param name="section"></param>
        public void AddSection(XcfSection section)
        {
            Children.Add(section);
        }

        /// <summary>
        ///     <para>Checks whether the current section has the specified key</para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsKey(string name)
        {
            return (Keys.Find(k => k.Name.EqualsIgnoreCase(name)) != null);
        }

        /// <summary>
        ///     <para>Checks whether the current section has the specified child section</para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsSection(string name)
        {
            return (Children.Find(s => s.Name.EqualsIgnoreCase(name)) != null);
        }

        /// <summary>
        ///     <para>Removes one or more keys from the current section</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="duplicates"></param>
        public void RemoveKey(string name, bool duplicates)
        {
            if (duplicates)
            {
                Keys.RemoveAll(k => k.Name.EqualsIgnoreCase(name));
            }
            else
            {
                Keys.Remove(Keys.First(k => k.Name.EqualsIgnoreCase(name)));
            }
        }

        /// <summary>
        ///     <para>Removes one or more children from the current section</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="duplicates"></param>
        public void RemoveSection(string name, bool duplicates)
        {
            if (duplicates)
            {
                Children.RemoveAll(s => s.Name.EqualsIgnoreCase(name));
            }
            else
            {
                Children.Remove(Children.First(s => s.Name.EqualsIgnoreCase(name)));
            }
        }

        #endregion
    }
}
