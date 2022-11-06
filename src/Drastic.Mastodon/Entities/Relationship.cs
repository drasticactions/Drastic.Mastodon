// <copyright file="Relationship.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Relationship
    {
        /// <summary>
        /// Gets or sets target account id.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user is currently following the account.
        /// </summary>
        [JsonPropertyName("following")]
        public bool Following { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user is currently being followed by the account.
        /// </summary>
        [JsonPropertyName("followed_by")]
        public bool FollowedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user is currently blocking the account.
        /// </summary>
        [JsonPropertyName("blocking")]
        public bool Blocking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user is currently muting the account.
        /// </summary>
        [JsonPropertyName("muting")]
        public bool Muting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user is also muting notifications.
        /// </summary>
        [JsonPropertyName("muting_notifications")]
        public bool MutingNotifications { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user has requested to follow the account.
        /// </summary>
        [JsonPropertyName("requested")]
        public bool Requested { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user is currently blocking the accounts's domain.
        /// </summary>
        [JsonPropertyName("domain_blocking")]
        public bool DomainBlocking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user's reblogs will show up in the home timeline.
        /// </summary>
        [JsonPropertyName("showing_reblogs")]
        public bool ShowingReblogs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the user is currently endorsing the account.
        /// </summary>
        [JsonPropertyName("endorsed")]
        public bool Endorsed { get; set; }
    }
}
