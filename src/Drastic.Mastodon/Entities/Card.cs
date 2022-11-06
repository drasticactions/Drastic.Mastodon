// <copyright file="Card.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Card
    {
        /// <summary>
        /// Gets or sets the url associated with the card.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the card.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the card description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image associated with the card, if any.
        /// </summary>
        [JsonPropertyName("image")]
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets "link", "photo", "video", or "rich".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets oEmbed data.
        /// </summary>
        [JsonPropertyName("author_name")]
        public string? AuthorName { get; set; }

        /// <summary>
        /// Gets or sets oEmbed data.
        /// </summary>
        [JsonPropertyName("author_url")]
        public string? AuthorUrl { get; set; }

        /// <summary>
        /// Gets or sets oEmbed data.
        /// </summary>
        [JsonPropertyName("provider_name")]
        public string? ProviderName { get; set; }

        /// <summary>
        /// Gets or sets oEmbed data.
        /// </summary>
        [JsonPropertyName("provider_url")]
        public string? ProviderUrl { get; set; }

        /// <summary>
        /// Gets or sets oEmbed data.
        /// </summary>
        [JsonPropertyName("html")]
        public string? Html { get; set; }

        /// <summary>
        /// Gets or sets oEmbed data.
        /// </summary>
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets oEmbed data.
        /// </summary>
        [JsonPropertyName("height")]
        public int? Height { get; set; }
    }
}
