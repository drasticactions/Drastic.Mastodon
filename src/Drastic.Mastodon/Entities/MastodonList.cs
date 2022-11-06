// <copyright file="MastodonList.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Drastic.Mastodon.Entities
{
    public class MastodonList<T> : List<T>
    {
        public long? NextPageMaxId { get; internal set; }

        public long? PreviousPageSinceId { get; internal set; }

        public long? PreviousPageMinId { get; internal set; }
    }
}
