// <copyright file="IBaseHttpClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public interface IBaseHttpClient
    {
        string Instance { get; }

        AppRegistration? AppRegistration { get; }

        Auth? AuthToken { get; }
    }
}
