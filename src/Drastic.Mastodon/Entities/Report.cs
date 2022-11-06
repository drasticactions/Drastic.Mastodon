// <copyright file="Report.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Report
    {
        /// <summary>
        /// Gets or sets the ID of the report.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the action taken in response to the report.
        /// </summary>
        [JsonPropertyName("action_taken")]
        public string? ActionTaken { get; set; }
    }
}
