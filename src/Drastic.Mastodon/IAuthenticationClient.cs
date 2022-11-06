// <copyright file="IAuthenticationClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public interface IAuthenticationClient : IBaseHttpClient
    {
        /// <summary>
        /// Registering an application.
        /// </summary>
        /// <param name="instance">Instance to connect.</param>
        /// <param name="appName">Name of your application.</param>
        /// <param name="scope">The rights needed by your application.</param>
        /// <param name="website">URL to the homepage of your app.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<AppRegistration> CreateApp(string appName, Scope scope, string? website = null, string? redirectUri = null);

        Task<Auth> ConnectWithPassword(string email, string password);

        Task<Auth> ConnectWithCode(string code, string? redirect_uri = null);

        string OAuthUrl(string? redirectUri = null);
    }
}
