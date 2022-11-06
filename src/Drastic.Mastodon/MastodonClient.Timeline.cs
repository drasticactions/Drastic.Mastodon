// <copyright file="MastodonClient.Timeline.cs" company="Drastic Actions">
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
        /// Retrieving Home timeline.
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit">Maximum number of items to get (Default 20).</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetHomeTimeline(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetHomeTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Retrieving Home timeline.
        /// </summary>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetHomeTimeline(ArrayOptions options)
        {
            string url = "/api/v1/timelines/home";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Status>(url);
        }

        /// <summary>
        /// Conversations (direct messages) for an account.
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit">Maximum number of items to get (Default 20).</param>
        /// <returns>Returns array of Conversation.</returns>
        public Task<MastodonList<Conversation>> GetConversations(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetConversations(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Conversations (direct messages) for an account.
        /// </summary>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns array of Conversation.</returns>
        public Task<MastodonList<Conversation>> GetConversations(ArrayOptions options)
        {
            string url = "/api/v1/conversations";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Conversation>(url);
        }

        /// <summary>
        /// Retrieving Public timeline.
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit">Maximum number of items to get (Default 20).</param>
        /// <param name="local">Only return statuses originating from this instance.</param>
        /// <param name="onlyMedia">Only statuses with media attachments.</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetPublicTimeline(long? maxId = null, long? sinceId = null, int? limit = null, bool local = false, bool onlyMedia = false)
        {
            return this.GetPublicTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, local, onlyMedia);
        }

        /// <summary>
        /// Retrieving Public timeline.
        /// </summary>
        /// <param name="options">Define the first and last items to get.</param>
        /// <param name="local">Only return statuses originating from this instance.</param>
        /// <param name="onlyMedia">Only statuses with media attachments.</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetPublicTimeline(ArrayOptions options, bool local = false, bool onlyMedia = false)
        {
            string url = "/api/v1/timelines/public";

            var queryParams = string.Empty;
            if (local)
            {
                queryParams += "?local=true";
            }

            if (onlyMedia)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + "only_media=true";
            }

            if (options != null)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + options.ToQueryString();
            }

            return this.GetMastodonList<Status>(url + queryParams);
        }

        /// <summary>
        /// Retrieving Tag timeline.
        /// </summary>
        /// <param name="hashtag">The tag to retieve.</param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit">Maximum number of items to get (Default 20).</param>
        /// <param name="local">Only return statuses originating from this instance.</param>
        /// <param name="onlyMedia">Only statuses with media attachments.</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetTagTimeline(string hashtag, long? maxId = null, long? sinceId = null, int? limit = null, bool local = false, bool onlyMedia = false)
        {
            return this.GetTagTimeline(hashtag, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, local, onlyMedia);
        }

        /// <summary>
        /// Retrieving Tag timeline.
        /// </summary>
        /// <param name="hashtag">The tag to retieve.</param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <param name="local">Only return statuses originating from this instance.</param>
        /// <param name="onlyMedia">Only statuses with media attachments.</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetTagTimeline(string hashtag, ArrayOptions options, bool local = false, bool onlyMedia = false)
        {
            string url = "/api/v1/timelines/tag/" + hashtag;

            var queryParams = string.Empty;
            if (local)
            {
                queryParams += "?local=true";
            }

            if (onlyMedia)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + "only_media=true";
            }

            if (options != null)
            {
                queryParams += (queryParams != string.Empty ? "&" : "?") + options.ToQueryString();
            }

            return this.GetMastodonList<Status>(url + queryParams);
        }

        /// <summary>
        /// Retrieving List timeline.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit">Maximum number of items to get (Default 20).</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetListTimeline(long listId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetListTimeline(listId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Retrieving List timeline.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Statuses, most recent ones first.</returns>
        public Task<MastodonList<Status>> GetListTimeline(long listId, ArrayOptions options)
        {
            string url = "/api/v1/timelines/list/" + listId;

            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Status>(url);
        }

        public TimelineStreaming GetPublicStreaming()
        {
            return this.GetStreaming(StreamingType.Public, null);
        }

        public TimelineStreaming GetPublicLocalStreaming()
        {
            return this.GetStreaming(StreamingType.PublicLocal, null);
        }

#if NETSTANDARD2_0
        private Lazy<Task<Instance>> instanceGetter;
#endif

        private TimelineStreaming GetStreaming(StreamingType streamingType, string? param)
        {
#if NETSTANDARD2_0
            return new TimelineWebSocketStreaming(streamingType, param, Instance, instanceGetter.Value, AuthToken?.AccessToken, client);
#else
            return new TimelineHttpStreaming(streamingType, param, this.Instance, this.AuthToken?.AccessToken, this.client);
#endif
        }

        public TimelineStreaming GetUserStreaming()
        {
            return this.GetStreaming(StreamingType.User, null);
        }

        public TimelineStreaming GetHashtagStreaming(string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", nameof(hashtag));
            }

            return this.GetStreaming(StreamingType.Hashtag, hashtag);
        }

        public TimelineStreaming GetHashtagLocalStreaming(string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", nameof(hashtag));
            }

            return this.GetStreaming(StreamingType.HashtagLocal, hashtag);
        }

        public TimelineStreaming GetDirectMessagesStreaming()
        {
            return this.GetStreaming(StreamingType.Direct, null);
        }

        public TimelineStreaming GetListStreaming(long listId)
        {
            return this.GetStreaming(StreamingType.List, listId.ToString());
        }

        public TimelineStreaming GetListStreaming(List list)
        {
            if (list == null)
            {
                throw new ArgumentException("You must specify a list", nameof(list));
            }

            return this.GetStreaming(StreamingType.List, list.Id.ToString());
        }

        [Obsolete("The url is not used, please use GetPublicStreaming() method")]
        public TimelineStreaming GetPublicStreaming(string streamingApiUrl) => this.GetPublicStreaming();

        [Obsolete("The url is not used, please use GetUserStreaming() method")]
        public TimelineStreaming GetUserStreaming(string streamingApiUrl) => this.GetUserStreaming();

        [Obsolete("The url is not used, please use GetHashtagStreaming(string hashtag) method")]
        public TimelineStreaming GetHashtagStreaming(string streamingApiUrl, string hashtag) => this.GetHashtagStreaming(hashtag);
    }
}
