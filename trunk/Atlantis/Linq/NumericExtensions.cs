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
    using System.Globalization;

    public partial class Constants
    {
        #region Constants

        /// <summary>
        ///     <para>Represents the timestamp origin from the day January 1, 1970 00:00</para>
        /// </summary>
        public static readonly DateTime TIMESTAMP_ORIGIN = new DateTime(1970, 1, 1, 0, 0, 0);

        #endregion
    }

    public static partial class Extensions
    {
        #region Methods

        /// <summary>
        ///     <para>Formats the System.Decimal to a proper currency format based on the culture specified</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="culture">Optional. The culture to render the specified decimal as currency.</param>
        /// <returns></returns>
        public static String ToCurrency(this Decimal source, CultureInfo culture = null)
        {
            culture = (culture == null ? CultureInfo.CurrentCulture : culture);

            return String.Format(culture, "{0:C}", source);
        }

        /// <summary>
        ///     <para>Converts a unix timestamp to a System.DateTime</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <devdoc>
        ///     <para>http://codeclimber.net.nz/archive/2007/07/10/convert-a-unix-timestamp-to-a-.net-datetime.aspx</para>
        /// </devdoc>
        public static DateTime ToDateTime(this double source)
        {
            return Constants.TIMESTAMP_ORIGIN.AddSeconds(source);
        }

        /// <summary>
        ///     <para>Converts a System.DateTime to a unix timestamp</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Double ToTimestamp(this DateTime source)
        {
            return Math.Floor((source - Constants.TIMESTAMP_ORIGIN).TotalSeconds);
        }

        #endregion
    }
}
