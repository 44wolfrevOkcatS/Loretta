﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Diagnostics;
using Loretta.CodeAnalysis.Text;
using Loretta.Utilities;

namespace Loretta.CodeAnalysis
{
    /// <summary>
    /// Information decoded from early well-known custom attributes applied on a property.
    /// </summary>
    internal class CommonPropertyEarlyWellKnownAttributeData : EarlyWellKnownAttributeData
    {
        #region ObsoleteAttribute
        private ObsoleteAttributeData _obsoleteAttributeData = ObsoleteAttributeData.Uninitialized;
        public ObsoleteAttributeData ObsoleteAttributeData
        {
            get
            {
                VerifySealed(expected: true);
                return _obsoleteAttributeData.IsUninitialized ? null : _obsoleteAttributeData;
            }
            set
            {
                VerifySealed(expected: false);
                RoslynDebug.Assert(value != null);
                RoslynDebug.Assert(!value.IsUninitialized);

                _obsoleteAttributeData = value;
                SetDataStored();
            }
        }
        #endregion
    }
}
