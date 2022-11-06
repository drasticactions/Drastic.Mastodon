// <copyright file="Application.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Application
    {
        /// <summary>
        /// Gets or sets name of the app.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets homepage URL of the app.
        /// </summary>
        [JsonPropertyName("website")]
        public string? Website { get; set; }
    }
}
