// <copyright file="Results.cs" company="Drastic Actions">
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
    public class Results
    {
        /// <summary>
        /// Gets or sets an array of matched Accounts.
        /// </summary>
        [JsonPropertyName("accounts")]
        public IEnumerable<Account> Accounts { get; set; } = Enumerable.Empty<Account>();

        /// <summary>
        /// Gets or sets an array of matched Statuses.
        /// </summary>
        [JsonPropertyName("statuses")]
        public IEnumerable<Status> Statuses { get; set; } = Enumerable.Empty<Status>();

        /// <summary>
        /// Gets or sets an array of matched hashtags, as strings.
        /// </summary>
        [JsonPropertyName("hashtags")]
        public IEnumerable<string> Hashtags { get; set; } = Enumerable.Empty<string>();
    }

    public class ResultsV2
    {
        /// <summary>
        /// Gets or sets an array of matched Accounts.
        /// </summary>
        [JsonPropertyName("accounts")]
        public IEnumerable<Account> Accounts { get; set; } = Enumerable.Empty<Account>();

        /// <summary>
        /// Gets or sets an array of matched Statuses.
        /// </summary>
        [JsonPropertyName("statuses")]
        public IEnumerable<Status> Statuses { get; set; } = Enumerable.Empty<Status>();

        /// <summary>
        /// Gets or sets an array of matched hashtags, as Tag instances.
        /// </summary>
        [JsonPropertyName("hashtags")]
        public IEnumerable<Tag> Hashtags { get; set; } = Enumerable.Empty<Tag>();
    }
}
