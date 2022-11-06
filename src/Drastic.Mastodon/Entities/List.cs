// <copyright file="List.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class List
    {
        /// <summary>
        /// Gets or sets iD of the list.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets title of the list.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
    }
}
