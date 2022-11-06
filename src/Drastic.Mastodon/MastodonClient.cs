﻿// <copyright file="MastodonClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public partial class MastodonClient : BaseHttpClient, IMastodonClient
    {
        public MastodonClient(AppRegistration appRegistration, Auth accessToken)
            : this(appRegistration, accessToken, DefaultHttpClient.Instance)
        {
        }

        public MastodonClient(AppRegistration appRegistration, Auth accessToken, HttpClient client)
            : base(client)
        {
            this.Instance = appRegistration.Instance;
            this.AppRegistration = appRegistration;
            this.AuthToken = accessToken;

#if NETSTANDARD2_0
            this.instanceGetter = new Lazy<Task<Instance>>(this.GetInstance);
#endif
        }

        /// <summary>
        /// Getting instance information.
        /// </summary>
        /// <returns>Returns the current Instance. Does not require authentication.</returns>
        public Task<Instance> GetInstance()
        {
            return this.Get<Instance>("/api/v1/instance");
        }

        /// <summary>
        /// User’s lists.
        /// </summary>
        /// <returns>Returns array of List.</returns>
        public Task<IEnumerable<List>> GetLists()
        {
            return this.Get<IEnumerable<List>>("/api/v1/lists");
        }

        /// <summary>
        /// User’s lists that a given account is part of.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns array of List.</returns>
        public Task<IEnumerable<List>> GetListsContainingAccount(long accountId)
        {
            return this.Get<IEnumerable<List>>($"/api/v1/accounts/{accountId}/lists");
        }

        /// <summary>
        /// Accounts that are in a given list.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns array of Account.</returns>
        public Task<MastodonList<Account>> GetListAccounts(long listId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetListAccounts(listId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Accounts that are in a given list.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns array of Account.</returns>
        public Task<MastodonList<Account>> GetListAccounts(long listId, ArrayOptions options)
        {
            var url = $"/api/v1/lists/{listId}/accounts";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Get a list.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>Returns List.</returns>
        public Task<List> GetList(long listId)
        {
            return this.Get<List>("/api/v1/lists/" + listId);
        }

        /// <summary>
        /// Create a new list.
        /// </summary>
        /// <param name="title">The title of the list.</param>
        /// <returns>The list created.</returns>
        public Task<List> CreateList(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("The title is required", nameof(title));
            }

            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("title", title),
            };

            return this.Post<List>("/api/v1/lists", data);
        }

        /// <summary>
        /// Update a list.
        /// </summary>
        /// <param name="title">The title of the list.</param>
        /// <returns>The list updated.</returns>
        public Task<List> UpdateList(long listId, string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle))
            {
                throw new ArgumentException("The title is required", nameof(newTitle));
            }

            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("title", newTitle),
            };

            return this.Put<List>("/api/v1/lists/" + listId, data);
        }

        /// <summary>
        /// Remove a list.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteList(long listId)
        {
            return this.Delete("/api/v1/lists/" + listId);
        }

        /// <summary>
        /// Add accounts to a list.
        /// Only accounts already followed by the user can be added to a list.
        /// </summary>
        /// <param name="listId">List ID.</param>
        /// <param name="accountIds">Array of account IDs.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AddAccountsToList(long listId, IEnumerable<long> accountIds)
        {
            if (accountIds == null || !accountIds.Any())
            {
                throw new ArgumentException("Accounts are required", nameof(accountIds));
            }

            var data = accountIds.Select(id => new KeyValuePair<string, string>("account_ids[]", id.ToString()));

            return this.Post($"/api/v1/lists/{listId}/accounts", data);
        }

        /// <summary>
        /// Add accounts to a list.
        /// Only accounts already followed by the user can be added to a list.
        /// </summary>
        /// <param name="listId">List ID.</param>
        /// <param name="accounts">Array of Accounts.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AddAccountsToList(long listId, IEnumerable<Account> accounts)
        {
            return this.AddAccountsToList(listId, accounts.Select(account => account.Id));
        }

        /// <summary>
        /// Remove accounts from a list.
        /// </summary>
        /// <param name="listId">List Id.</param>
        /// <param name="accountIds">Array of Account IDs.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task RemoveAccountsFromList(long listId, IEnumerable<long> accountIds)
        {
            if (accountIds == null || !accountIds.Any())
            {
                throw new ArgumentException("Accounts are required", nameof(accountIds));
            }

            var data = accountIds.Select(id => new KeyValuePair<string, string>("account_ids[]", id.ToString()));

            return this.Delete($"/api/v1/lists/{listId}/accounts", data);
        }

        /// <summary>
        /// Remove accounts from a list.
        /// </summary>
        /// <param name="listId">List Id.</param>
        /// <param name="accountIds">Array of Accounts.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task RemoveAccountsFromList(long listId, IEnumerable<Account> accounts)
        {
            return this.RemoveAccountsFromList(listId, accounts.Select(account => account.Id));
        }

        /// <summary>
        /// Uploading a media attachment.
        /// </summary>
        /// <param name="data">Media stream to be uploaded.</param>
        /// <param name="fileName">Media file name (must contains extension ex: .png, .jpg, ...)</param>
        /// <param name="description">A plain-text description of the media for accessibility (max 420 chars).</param>
        /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see>.</param>
        /// <returns>Returns an Attachment that can be used when creating a status.</returns>
        public Task<Attachment> UploadMedia(Stream data, string fileName = "file", string? description = null, AttachmentFocusData? focus = null)
        {
            return this.UploadMedia(new MediaDefinition(data, fileName), description, focus);
        }

        /// <summary>
        /// Uploading a media attachment.
        /// </summary>
        /// <param name="media">Media to be uploaded.</param>
        /// <param name="description">A plain-text description of the media for accessibility (max 420 chars).</param>
        /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see>.</param>
        /// <returns>Returns an Attachment that can be used when creating a status.</returns>
        public Task<Attachment> UploadMedia(MediaDefinition media, string? description = null, AttachmentFocusData? focus = null)
        {
            media.ParamName = "file";
            var list = new List<MediaDefinition>() { media };
            var data = new Dictionary<string, string>();
            if (description != null)
            {
                data.Add("description", description);
            }

            if (focus != null)
            {
                data.Add("focus", $"{focus.X},{focus.Y}");
            }

            return this.Post<Attachment>("/api/v1/media", data, list);
        }

        /// <summary>
        /// Update a media attachment. Can only be done before the media is attached to a status.
        /// </summary>
        /// <param name="mediaId">Media ID.</param>
        /// <param name="description">A plain-text description of the media for accessibility (max 420 chars).</param>
        /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see>.</param>
        /// <returns>Returns an Attachment that can be used when creating a status.</returns>
        public Task<Attachment> UpdateMedia(long mediaId, string? description = null, AttachmentFocusData? focus = null)
        {
            var data = new Dictionary<string, string>();
            if (description != null)
            {
                data.Add("description", description);
            }

            if (focus != null)
            {
                data.Add("focus", $"{focus.X},{focus.Y}");
            }

            return this.Put<Attachment>("/api/v1/media/" + mediaId, data);
        }

        /// <summary>
        /// Custom emojis that are available on the server.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<IEnumerable<Emoji>> GetCustomEmojis()
        {
            return this.Get<IEnumerable<Emoji>>("/api/v1/custom_emojis");
        }

        /// <summary>
        /// Fetching a user's notifications.
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit">Maximum number of items to get (Default 40, Max 80).</param>
        /// <param name="excludeTypes">Types to exclude.</param>
        /// <returns>Returns a list of Notifications for the authenticated user.</returns>
        public Task<MastodonList<Notification>> GetNotifications(long? maxId = null, long? sinceId = null, int? limit = null, NotificationType excludeTypes = NotificationType.None)
        {
            return this.GetNotifications(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, excludeTypes);
        }

        /// <summary>
        /// Fetching a user's notifications.
        /// </summary>
        /// <param name="options">Define the first and last items to get.</param>
        /// <param name="excludeTypes">Types to exclude.</param>
        /// <returns>Returns a list of Notifications for the authenticated user.</returns>
        public Task<MastodonList<Notification>> GetNotifications(ArrayOptions options, NotificationType excludeTypes = NotificationType.None)
        {
            var url = "/api/v1/notifications";
            var queryParams = string.Empty;
            if (options != null)
            {
                queryParams += "?" + options.ToQueryString();
            }

            if ((excludeTypes & NotificationType.Follow) == NotificationType.Follow)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + "exclude_types[]=follow";
            }

            if ((excludeTypes & NotificationType.Favourite) == NotificationType.Favourite)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + "exclude_types[]=favourite";
            }

            if ((excludeTypes & NotificationType.Reblog) == NotificationType.Reblog)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + "exclude_types[]=reblog";
            }

            if ((excludeTypes & NotificationType.Mention) == NotificationType.Mention)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + "exclude_types[]=mention";
            }

            if ((excludeTypes & NotificationType.Poll) == NotificationType.Poll)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + "exclude_types[]=poll";
            }

            return this.GetMastodonList<Notification>(url + queryParams);
        }

        /// <summary>
        /// Getting a single notification.
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>Returns the Notification.</returns>
        public Task<Notification> GetNotification(long notificationId)
        {
            return this.Get<Notification>($"/api/v1/notifications/{notificationId}");
        }

        /// <summary>
        /// Deletes all notifications from the Mastodon server for the authenticated user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ClearNotifications()
        {
            return this.Post("/api/v1/notifications/clear");
        }

        /// <summary>
        /// Delete a single notification from the server.
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DismissNotification(long notificationId)
        {
            var data = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("id", notificationId.ToString()) };
            return this.Post("/api/v1/notifications/dismiss", data);
        }

        /// <summary>
        /// Fetching a user's reports.
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns a list of Reports made by the authenticated user.</returns>
        public Task<MastodonList<Report>> GetReports(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetReports(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's reports.
        /// </summary>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns a list of Reports made by the authenticated user.</returns>
        public Task<MastodonList<Report>> GetReports(ArrayOptions options)
        {
            var url = "/api/v1/reports";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Report>(url);
        }

        /// <summary>
        /// Reporting a user.
        /// </summary>
        /// <param name="accountId">The ID of the account to report.</param>
        /// <param name="statusIds">The IDs of statuses to report.</param>
        /// <param name="comment">A comment to associate with the report.</param>
        /// <param name="forward">Whether to forward to the remote admin (in case of a remote account).</param>
        /// <returns>Returns the finished Report.</returns>
        public Task<Report> Report(long accountId, IEnumerable<long>? statusIds = null, string? comment = null, bool? forward = null)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("account_id", accountId.ToString()),
            };
            foreach (var statusId in statusIds ?? Enumerable.Empty<long>())
            {
                data.Add(new KeyValuePair<string, string>("status_ids[]", statusId.ToString()));
            }

            if (comment != null)
            {
                data.Add(new KeyValuePair<string, string>("comment", comment));
            }

            if (forward.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("forward", forward.Value.ToString().ToLowerInvariant()));
            }

            return this.Post<Report>("/api/v1/reports", data);
        }

        /// <summary>
        /// Searching for content.
        /// </summary>
        /// <param name="q">The search query.</param>
        /// <param name="resolve">Whether to resolve non-local accounts.</param>
        /// <returns>Returns Results. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search.</returns>
        public Task<Results> Search(string q, bool resolve = false)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Task.FromResult(new Results());
            }

            string url = "/api/v1/search?q=" + Uri.EscapeDataString(q);
            if (resolve)
            {
                url += "&resolve=true";
            }

            return this.Get<Results>(url);
        }

        /// <summary>
        /// Searching for content.
        /// </summary>
        /// <param name="q">The search query.</param>
        /// <param name="resolve">Whether to resolve non-local accounts.</param>
        /// <returns>Returns ResultsV2. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search.</returns>
        public Task<ResultsV2> SearchV2(string q, bool resolve = false)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Task.FromResult(new ResultsV2());
            }

            string url = "/api/v2/search?q=" + Uri.EscapeDataString(q);
            if (resolve)
            {
                url += "&resolve=true";
            }

            return this.Get<ResultsV2>(url);
        }

        /// <summary>
        /// Searching for accounts.
        /// </summary>
        /// <param name="q">What to search for.</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40).</param>
        /// <param name="resolve">Attempt WebFinger look-up (default: false).</param>
        /// <param name="following">Only who the user is following (default: false).</param>
        /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database.</returns>
        public Task<List<Account>> SearchAccounts(string q, int? limit = null, bool resolve = false, bool following = false)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Task.FromResult(new List<Account>());
            }

            string url = "/api/v1/accounts/search?q=" + Uri.EscapeDataString(q);
            if (limit.HasValue)
            {
                url += "&limit=" + limit.Value;
            }

            if (resolve)
            {
                url += "&resolve=true";
            }

            if (following)
            {
                url += "&following=true";
            }

            return this.Get<List<Account>>(url);
        }

        [Obsolete("maxId ans sinceId are not used for account search. Use SearchAccounts(string q, int? limit) instead")]
        public Task<List<Account>> SearchAccounts(string q, long? maxId, long? sinceId, int? limit = null)
        {
            return this.SearchAccounts(q, limit);
        }

        [Obsolete("options are not used for account search. Use SearchAccounts(string q, int? limit) instead")]
        public Task<List<Account>> SearchAccounts(string q, ArrayOptions options)
        {
            return this.SearchAccounts(q, options?.Limit);
        }

        /// <summary>
        /// Listing all text filters the user has configured that potentially must be applied client-side.
        /// </summary>
        /// <returns>Returns an array of filters.</returns>
        public Task<IEnumerable<Filter>> GetFilters()
        {
            return this.Get<IEnumerable<Filter>>("/api/v1/filters");
        }

        /// <summary>
        /// Creating a new filter.
        /// </summary>
        /// <param name="phrase">Keyword or phrase to filter.</param>
        /// <param name="context">Filtering context. At least one context must be specified.</param>
        /// <param name="irreversible">Irreversible filtering will only work in home and notifications contexts by fully dropping the records.</param>
        /// <param name="wholeWord">Whether to consider word boundaries when matching.</param>
        /// <param name="expiresIn">Number that indicates seconds. Filter will be expire in seconds after API processed. Leave null for no expiration.</param>
        /// <returns>Returns a created filter.</returns>
        public Task<Filter> CreateFilter(string phrase, FilterContext context, bool irreversible = false, bool wholeWord = false, uint? expiresIn = null)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                throw new ArgumentException("The phrase is required", nameof(phrase));
            }

            if (context == 0)
            {
                throw new ArgumentException("At least one context must be specified", nameof(context));
            }

            var data = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("phrase", phrase) };
            foreach (FilterContext checkFlag in new[] { FilterContext.Home, FilterContext.Notifications, FilterContext.Public, FilterContext.Thread })
            {
                if ((context & checkFlag) == checkFlag)
                {
                    data.Add(new KeyValuePair<string, string>("context[]", checkFlag.ToString().ToLowerInvariant()));
                }
            }

            if (irreversible)
            {
                data.Add(new KeyValuePair<string, string>("irreversible", "true"));
            }

            if (wholeWord)
            {
                data.Add(new KeyValuePair<string, string>("whole_word", "true"));
            }

            if (expiresIn.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("expires_in", expiresIn.Value.ToString()));
            }

            return this.Post<Filter>("/api/v1/filters", data);
        }

        /// <summary>
        /// Getting a text filter.
        /// </summary>
        /// <param name="filterId">Filter ID.</param>
        /// <returns>Returns a filter.</returns>
        public Task<Filter> GetFilter(long filterId)
        {
            return this.Get<Filter>($"/api/v1/filters/{filterId}");
        }

        /// <summary>
        /// Updating a text filter.
        /// </summary>
        /// <param name="filterId">Filter ID.</param>
        /// <param name="phrase">A new keyword or phrase to filter, or null to keep.</param>
        /// <param name="context">A new filtering context, or null to keep.</param>
        /// <param name="irreversible">A new irreversible flag, or null to keep.</param>
        /// <param name="wholeWord">A new whole_word flag, or null to keep.</param>
        /// <param name="expiresIn">A new number that indicates seconds. Filter will be expire in seconds after API processed. Leave null to keep.</param>
        /// <returns>Returns an updated filter.</returns>
        public Task<Filter> UpdateFilter(long filterId, string? phrase = null, FilterContext? context = null, bool? irreversible = null, bool? wholeWord = null, uint? expiresIn = null)
        {
            if (context == 0)
            {
                throw new ArgumentException("At least one context to filter must be specified", nameof(context));
            }

            var data = new List<KeyValuePair<string, string>>();
            if (phrase != null)
            {
                data.Add(new KeyValuePair<string, string>("phrase", phrase));
            }

            if (context.HasValue)
            {
                foreach (FilterContext checkFlag in new[] { FilterContext.Home, FilterContext.Notifications, FilterContext.Public, FilterContext.Thread })
                {
                    if ((context & checkFlag) == checkFlag)
                    {
                        data.Add(new KeyValuePair<string, string>("context[]", checkFlag.ToString().ToLowerInvariant()));
                    }
                }
            }

            if (irreversible.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("irreversible", irreversible.Value.ToString().ToLowerInvariant()));
            }

            if (wholeWord.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("whole_word", wholeWord.Value.ToString().ToLowerInvariant()));
            }

            if (expiresIn.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("expires_in", expiresIn.Value.ToString()));
            }

            return this.Put<Filter>($"/api/v1/filters/{filterId}", data);
        }

        /// <summary>
        /// Deleting a text filter.
        /// </summary>
        /// <param name="filterId"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteFilter(long filterId)
        {
            return this.Delete($"/api/v1/filters/{filterId}");
        }

        /// <summary>
        /// Get a poll.
        /// </summary>
        /// <param name="id">The ID of the poll.</param>
        /// <returns>Returns Poll.</returns>
        public Task<Poll> GetPoll(long id)
        {
            return this.Get<Poll>("/api/v1/polls/" + id.ToString());
        }

        /// <summary>
        /// Vote on a poll.
        /// </summary>
        /// <param name="id">The ID of the poll.</param>
        /// <param name="choices">Array of choice indices.</param>
        /// <returns>Returns Poll.</returns>
        public Task<Poll> Vote(long id, IEnumerable<int> choices)
        {
            var data = choices
                .Select(index => new KeyValuePair<string, string>("choices[]", index.ToString()))
                .ToArray();
            return this.Post<Poll>("/api/v1/polls/" + id.ToString() + "/votes", data);
        }
    }
}
