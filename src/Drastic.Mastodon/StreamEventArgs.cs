// <copyright file="StreamEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public class StreamUpdateEventArgs : EventArgs
    {
        public StreamUpdateEventArgs(Status status)
        {
            this.Status = status;
        }

        public Status Status { get; set; }
    }

    public class StreamNotificationEventArgs : EventArgs
    {
        public StreamNotificationEventArgs(Notification notification)
        {
            this.Notification = notification;
        }

        public Notification Notification { get; set; }
    }

    public class StreamDeleteEventArgs : EventArgs
    {
        public StreamDeleteEventArgs(long statusId)
        {
            this.StatusId = statusId;
        }

        public long StatusId { get; set; }
    }

    public class StreamFiltersChangedEventArgs : EventArgs
    {
    }

    public class StreamConversationEvenTargs : EventArgs
    {
        public StreamConversationEvenTargs(Conversation conversation)
        {
            this.Conversation = conversation;
        }

        public Conversation Conversation { get; set; }
    }
}
