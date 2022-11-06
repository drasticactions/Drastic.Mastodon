// <copyright file="MuteBlockFavTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Drastic.Mastodon.Tests
{
    public class MuteBlockFavTests : MastodonClientTests
    {
        [Fact]
        public async Task Block()
        {
            var client = this.GetTestClient();
            var rel = await client.Block(10);
            Assert.NotNull(rel);
            Assert.True(rel.Blocking);
        }

        [Fact]
        public async Task Unblock()
        {
            var client = this.GetTestClient();
            var rel = await client.Unblock(10);
            Assert.NotNull(rel);
            Assert.False(rel.Blocking);
        }

        [Fact]
        public async Task GetBlocks()
        {
            var client = this.GetTestClient();
            var blocked = await client.GetBlocks();
            Assert.NotNull(blocked);
        }

        [Fact]
        public async Task Mute()
        {
            var client = this.GetTestClient();
            var rel = await client.Mute(10);
            Assert.NotNull(rel);
            Assert.True(rel.Muting);
        }

        [Fact]
        public async Task Unmute()
        {
            var client = this.GetTestClient();
            var rel = await client.Unmute(10);
            Assert.NotNull(rel);
            Assert.False(rel.Muting);
        }

        [Fact]
        public async Task GetMutes()
        {
            var client = this.GetTestClient();
            var muted = await client.GetMutes();
            Assert.NotNull(muted);
        }

        [Fact]
        public async Task FavouriteUnfavourite()
        {
            var client = this.GetTestClient();
            var tl = await client.GetHomeTimeline(limit: 1);
            var status = tl.First();

            status = await client.Favourite(status.Id);
            Assert.True(status.Favourited);

            status = await client.Unfavourite(status.Id);
            Assert.False(status.Favourited);
        }

        [Fact]
        public async Task GetFavourites()
        {
            var client = this.GetTestClient();
            var favs = await client.GetFavourites();
            Assert.NotNull(favs);
        }
    }
}
