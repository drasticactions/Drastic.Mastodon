// <copyright file="TimelineWebSocketStreaming.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public class TimelineWebSocketStreaming : TimelineHttpStreaming
    {
        private const int ReceiveChunkSize = 512;
        private readonly Task<Instance> instanceGetter;
        private ClientWebSocket? socket;

        public TimelineWebSocketStreaming(StreamingType type, string? param, string instance, Task<Instance> instanceGetter, string? accessToken)
            : this(type, param, instance, instanceGetter, accessToken, DefaultHttpClient.Instance)
        {
        }

        public TimelineWebSocketStreaming(StreamingType type, string? param, string instance, Task<Instance> instanceGetter, string? accessToken, HttpClient client)
            : base(type, param, instance, accessToken, client)
        {
            this.instanceGetter = instanceGetter;
        }

        public override async Task Start()
        {
            var instance = await this.instanceGetter;
            var url = instance?.Urls?.StreamingAPI;

            if (url == null)
            {
                // websocket disabled, fallback to http streaming
                await base.Start();
                return;
            }

            url += "/api/v1/streaming?access_token=" + this.accessToken;

            switch (this.streamingType)
            {
                case StreamingType.User:
                    url += "&stream=user";
                    break;
                case StreamingType.Public:
                    url += "&stream=public";
                    break;
                case StreamingType.PublicLocal:
                    url += "&stream=public:local";
                    break;
                case StreamingType.Hashtag:
                    url += "&stream=hashtag&tag=" + this.param;
                    break;
                case StreamingType.HashtagLocal:
                    url += "&stream=hashtag:local&tag=" + this.param;
                    break;
                case StreamingType.List:
                    url += "&stream=list&list=" + this.param;
                    break;
                case StreamingType.Direct:
                    url += "&stream=direct";
                    break;
                default:
                    throw new NotImplementedException();
            }

            this.socket = new ClientWebSocket();
            await this.socket.ConnectAsync(new Uri(url), CancellationToken.None);

            byte[] buffer = new byte[ReceiveChunkSize];
            MemoryStream ms = new MemoryStream();
            while (this.socket != null)
            {
                var result = await this.socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                ms.Write(buffer, 0, result.Count);

                if (result.EndOfMessage)
                {
                    var messageStr = Encoding.UTF8.GetString(ms.ToArray());

                    var message = JsonSerializer.Deserialize<TimelineMessage>(messageStr);
                    if (message != null)
                    {
                        this.SendEvent(message.Event, message.Payload);
                    }

                    ms.Dispose();
                    ms = new MemoryStream();
                }
            }

            ms.Dispose();

            this.Stop();
        }

        public override void Stop()
        {
            if (this.socket != null)
            {
                this.socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                this.socket.Dispose();
                this.socket = null;
            }

            base.Stop();
        }

        private class TimelineMessage
        {
            public string Event { get; set; } = default!;

            public string Payload { get; set; } = default!;
        }
    }
}
