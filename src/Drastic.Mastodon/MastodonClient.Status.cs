﻿// <copyright file="MastodonClient.Status.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public partial class MastodonClient
    {
        /// <summary>
        /// Fetching a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Status.</returns>
        public Task<Status> GetStatus(long statusId)
        {
            return this.Get<Status>($"/api/v1/statuses/{statusId}");
        }

        /// <summary>
        /// Getting status context.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Context.</returns>
        public Task<Context> GetStatusContext(long statusId)
        {
            return this.Get<Context>($"/api/v1/statuses/{statusId}/context");
        }

        /// <summary>
        /// Getting a card associated with a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Card.</returns>
        public Task<Card> GetStatusCard(long statusId)
        {
            return this.Get<Card>($"/api/v1/statuses/{statusId}/card");
        }

        /// <summary>
        /// Getting who reblogged a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetRebloggedBy(long statusId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetRebloggedBy(statusId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Getting who reblogged a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetRebloggedBy(long statusId, ArrayOptions options)
        {
            var url = $"/api/v1/statuses/{statusId}/reblogged_by";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Getting who favourited a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value.</param>
        /// <param name="sinceId">Get items with ID greater than this value.</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80).</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetFavouritedBy(long statusId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return this.GetFavouritedBy(statusId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Getting who favourited a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get.</param>
        /// <returns>Returns an array of Accounts.</returns>
        public Task<MastodonList<Account>> GetFavouritedBy(long statusId, ArrayOptions options)
        {
            var url = $"/api/v1/statuses/{statusId}/favourited_by";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }

            return this.GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Posting a new status.
        /// </summary>
        /// <param name="status">The text of the status.</param>
        /// <param name="visibility">either "direct", "private", "unlisted" or "public".</param>
        /// <param name="replyStatusId">local ID of the status you want to reply to.</param>
        /// <param name="mediaIds">array of media IDs to attach to the status (maximum 4).</param>
        /// <param name="sensitive">set this to mark the media of the status as NSFW.</param>
        /// <param name="spoilerText">text to be shown as a warning before the actual content.</param>
        /// <param name="scheduledAt">DateTime to schedule posting of status.</param>
        /// <param name="language">Override language code of the toot (ISO 639-2).</param>
        /// <param name="poll">Nested parameters to attach a poll to the status.</param>
        /// <returns>Returns Status.</returns>
        public Task<Status> PostStatus(string status, Visibility? visibility = null, long? replyStatusId = null, IEnumerable<long>? mediaIds = null, bool sensitive = false, string? spoilerText = null, DateTime? scheduledAt = null, string? language = null, PollParameters? poll = null)
        {
            if (string.IsNullOrEmpty(status) && (mediaIds == null || !mediaIds.Any()))
            {
                throw new ArgumentException("A status must have either text (status) or media (mediaIds)", nameof(status));
            }

            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("status", status),
            };

            if (replyStatusId.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("in_reply_to_id", replyStatusId.Value.ToString()));
            }

            if (mediaIds != null && mediaIds.Any())
            {
                foreach (var mediaId in mediaIds)
                {
                    data.Add(new KeyValuePair<string, string>("media_ids[]", mediaId.ToString()));
                }
            }

            if (sensitive)
            {
                data.Add(new KeyValuePair<string, string>("sensitive", "true"));
            }

            if (spoilerText != null)
            {
                data.Add(new KeyValuePair<string, string>("spoiler_text", spoilerText));
            }

            if (visibility.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("visibility", visibility.Value.ToString().ToLowerInvariant()));
            }

            if (scheduledAt.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("scheduled_at", scheduledAt.Value.ToString("o")));
            }

            if (language != null)
            {
                data.Add(new KeyValuePair<string, string>("language", language));
            }

            if (poll != null)
            {
                data.AddRange(poll.Options.Select(option => new KeyValuePair<string, string>("poll[options][]", option)));
                data.Add(new KeyValuePair<string, string>("poll[expires_in]", poll.ExpiresIn.TotalSeconds.ToString()));
                if (poll.Multiple.HasValue)
                {
                    data.Add(new KeyValuePair<string, string>("poll[multiple]", poll.Multiple.Value.ToString()));
                }

                if (poll.HideTotals.HasValue)
                {
                    data.Add(new KeyValuePair<string, string>("poll[hide_totals]", poll.HideTotals.Value.ToString()));
                }
            }

            return this.Post<Status>("/api/v1/statuses", data);
        }

        /// <summary>
        /// Deleting a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteStatus(long statusId)
        {
            return this.Delete($"/api/v1/statuses/{statusId}");
        }

        /// <summary>
        /// Get scheduled statuses.
        /// </summary>
        /// <returns>Returns array of ScheduledStatus.</returns>
        public Task<IEnumerable<ScheduledStatus>> GetScheduledStatuses()
        {
            return this.Get<IEnumerable<ScheduledStatus>>("/api/v1/scheduled_statuses");
        }

        /// <summary>
        /// Get scheduled status.
        /// </summary>
        /// <param name="scheduledStatusId"></param>
        /// <returns>Returns ScheduledStatus.</returns>
        public Task<ScheduledStatus> GetScheduledStatus(long scheduledStatusId)
        {
            return this.Get<ScheduledStatus>("/api/v1/scheduled_statuses/" + scheduledStatusId);
        }

        /// <summary>
        /// Update Scheduled status. Only scheduled_at can be changed. To change the content, delete it and post a new status.
        /// </summary>
        /// <param name="scheduledStatusId"></param>
        /// <param name="scheduledAt">DateTime to schedule posting of status.</param>
        /// <returns>Returns ScheduledStatus.</returns>
        public Task<ScheduledStatus> UpdateScheduledStatus(long scheduledStatusId, DateTime? scheduledAt)
        {
            var data = new List<KeyValuePair<string, string>>();
            if (scheduledAt.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("scheduled_at", scheduledAt.Value.ToString()));
            }

            return this.Put<ScheduledStatus>("/api/v1/scheduled_statuses/" + scheduledStatusId, data);
        }

        /// <summary>
        /// Remove Scheduled status.
        /// </summary>
        /// <param name="scheduledStatusId"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteScheduledStatus(long scheduledStatusId)
        {
            return this.Delete("/api/v1/scheduled_statuses/" + scheduledStatusId);
        }

        /// <summary>
        /// Reblogging a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> Reblog(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/reblog");
        }

        /// <summary>
        /// Unreblogging a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> Unreblog(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/unreblog");
        }

        /// <summary>
        /// Favouriting a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> Favourite(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/favourite");
        }

        /// <summary>
        /// Unfavouriting a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> Unfavourite(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/unfavourite");
        }

        /// <summary>
        /// Muting a conversation of a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> MuteConversation(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/mute");
        }

        /// <summary>
        /// Unmuting a conversation of a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> UnmuteConversation(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/unmute");
        }

        /// <summary>
        /// Pinning a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> Pin(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/pin");
        }

        /// <summary>
        /// Unpinning a status.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status.</returns>
        public Task<Status> Unpin(long statusId)
        {
            return this.Post<Status>($"/api/v1/statuses/{statusId}/unpin");
        }
    }
}
