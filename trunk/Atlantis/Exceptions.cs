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

namespace Atlantis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FrameworkUninitializedException : Exception
    {
        #region Constructor(s)

        public FrameworkUninitializedException()
            : this(null)
        {
        }

        public FrameworkUninitializedException(Exception innerException)
            : base("The Framework has not been initialized. Make sure you have called Framework.Initialize() before trying to use the Framework class.", innerException)
        {
        }

        #endregion
    }

    public class InvalidFrameworkAttributeException : Exception
    {
        #region Constructor(s)

        public InvalidFrameworkAttributeException()
            : this(null)
        {
        }

        public InvalidFrameworkAttributeException(Exception innerException)
            : base("Atlantis cannot find the ApplicationAttribute for to configure itself. Please check the documentation.", innerException)
        {

        }

        #endregion
    }

    public class InvalidFrameworkUsage : Exception
    {
        #region Constructor(s)

        public InvalidFrameworkUsage()
            : this(null)
        {
        }

        public InvalidFrameworkUsage(Exception innerException)
            : base("Atlantis is running in an unknown application mode. Please check the documentation.", innerException)
        {
        }

        #endregion
    }
}
