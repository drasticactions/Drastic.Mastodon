// <copyright file="Conversation.cs" company="Drastic Actions">
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
    public class Conversation
    {
        /// <summary>
        /// Gets or sets the conversation ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets accounts in the conversation.
        /// </summary>
        [JsonPropertyName("accounts")]
        public IEnumerable<Account> Accounts { get; set; } = Enumerable.Empty<Account>();

        /// <summary>
        /// Gets or sets last status of the conversation.
        /// </summary>
        [JsonPropertyName("last_status")]
        public Status? LastStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the converstation is unread.
        /// </summary>
        [JsonPropertyName("unread")]
        public bool Unread { get; set; }
    }
}
