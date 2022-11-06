// <copyright file="Poll.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Poll
    {
        /// <summary>
        /// Gets or sets the poll ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the due DateTime.
        /// </summary>
        [JsonPropertyName("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the poll is expired.
        /// </summary>
        [JsonPropertyName("expired")]
        public bool Expired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether a vote for multiple options is accepted.
        /// </summary>
        [JsonPropertyName("multiple")]
        public bool Multiple { get; set; }

        /// <summary>
        /// Gets or sets the total number of votes.
        /// </summary>
        [JsonPropertyName("votes_count")]
        public int VotesCount { get; set; }

        /// <summary>
        /// Gets or sets the array of options.
        /// </summary>
        [JsonPropertyName("options")]
        public IEnumerable<PollOption> Options { get; set; } = Enumerable.Empty<PollOption>();

        /// <summary>
        /// Gets or sets whether the account has voted.
        /// </summary>
        [JsonPropertyName("voted")]
        public bool? Voted { get; set; }
    }

    public class PollOption
    {
        /// <summary>
        /// Gets or sets the options' title.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of votes for the option.
        /// </summary>
        [JsonPropertyName("votes_count")]
        public int? VotesCount { get; set; }
    }

    public class PollParameters
    {
        /// <summary>
        /// Gets or sets the array of options.
        /// </summary>
        public IEnumerable<string> Options { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Gets or sets the timespan until expiration.
        /// </summary>
        public TimeSpan ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets whether to accept a vote for multiple options.
        /// </summary>
        public bool? Multiple { get; set; }

        /// <summary>
        /// Gets or sets whether to hide the number of votes for each option until expiration.
        /// </summary>
        public bool? HideTotals { get; set; }
    }
}
