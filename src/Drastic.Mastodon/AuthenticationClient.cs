// <copyright file="AuthenticationClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public class AuthenticationClient : BaseHttpClient, IAuthenticationClient
    {
        public AuthenticationClient(string instance)
            : this(instance, DefaultHttpClient.Instance)
        {
        }

        public AuthenticationClient(AppRegistration app)
            : this(app, DefaultHttpClient.Instance)
        {
        }

        public AuthenticationClient(string instance, HttpClient client)
            : base(client)
        {
            this.Instance = instance;
        }

        public AuthenticationClient(AppRegistration app, HttpClient client)
            : base(client)
        {
            this.Instance = app.Instance;
            this.AppRegistration = app;
        }

        /// <summary>
        /// Registering an application.
        /// </summary>
        /// <param name="instance">Instance to connect.</param>
        /// <param name="appName">Name of your application.</param>
        /// <param name="scope">The rights needed by your application.</param>
        /// <param name="website">URL to the homepage of your app.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<AppRegistration> CreateApp(string appName, Scope scope, string? website = null, string? redirectUri = null)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_name", appName),
                new KeyValuePair<string, string>("scopes", GetScopeParam(scope)),
                new KeyValuePair<string, string>("redirect_uris", redirectUri ?? "urn:ietf:wg:oauth:2.0:oob"),
            };

            if (website != null)
            {
                data.Add(new KeyValuePair<string, string>("website", website));
            }

            var appRegistration = await this.Post<AppRegistration>("/api/v1/apps", data);

            appRegistration.Instance = this.Instance;
            appRegistration.Scope = scope;
            this.AppRegistration = appRegistration;

            return appRegistration;
        }

        public async Task<Auth> ConnectWithPassword(string email, string password)
        {
            if (this.AppRegistration == null)
            {
                throw new Exception("The app must be registered before you can connect");
            }

            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", this.AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", this.AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("scope", GetScopeParam(this.AppRegistration.Scope)),
            };

            var auth = await this.Post<Auth>("/oauth/token", data);
            this.AuthToken = auth;
            return auth;
        }

        public async Task<Auth> ConnectWithCode(string code, string? redirect_uri = null)
        {
            if (this.AppRegistration == null)
            {
                throw new Exception("The app must be registered before you can connect");
            }

            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", this.AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", this.AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri ?? "urn:ietf:wg:oauth:2.0:oob"),
                new KeyValuePair<string, string>("code", code),
            };

            var auth = await this.Post<Auth>("/oauth/token", data);
            this.AuthToken = auth;
            return auth;
        }

        public string OAuthUrl(string? redirectUri = null)
        {
            if (this.AppRegistration == null)
            {
                throw new Exception("The app must be registered before you can connect");
            }

            if (redirectUri != null)
            {
                redirectUri = WebUtility.UrlEncode(WebUtility.UrlDecode(redirectUri));
            }
            else
            {
                redirectUri = "urn:ietf:wg:oauth:2.0:oob";
            }

            return $"https://{this.Instance}/oauth/authorize?response_type=code&client_id={this.AppRegistration.ClientId}&scope={GetScopeParam(this.AppRegistration.Scope).Replace(" ", "%20")}&redirect_uri={redirectUri ?? "urn:ietf:wg:oauth:2.0:oob"}";
        }

        private static string GetScopeParam(Scope scope)
        {
            var scopeParam = string.Empty;
            if ((scope & Scope.Read) == Scope.Read)
            {
                scopeParam += " read";
            }

            if ((scope & Scope.Write) == Scope.Write)
            {
                scopeParam += " write";
            }

            if ((scope & Scope.Follow) == Scope.Follow)
            {
                scopeParam += " follow";
            }

            return scopeParam.Trim();
        }
    }
}
