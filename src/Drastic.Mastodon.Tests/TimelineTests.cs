// <copyright file="TimelineTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Drastic.Mastodon.Tests
{
    public class TimelineTests : MastodonClientTests
    {
        [Fact]
        public async Task GetHomeTimeline()
        {
            var client = this.GetTestClient();
            var timeline = await client.GetHomeTimeline();
            Assert.NotNull(timeline);
        }

        [Fact]
        public async Task GetPublicTimeline()
        {
            var client = this.GetTestClient();
            var timeline = await client.GetPublicTimeline();
            Assert.NotNull(timeline);
        }

        [Fact]
        public async Task GetTagTimeline()
        {
            var client = this.GetTestClient();
            var timeline = await client.GetTagTimeline("mastodon");
            Assert.NotNull(timeline);
        }
    }
}
