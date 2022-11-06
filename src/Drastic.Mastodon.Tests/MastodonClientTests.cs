// <copyright file="MastodonClientTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drastic.Mastodon;
using Drastic.Mastodon.Entities;
using Xunit;

namespace InitTest
{
    public partial class InitTest
    {
        private string Instance => string.Empty;

        // Add instance name, email and password of a test account, and run this test to get authentication tokens
        // The test will throw an Exception with the token data, just copy and paste it on the class below
        // /!\ DO NOT COMMIT YOUR E-MAIL, PASSWORD OR ACCESS TOKENS /!\

        // The tests were initially written for a set of known accounts, most of them will fail on different accounts,
        // don't just "Run all", but you can use them for custom testing
        [Fact]
        public async Task GetTokens()
        {
            var authClient = new AuthenticationClient(this.Instance);
            var appRegistration = await authClient.CreateApp("Drastic.Mastodon Unit Test", Scope.Read | Scope.Write | Scope.Follow);

            var accessToken1 = await authClient.ConnectWithPassword(this.Email1, this.Password1);
            var accessToken2 = await authClient.ConnectWithPassword(this.Email2, this.Password2);

            var authCodeBuilder = new StringBuilder();
            authCodeBuilder.AppendLine("private static AppRegistration app = new AppRegistration");
            authCodeBuilder.AppendLine("{");
            authCodeBuilder.AppendLine("Instance = \"" + this.Instance + "\",");
            authCodeBuilder.AppendLine("ClientId = \"" + appRegistration.ClientId + "\",");
            authCodeBuilder.AppendLine("ClientSecret = \"" + appRegistration.ClientSecret + "\"");
            authCodeBuilder.AppendLine("};");
            authCodeBuilder.AppendLine("private static string testAccessToken = \"" + accessToken1.AccessToken + "\";");
            authCodeBuilder.AppendLine("private static string privateAccessToken = \"" + accessToken2.AccessToken + "\";");

            var authCode = authCodeBuilder.ToString();

            throw new Exception(authCode);
        }

        private string Email1 => string.Empty;

        private string Password1 => string.Empty;

        private string Email2 => string.Empty;

        private string Password2 => string.Empty;
    }
}

namespace Drastic.Mastodon.Tests
{
    public partial class MastodonClientTests
    {
        // ####################
        // ##  REPLACE HERE  ##
        // ####################
        private static AppRegistration app = new AppRegistration
        {
            Instance = string.Empty,
            ClientId = string.Empty,
            ClientSecret = string.Empty,
        };

        private static string testAccessToken = string.Empty;
        private static string privateAccessToken = string.Empty;

        // ####################
        // ####################
        protected MastodonClient GetTestClient()
        {
            return new MastodonClient(app, new Auth() { AccessToken = testAccessToken });
        }

        protected MastodonClient GetPrivateClient()
        {
            return new MastodonClient(app, new Auth() { AccessToken = privateAccessToken });
        }
    }
}
