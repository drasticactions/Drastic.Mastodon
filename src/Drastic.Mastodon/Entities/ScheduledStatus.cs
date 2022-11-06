// <copyright file="ScheduledStatus.cs" company="Drastic Actions">
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
    public class ScheduledStatus
    {
        /// <summary>
        /// Gets or sets the scheduled status's ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets dateTime to publish the scheduled status.
        /// </summary>
        [JsonPropertyName("scheduled_at")]
        public DateTime ScheduledAt { get; set; }

        /// <summary>
        /// Gets or sets parameters of the scheduled status.
        /// </summary>
        [JsonPropertyName("params")]
        public StatusParams Params { get; set; } = new StatusParams();

        /// <summary>
        /// Gets or sets media attached to the scheduled status.
        /// </summary>
        [JsonPropertyName("media_attachments")]
        public IEnumerable<Attachment> MediaAttachments { get; set; } = Enumerable.Empty<Attachment>();
    }

    public class StatusParams
    {
        /// <summary>
        /// Gets or sets content of the status in plain text.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets null or the ID of the status it replies to.
        /// </summary>
        [JsonPropertyName("in_reply_to_id")]
        public string? InReplyToId { get; set; }

        /// <summary>
        /// Gets or sets iDs of the attachments.
        /// </summary>
        [JsonPropertyName("media_ids")]
        public IEnumerable<long>? MediaIds { get; set; }

        /// <summary>
        /// Gets or sets whether to mark the attachment as sensitive, or null.
        /// </summary>
        [JsonPropertyName("sensitive")]
        public bool? Sensitive { get; set; }

        /// <summary>
        /// Gets or sets spoiler text if any.
        /// </summary>
        [JsonPropertyName("spoiler_text")]
        public string? SpoilerText { get; set; }

        /// <summary>
        /// Gets or sets visibility of the scheduled status.
        /// </summary>
        [JsonPropertyName("visibility")]
        public Visibility Visibility { get; set; }

        /// <summary>
        /// Gets or sets dateTime to publish the scheduled status.
        /// </summary>
        [JsonPropertyName("scheduled_at")]
        public DateTime? ScheduledAt { get; set; }

        /// <summary>
        /// Gets or sets application ID that created the scheduled status.
        /// </summary>
        [JsonPropertyName("application_id")]
        public long ApplicationId { get; set; }
    }
}
