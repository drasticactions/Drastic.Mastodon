// <copyright file="Emoji.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Emoji
    {
        /// <summary>
        /// Gets or sets the shortcode of the emoji.
        /// </summary>
        [JsonPropertyName("shortcode")]
        public string Shortcode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL to the emoji static image.
        /// </summary>
        [JsonPropertyName("static_url")]
        public string StaticUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL to the emoji image.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether boolean that indicates if the emoji is visible in picker.
        /// </summary>
        [JsonPropertyName("visible_in_picker")]
        public bool VisibleInPicker { get; set; }
    }
}
