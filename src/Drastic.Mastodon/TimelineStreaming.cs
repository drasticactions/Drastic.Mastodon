// <copyright file="TimelineStreaming.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public abstract class TimelineStreaming
    {
        protected readonly StreamingType streamingType;
        protected readonly string? param;
        protected readonly string? accessToken;

        protected TimelineStreaming(StreamingType type, string? param, string? accessToken)
        {
            this.streamingType = type;
            this.param = param;
            this.accessToken = accessToken;
        }

        public event EventHandler<StreamUpdateEventArgs>? OnUpdate;

        public event EventHandler<StreamNotificationEventArgs>? OnNotification;

        public event EventHandler<StreamDeleteEventArgs>? OnDelete;

        public event EventHandler<StreamFiltersChangedEventArgs>? OnFiltersChanged;

        public event EventHandler<StreamConversationEvenTargs>? OnConversation;

        public abstract Task Start();

        public abstract void Stop();

        protected void SendEvent(string eventName, string data)
        {
            switch (eventName)
            {
                case "update":
                    var status = JsonSerializer.Deserialize<Status>(data);
                    if (status != null)
                    {
                        this.OnUpdate?.Invoke(this, new StreamUpdateEventArgs(status));
                    }

                    break;
                case "notification":
                    var notification = JsonSerializer.Deserialize<Notification>(data);
                    if (notification != null)
                    {
                        this.OnNotification?.Invoke(this, new StreamNotificationEventArgs(notification));
                    }

                    break;
                case "delete":
                    if (long.TryParse(data, out long statusId))
                    {
                        this.OnDelete?.Invoke(this, new StreamDeleteEventArgs(statusId));
                    }

                    break;
                case "filters_changed":
                    this.OnFiltersChanged?.Invoke(this, new StreamFiltersChangedEventArgs());
                    break;
                case "conversation":
                    var conversation = JsonSerializer.Deserialize<Conversation>(data);
                    if (conversation != null)
                    {
                        this.OnConversation?.Invoke(this, new StreamConversationEvenTargs(conversation));
                    }

                    break;
            }
        }
    }
}