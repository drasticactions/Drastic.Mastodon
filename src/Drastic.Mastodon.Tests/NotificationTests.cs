// <copyright file="NotificationTests.cs" company="Drastic Actions">
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
    public class NotificationTests : MastodonClientTests
    {
        [Fact]
        public async Task GetNotifications()
        {
            var testClient = this.GetTestClient();
            var privClient = this.GetPrivateClient();

            // Have 1 notif
            await testClient.ClearNotifications();
            await privClient.PostStatus("@TestAccount hello", Visibility.Direct);

            // Get notif
            var notifications = await testClient.GetNotifications();
            Assert.True(notifications.Any());
        }

        [Fact]
        public async Task GetNotification()
        {
            var testClient = this.GetTestClient();
            var privClient = this.GetPrivateClient();

            // Have 1 notif
            await testClient.ClearNotifications();
            await privClient.PostStatus("@TestAccount hello", Visibility.Direct);

            // Get notif
            var notifications = await testClient.GetNotifications();
            var notifId = notifications.First(n => n.Account.Id == 11).Id;

            var notification = await testClient.GetNotification(notifId);
            Assert.NotNull(notification);
            Assert.Equal(11, notification.Account.Id);
        }

        [Fact]
        public async Task ClearNotifications()
        {
            var testClient = this.GetTestClient();
            var privClient = this.GetPrivateClient();

            // Have notifs
            await privClient.PostStatus("@TestAccount hello", Visibility.Direct);
            var notifications = await testClient.GetNotifications();
            Assert.True(notifications.Any());

            // Clear notifs
            await testClient.ClearNotifications();

            notifications = await testClient.GetNotifications();
            Assert.False(notifications.Any());
        }
    }
}
