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

namespace Atlantis.Enterprise.Linq
{
    using System;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Globalization;

    public static class StringExtensions
    {
        #region Methods

        /// <summary>
        ///     <para>Creates a pluralized version of a single word based on the number value.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>Uses <see cref="System.Data.Entity.Design.PluralizationServices" /> from .NET Entity Framework</para>
        /// </devdoc>
        public static String Pluralize(this String source, Int32 number, CultureInfo culture = null)
        {
            if (number > 1 && number != 0)
            {
                culture = (culture == null ? CultureInfo.CurrentCulture : culture);

                var service = PluralizationService.CreateService(culture);

                // TODO: Pluralize entire sentences
                return (service.IsPlural(source) ? source : service.Pluralize(source));
            }

            return source;
        }

        #endregion
    }
}