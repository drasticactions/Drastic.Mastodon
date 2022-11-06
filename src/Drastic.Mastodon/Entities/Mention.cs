// <copyright file="Mention.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Mention
    {
        /// <summary>
        /// Gets or sets account ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets uRL of user's profile (can be remote).
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the account.
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets equals username for local users, includes @domain for remote ones.
        /// </summary>
        [JsonPropertyName("acct")]
        public string AccountName { get; set; } = string.Empty;
    }
}
