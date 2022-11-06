// <copyright file="Error.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Error
    {
        /// <summary>
        /// Gets or sets a textual description of the error.
        /// </summary>
        [JsonPropertyName("error")]
        public string Description { get; set; } = string.Empty;
    }
}
