// <copyright file="Notification.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Notification
    {
        /// <summary>
        /// Gets or sets the notification ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets one of: "mention", "reblog", "favourite", "follow".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time the notification was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the Account sending the notification to the user.
        /// </summary>
        [JsonPropertyName("account")]
        public Account Account { get; set; } = new Account();

        /// <summary>
        /// Gets or sets the Status associated with the notification, if applicible.
        /// </summary>
        [JsonPropertyName("status")]
        public Status? Status { get; set; }
    }
}
