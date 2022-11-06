// <copyright file="Scope.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Drastic.Mastodon
{
    [Flags]
    public enum Scope
    {
        Read = 1,
        Write = 2,
        Follow = 4,
    }
}
