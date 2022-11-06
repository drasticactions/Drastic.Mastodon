﻿// <copyright file="MastodonClient.Account.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public partial class MastodonClient
    {
        /// <summary>
        /// Fetching an account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns an Account.</returns>
        public Task<Account> GetAccount(long accountId)
        {
            return this.Get<Account>($"/api/v1/accounts/{accountId}");
        }

        /// <summary>
        /// Getting the current user.
        /// </summary>
        /// <returns>Returns the authenticated user's Account.</returns>
        public Task<Account> GetCurrentUser()
        {
            return this.Get<Account>($"/api/v1/accounts/verify_credentials");
        }

        /// <summary>
        /// Updating the current user.
        /// </summary>
        /// <param name="display_name">The name to display in the user's profile.</param>
        /// <param name="note">A new biography for the user.</param>
        /// <param name="avatar">A base64 encoded image to display as the user's avatar.</param>
        /// <param name="header">A base64 encoded image to display as the user's header image.</param>
        /// <param name="locked">Whether to enable follow requests.</param>
        /// <param name="source_privacy">Default post privacy preference.</param>
        /// <param name="source_sensitive">Whether to mark statuses as sensitive by default.</param>
        /// <param name="source_language">Override language on statuses by default (ISO6391).</param>
        /// <param name="fields_attributes">Profile metadata (max. 4).</param>
        /// <returns>Returns the authenticated user's Account.</returns>
        public Task<Account> UpdateCredentials(
            string? display_name = null,
            string? note = null,
            MediaDefinition? avatar = null,
            MediaDefinition? header = null,
            bool? locked = null,
            Visibility? source_privacy = null,
            bool? source_sensitive = null,
            string? source_language = null,
            IEnumerable<AccountField>? fields_attributes = null)
        {
            if (fields_attributes?.Count() > 4)
            {
                throw new ArgumentException("Number of fields must be 4 or fewer.", nameof(fields_attributes));
            }

            var data = new List<KeyValuePair<string, string>>();
            var media = new List<MediaDefinition>();

            if (display_name != null)
            {
                data.Add(new KeyValuePair<string, string>("display_name", display_name));
            }

            if (note != null)
            {
                data.Add(new KeyValuePair<string, string>("note", note));
            }

            if (avatar != null)
            {
                avatar.ParamName = "avatar";
                media.Add(avatar);
            }

            if (header != null)
            {
                header.ParamName = "header";
                media.Add(header);
            }

            if (locked.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("locked", locked.Value.ToString().ToLowerInvariant()));
            }

            if (source_privacy.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("source[privacy]", source_privacy.Value.ToString().ToLowerInvariant()));
            }

            if (source_sensitive.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("source[sensitive]", source_sensitive.Value.ToString().ToLowerInvariant()));
            }

            if (source_language != null)
            {
                data.Add(new KeyValuePair<string, string>("source[language]", source_language));
            }

            if (fields_attributes != null)
            {
                foreach (var item in fields_attributes.Select((f, i) => new { f, i }))
                {
                    data.Add(new KeyValuePair<string, string>($"fields_attributes[{item.i}][name]", item.f.Name));
                    data.Add(new KeyValuePair<string, string>($"fields_attributes[{item.i}][value]", item.f.Value));
                }
            }

            return this.Patch<Account>($"/api/v1/accounts/update_credentials", data, media);
        }

        /// <summary>
        /// Getting an account's relationships.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <returns>Returns an array of Relationships of the current user to a given account.</returns>
        public Task<IEnumerable<Relationship>> GetAccountRelationships(long id)
        {
            return this.GetAccountRelationships(new long[] { id });
        }

        /// <summary>
        /// Getting an account's relationships.
        /// </summary>
        /// <param name="id">Account IDs.</param>
        /// <returns>Returns an array of Relationships of the current user to a list of given accounts.</returns>
        public Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<long> ids)
        {
            var data = new List<KeyValuePair<string, string>>();
            foreach (var id in ids)
            {
                data.Add(new KeyValuePair<string, string>("id[]", id.ToString()));
            }

            return this.Get<IEnumerable<Relationship>>("/api/v1/accounts/relationships", data);
        }

        /// <summary>
        /// Getting an account's followers.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetAccountFollowers(long accountId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetAccountFollowers(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Getting an account's followers.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetAccountFollowers(long accountId, ArrayOptions options)
        {
            var url = $"/api/v1/accounts/{accountId}/followers";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Getting who account is following.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetAccountFollowing(long accountId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetAccountFollowing(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Getting who account is following.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetAccountFollowing(long accountId, ArrayOptions options)
        {
            var url = $"/api/v1/accounts/{accountId}/following";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Getting an account's statuses.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments.</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses.</param>
        /// <param name="pinned">Only return statuses that have been pinned.</param>
        /// <param name="excludeReblogs">Skip statuses that are reblogs of other statuses.</param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns an array of Statuses.</returns>
        public Task<MastodonList<Status>> GetAccountStatuses(long accountId, long? maxId = null, long? sinceId = null, int? limit = null, bool onlyMedia = false, bool excludeReplies = false, bool pinned = false, bool excludeReblogs = false)
        {
            return this.GetAccountStatuses(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, onlyMedia, pinned, excludeReplies, excludeReblogs);
        }

        /// <summary>
        /// Getting an account's statuses.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments.</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses.</param>
        /// <param name="pinned">Only return statuses that have been pinned.</param>
        /// <param name="excludeReblogs">Skip statuses that are reblogs of other statuses.</param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Statuses.</returns>
        public Task<MastodonList<Status>> GetAccountStatuses(long accountId, ArrayOptions options, bool onlyMedia = false, bool excludeReplies = false, bool pinned = false, bool excludeReblogs = false)
        {
            var url = $"/api/v1/accounts/{accountId}/statuses";

            string queryParams = string.Empty;
            if (onlyMedia)
            {
                queryParams = "?only_media=true";
            }

            if (pinned)
            {
                if (queryParams != string.Empty)
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }

                queryParams += "pinned=true";
            }

            if (excludeReplies)
            {
                if (queryParams != string.Empty)
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }

                queryParams += "exclude_replies=true";
            }

            if (excludeReblogs)
            {
                if (queryParams != string.Empty)
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }

                queryParams += "exclude_reblogs=true";
            }

            if (options != null)
            {
                if (queryParams != string.Empty)
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }

                queryParams += options.ToQueryString();
            }

            return this.GetMastodonList<Status>(url + queryParams);
        }

        /// <summary>
        /// Fetching a list of follow requests.
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user.</returns>
        public Task<MastodonList<Account>> GetFollowRequests(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetFollowRequests(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a list of follow requests.
        /// </summary>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user.</returns>
        public Task<MastodonList<Account>> GetFollowRequests(ArrayOptions options)
        {
            var url = "/api/v1/follow_requests";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Authorizing follow requests.
        /// </summary>
        /// <param name="accountId">The id of the account to authorize.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AuthorizeRequest(long accountId)
        {
            return this.Post($"/api/v1/follow_requests/{accountId}/authorize");
        }

        /// <summary>
        /// Rejecting follow requests.
        /// </summary>
        /// <param name="accountId">The id of the account to reject.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task RejectRequest(long accountId)
        {
            return this.Post($"/api/v1/follow_requests/{accountId}/reject");
        }

        /// <summary>
        /// Listing accounts the user had past positive interactions with, but is not following yet.
        /// </summary>
        /// <returns>Returns array of Account.</returns>
        public Task<IEnumerable<Account>> GetFollowSuggestions()
        {
            return this.Get<IEnumerable<Account>>("/api/v1/suggestions");
        }

        /// <summary>
        /// Removing account from suggestions.
        /// </summary>
        /// <param name="accountId">The account ID to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteFollowSuggestion(long accountId)
        {
            return this.Delete($"/api/v1/suggestions/{accountId}");
        }

        /// <summary>
        /// Fetching a user's favourites.
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user.</returns>
        public Task<MastodonList<Status>> GetFavourites(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetFavourites(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's favourites.
        /// </summary>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user.</returns>
        public Task<MastodonList<Status>> GetFavourites(ArrayOptions options)
        {
            var url = "/api/v1/favourites";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Status>(url);
        }
    }
}
