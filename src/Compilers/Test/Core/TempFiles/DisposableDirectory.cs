﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.IO;

namespace Loretta.CodeAnalysis.Test.Utilities
{
    public sealed class DisposableDirectory : TempDirectory, IDisposable
    {
        public DisposableDirectory(TempRoot root)
            : base(root)
        {
        }

        public void Dispose()
        {
            if (Path != null && Directory.Exists(Path))
            {
                try
                {
                    Directory.Delete(Path, recursive: true);
                }
                catch
                {
                }
            }
        }
    }
}
