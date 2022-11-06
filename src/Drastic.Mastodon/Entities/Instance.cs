// <copyright file="Instance.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Instance
    {
        /// <summary>
        /// Gets or sets uRI of the current instance.
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the instance's title.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a description for the instance.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an email address which can be used to contact the instance administrator.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Mastodon version of the instance.
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRI for the thumbnail of the hero image.
        /// </summary>
        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets uRLs related to the instance.
        /// </summary>
        [JsonPropertyName("urls")]
        public InstanceUrls Urls { get; set; } = new InstanceUrls();

        /// <summary>
        /// Gets or sets the instance's stats.
        /// </summary>
        [JsonPropertyName("stats")]
        public InstanceStats Stats { get; set; } = new InstanceStats();

        /// <summary>
        /// Gets or sets array that consists of the instance's default locale.
        /// </summary>
        [JsonPropertyName("languages")]
        public IEnumerable<string> Languages { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Gets or sets the instance's admin account.
        /// </summary>
        [JsonPropertyName("contact_account")]
        public Account? ContactAccount { get; set; }
    }

    public class InstanceUrls
    {
        /// <summary>
        /// Gets or sets websocket base URL for streaming API.
        /// </summary>
        [JsonPropertyName("streaming_api")]
        public string StreamingAPI { get; set; } = string.Empty;
    }

    public class InstanceStats
    {
        /// <summary>
        /// Gets or sets number of users that belongs to the instance.
        /// </summary>
        [JsonPropertyName("user_count")]
        public int UserCount { get; set; }

        /// <summary>
        /// Gets or sets number of statuses that belongs to the instance.
        /// </summary>
        [JsonPropertyName("status_count")]
        public int StatusCount { get; set; }

        /// <summary>
        /// Gets or sets number of remote instances known to the instance.
        /// </summary>
        [JsonPropertyName("domain_count")]
        public int DomainCount { get; set; }
    }
}
