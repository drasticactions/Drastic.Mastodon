// <copyright file="Tag.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Tag
    {
        /// <summary>
        /// Gets or sets the hashtag, not including the preceding #.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the hashtag.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 7-day stats of the hashtag.
        /// </summary>
        [JsonPropertyName("history")]
        public IEnumerable<History>? History { get; set; }
    }

    public class History
    {
        /// <summary>
        /// Gets or sets uNIX time of the beginning of the day.
        /// </summary>
        [JsonPropertyName("day")]
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets number of statuses with the hashtag during the day.
        /// </summary>
        [JsonPropertyName("uses")]
        public int Uses { get; set; }

        /// <summary>
        /// Gets or sets number of accounts that used the hashtag during the day.
        /// </summary>
        [JsonPropertyName("accounts")]
        public int Accounts { get; set; }
    }
}
