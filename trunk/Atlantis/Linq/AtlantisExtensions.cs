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

namespace Atlantis.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    public static partial class Extensions
    {
        // TODO: http://pietschsoft.com/post/2008/07/C-Enhance-Enums-using-Extension-Methods.aspx

        /// <summary>
        ///     Converts a System.Enum member into a System.String representation
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ToString(this Enum e)
        {
            var descs = (System.ComponentModel.DescriptionAttribute[])e.GetType().GetField(e.ToString())
                            .GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            return (descs.Length > 0 ? descs[0].Description : String.Empty);
        }

        /*
         * Doesn't work as intended.
        /// <summary>
        ///     Gets the current Application-specific directory path
        /// </summary>
        /// <param name="sf"></param>
        /// <returns></returns>
        public static string GetApplicationDataPath(this Environment.SpecialFolder sf)
        {
            var pName = System.Windows.Forms.Application.ProductName;
            var aPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return System.IO.Path.Combine(aPath, pName);
        }*/

        /// <summary>
        ///     <para>Returns whether the specified type is a generic</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsGeneric<T>(this T source)
        {
            return (source.GetType().IsGeneric());
        }

        /// <summary>
        ///     <para>Checks whether the current source is inside the specified list</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T source, T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }

        /// <summary>
        ///     <para>Returns a random item from the specified list</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T OneOf<T>(this Random source, params T[] list)
        {
            return list[source.Next(list.Length - 1)];
        }

        /// <summary>
        ///     <para>Logs the specified exception out to the exception logger</para>
        /// </summary>
        /// <param name="source"></param>
        public static void Log(this Exception source)
        {
            if (Framework.IsInitialized)
            {
                Framework.Exceptions.Error("An unhandled {0} was caught and logged: {1}", source.GetType().ToString(), source.StackTrace);
            }
        }
    }
}