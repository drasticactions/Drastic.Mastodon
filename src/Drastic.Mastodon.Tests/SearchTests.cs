// <copyright file="SearchTests.cs" company="Drastic Actions">
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
    public class SearchTests : MastodonClientTests
    {
        [Fact]
        public async Task SearchAccounts()
        {
            var client = this.GetTestClient();

            var found = await client.SearchAccounts("glacasa");
            Assert.True(found.Any());
        }

        [Fact]
        public async Task Search()
        {
            var client = this.GetTestClient();
            var found = await client.Search("search", false);

            Assert.True(found.Hashtags.Any());
        }
    }
}
