// <copyright file="DefaultHttpClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Drastic.Mastodon
{
    /// <summary>
    /// Singleton-like static class for <see cref="System.Net.Http.HttpClient"/>.
    /// Used as default in constructor parameter of <see cref="Drastic.Mastodon.BaseHttpClient"/> and its subclasses.
    /// </summary>
    internal static class DefaultHttpClient
    {
        /// <summary>
        /// Gets the only <see cref="System.Net.Http.HttpClient"/> instance.
        /// </summary>
        internal static HttpClient Instance { get; } = new HttpClient();
    }
}
