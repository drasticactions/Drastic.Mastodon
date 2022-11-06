// <copyright file="Context.cs" company="Drastic Actions">
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
    public class Context
    {
        /// <summary>
        /// Gets or sets the ancestors of the status in the conversation.
        /// </summary>
        [JsonPropertyName("ancestors")]
        public IEnumerable<Status> Ancestors { get; set; } = Enumerable.Empty<Status>();

        /// <summary>
        /// Gets or sets the descendants of the status in the conversation.
        /// </summary>
        [JsonPropertyName("descendants")]
        public IEnumerable<Status> Descendants { get; set; } = Enumerable.Empty<Status>();
    }
}
