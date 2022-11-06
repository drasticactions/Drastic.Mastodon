// <copyright file="Status.cs" company="Drastic Actions">
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
    public class Status
    {
        /// <summary>
        /// Gets or sets the ID of the status.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets a Fediverse-unique resource ID.
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL to the status page (can be remote).
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the Account which posted the status.
        /// </summary>
        [JsonPropertyName("account")]
        public Account Account { get; set; } = new Account();

        /// <summary>
        /// Gets or sets null or the ID of the status it replies to.
        /// </summary>
        [JsonPropertyName("in_reply_to_id")]
        public long? InReplyToId { get; set; }

        /// <summary>
        /// Gets or sets null or the ID of the account it replies to.
        /// </summary>
        [JsonPropertyName("in_reply_to_account_id")]
        public long? InReplyToAccountId { get; set; }

        /// <summary>
        /// Gets or sets null or the reblogged Status.
        /// </summary>
        [JsonPropertyName("reblog")]
        public Status? Reblog { get; set; }

        /// <summary>
        /// Gets or sets body of the status; this will contain HTML (remote HTML already sanitized).
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time the status was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets an array of Emojis.
        /// </summary>
        [JsonPropertyName("emojis")]
        public IEnumerable<Emoji> Emojis { get; set; } = Enumerable.Empty<Emoji>();

        /// <summary>
        /// Gets or sets the number of replies for the status.
        /// </summary>
        [JsonPropertyName("replies_count")]
        public int RepliesCount { get; set; }

        /// <summary>
        /// Gets or sets the number of reblogs for the status.
        /// </summary>
        [JsonPropertyName("reblogs_count")]
        public int ReblogCount { get; set; }

        /// <summary>
        /// Gets or sets the number of favourites for the status.
        /// </summary>
        [JsonPropertyName("favourites_count")]
        public int FavouritesCount { get; set; }

        /// <summary>
        /// Gets or sets whether the authenticated user has reblogged the status.
        /// </summary>
        [JsonPropertyName("reblogged")]
        public bool? Reblogged { get; set; }

        /// <summary>
        /// Gets or sets whether the authenticated user has favourited the status.
        /// </summary>
        [JsonPropertyName("favourited")]
        public bool? Favourited { get; set; }

        /// <summary>
        /// Gets or sets whether the authenticated user has muted the conversation this status from.
        /// </summary>
        [JsonPropertyName("muted")]
        public bool? Muted { get; set; }

        /// <summary>
        /// Gets or sets whether media attachments should be hidden by default.
        /// </summary>
        [JsonPropertyName("sensitive")]
        public bool? Sensitive { get; set; }

        /// <summary>
        /// Gets or sets if not empty, warning text that should be displayed before the actual content.
        /// </summary>
        [JsonPropertyName("spoiler_text")]
        public string SpoilerText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets one of: public, unlisted, private, direct.
        /// </summary>
        [JsonPropertyName("visibility")]
        public Visibility Visibility { get; set; }

        /// <summary>
        /// Gets or sets an array of Attachments.
        /// </summary>
        [JsonPropertyName("media_attachments")]
        public IEnumerable<Attachment> MediaAttachments { get; set; } = Enumerable.Empty<Attachment>();

        /// <summary>
        /// Gets or sets an array of Mentions.
        /// </summary>
        [JsonPropertyName("mentions")]
        public IEnumerable<Mention> Mentions { get; set; } = Enumerable.Empty<Mention>();

        /// <summary>
        /// Gets or sets an array of Tags.
        /// </summary>
        [JsonPropertyName("tags")]
        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();

        /// <summary>
        /// Gets or sets attached card, if any.
        /// </summary>
        [JsonPropertyName("card")]
        public Card? Card { get; set; }

        /// <summary>
        /// Gets or sets attached poll, if any.
        /// </summary>
        [JsonPropertyName("poll")]
        public Poll? Poll { get; set; }

        /// <summary>
        /// Gets or sets application from which the status was posted.
        /// </summary>
        [JsonPropertyName("application")]
        public Application Application { get; set; } = new Application();

        /// <summary>
        /// Gets or sets the detected language for the status, if detected.
        /// </summary>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets whether the status is pinned.
        /// </summary>
        [JsonPropertyName("pinned")]
        public bool? Pinned { get; set; }
    }
}
