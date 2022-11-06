// <copyright file="Attachment.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Attachment
    {
        /// <summary>
        /// Gets or sets iD of the attachment.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets one of: "image", "video", "gifv", "unknown".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL of the locally hosted version of the image.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets for remote images, the remote URL of the original image.
        /// </summary>
        [JsonPropertyName("remote_url")]
        public string? RemoteUrl { get; set; }

        /// <summary>
        /// Gets or sets uRL of the preview image.
        /// </summary>
        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets shorter URL for the image, for insertion into text (only present on local images).
        /// </summary>
        [JsonPropertyName("text_url")]
        public string? TextUrl { get; set; }

        /// <summary>
        /// Gets or sets metadata of the attachment.
        /// </summary>
        [JsonPropertyName("meta")]
        public AttachmentMeta? Meta { get; set; }

        /// <summary>
        /// Gets or sets description of the attachment.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    public class AttachmentMeta
    {
        [JsonPropertyName("original")]
        public AttachmentSizeData? Original { get; set; }

        [JsonPropertyName("small")]
        public AttachmentSizeData? Small { get; set; }

        [JsonPropertyName("focus")]
        public AttachmentFocusData? Focus { get; set; }
    }

    public class AttachmentSizeData
    {
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("size")]
        public string? Size { get; set; }

        [JsonPropertyName("aspect")]
        public double? Aspect { get; set; }

        [JsonPropertyName("frame_rate")]
        public string? FrameRate { get; set; }

        [JsonPropertyName("duration")]
        public double? Duration { get; set; }

        [JsonPropertyName("bitrate")]
        public int? BitRate { get; set; }
    }

    public class AttachmentFocusData
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }
    }
}
