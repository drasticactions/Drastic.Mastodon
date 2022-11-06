// <copyright file="Filter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Filter
    {
        /// <summary>
        /// Gets or sets iD of the filter.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets keyword or phrase to filter.
        /// </summary>
        [JsonPropertyName("phrase")]
        public string Phrase { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets contexts to apply the filter.
        /// </summary>
        [JsonPropertyName("context")]
        public FilterContext Context { get; set; }

        /// <summary>
        /// Gets or sets dateTime when the filter expires if set.
        /// </summary>
        [JsonPropertyName("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the filter is irreversible.
        /// </summary>
        [JsonPropertyName("irreversible")]
        public bool Irreversible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether to consider word boundaries when matching.
        /// </summary>
        [JsonPropertyName("whole_word")]
        public bool WholeWord { get; set; }
    }
}
