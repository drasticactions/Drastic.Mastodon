// <copyright file="Account.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drastic.Mastodon.Entities
{
    public class Account
    {
        /// <summary>
        /// Gets or sets the ID of the account.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the account.
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets equals username for local users, includes @domain for remote ones.
        /// </summary>
        [JsonPropertyName("acct")]
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account's display name.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether boolean for when the account cannot be followed without waiting for approval first.
        /// </summary>
        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the time the account was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the number of followers for the account.
        /// </summary>
        [JsonPropertyName("followers_count")]
        public int FollowersCount { get; set; }

        /// <summary>
        /// Gets or sets the number of accounts the given account is following.
        /// </summary>
        [JsonPropertyName("following_count")]
        public int FollowingCount { get; set; }

        /// <summary>
        /// Gets or sets the number of statuses the account has made.
        /// </summary>
        [JsonPropertyName("statuses_count")]
        public int StatusesCount { get; set; }

        /// <summary>
        /// Gets or sets biography of user.
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL of the user's profile page (can be remote).
        /// </summary>
        [JsonPropertyName("url")]
        public string ProfileUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL to the avatar image.
        /// </summary>
        [JsonPropertyName("avatar")]
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL to the avatar static image (gif).
        /// </summary>
        [JsonPropertyName("avatar_static")]
        public string StaticAvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL to the header image.
        /// </summary>
        [JsonPropertyName("header")]
        public string HeaderUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets uRL to the header image.
        /// </summary>
        [JsonPropertyName("header_static")]
        public string StaticHeaderUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets emojis used in the account info.
        /// </summary>
        [JsonPropertyName("emojis")]
        public IEnumerable<Emoji> Emojis { get; set; } = Enumerable.Empty<Emoji>();

        /// <summary>
        /// Gets or sets if moved, the new account for the account.
        /// </summary>
        [JsonPropertyName("moved")]
        public Account? Moved { get; set; }

        /// <summary>
        /// Gets or sets the custom fields of the account.
        /// </summary>
        [JsonPropertyName("fields")]
        public IEnumerable<AccountField>? Fields { get; set; }

        /// <summary>
        /// Gets or sets whether the account is a bot.
        /// </summary>
        [JsonPropertyName("bot")]
        public bool? Bot { get; set; }

        /// <summary>
        /// Gets or sets source of the authorized account's profile (returned only from *_credentials endpoints).
        /// </summary>
        [JsonPropertyName("source")]
        public AccountSource? Source { get; set; }
    }

    public class AccountField
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value of the field (HTML).
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the datetime when the account is verified if the field value is a link.
        /// </summary>
        [JsonPropertyName("verified_at")]
        public DateTime? VerifiedAt { get; set; }
    }

    public class AccountSource
    {
        /// <summary>
        /// Gets or sets the default visibility for the account.
        /// </summary>
        [JsonPropertyName("privacy")]
        public Visibility? Privacy { get; set; }

        /// <summary>
        /// Gets or sets the default media sensitiveness setting for the account.
        /// </summary>
        [JsonPropertyName("sensitive")]
        public bool? Sensitive { get; set; }

        /// <summary>
        /// Gets or sets the language setting for the account (ISO6391).
        /// </summary>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets biography of the user (in plain text).
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the custom fields of the account (in plain text).
        /// </summary>
        [JsonPropertyName("fields")]
        public IEnumerable<AccountField> Fields { get; set; } = Enumerable.Empty<AccountField>();
    }
}
