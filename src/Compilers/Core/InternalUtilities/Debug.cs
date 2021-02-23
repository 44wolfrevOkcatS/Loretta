﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Loretta.Utilities
{
    internal static class RoslynDebug
    {
        /// <inheritdoc cref="RoslynDebug.Assert(bool)"/>
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool b) => RoslynDebug.Assert(b);

        /// <inheritdoc cref="RoslynDebug.Assert(bool, string)"/>
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool b, string message)
            => RoslynDebug.Assert(b, message);

        [Conditional("DEBUG")]
        public static void AssertNotNull<T>([NotNull] T value)
        {
            Assert(value is object, "Unexpected null reference");
        }
    }
}