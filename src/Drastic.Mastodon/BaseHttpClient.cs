// <copyright file="BaseHttpClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public abstract class BaseHttpClient : IBaseHttpClient
    {
        protected readonly HttpClient client;
        private string instance = string.Empty;

        private Regex idFinderRegex = new Regex("_id=([0-9]+)");

        protected BaseHttpClient(HttpClient client)
        {
            this.client = client;
        }

        public AppRegistration? AppRegistration { get; set; }

        public Auth? AuthToken { get; set; }

        public string Instance
        {
            get
            {
                return this.instance;
            }

            set
            {
                this.CheckInstance(value);
                this.instance = value;
            }
        }

        protected async Task<string> Delete(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        {
            string url = "https://" + this.Instance + route;
            if (data != null)
            {
                var querystring = "?" + string.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
                url += querystring;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                this.AddHttpHeader(request);
                using (var response = await this.client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        protected async Task<string> Get(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        {
            string url = "https://" + this.Instance + route;
            if (data != null)
            {
                var querystring = "?" + string.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
                url += querystring;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                this.AddHttpHeader(request);
                using (var response = await this.client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private void CheckInstance(string instance)
        {
            var notSupportedList = new List<string> { "gab.", "truthsocial." };
            var lowered = instance.ToLowerInvariant();
            if (notSupportedList.Any(n => lowered.Contains(n)))
            {
                throw new NotSupportedException();
            }
        }

        private void AddHttpHeader(HttpRequestMessage request)
        {
            if (this.AuthToken != null)
            {
                request.Headers.Add("Authorization", "Bearer " + this.AuthToken.AccessToken);
            }
        }

        protected async Task<T> Get<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
            where T : class
        {
            var content = await this.Get(route, data);
            return this.TryDeserialize<T>(content);
        }

        protected async Task<MastodonList<T>> GetMastodonList<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        {
            string url = "https://" + this.Instance + route;
            if (data != null)
            {
                var querystring = "?" + string.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
                url += querystring;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                this.AddHttpHeader(request);
                using (var response = await this.client.SendAsync(request))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = this.TryDeserialize<MastodonList<T>>(content);

                    // Read `Link` header
                    IEnumerable<string>? linkHeader;
                    if (response.Headers.TryGetValues("Link", out linkHeader))
                    {
                        var links = linkHeader.Single().Split(',');
                        foreach (var link in links)
                        {
                            if (link.Contains("rel=\"next\""))
                            {
                                result.NextPageMaxId = long.Parse(this.idFinderRegex.Match(link).Groups[1].Value);
                            }

                            if (link.Contains("rel=\"prev\""))
                            {
                                if (link.Contains("since_id"))
                                {
                                    result.PreviousPageSinceId = long.Parse(this.idFinderRegex.Match(link).Groups[1].Value);
                                }

                                if (link.Contains("min_id"))
                                {
                                    result.PreviousPageMinId = long.Parse(this.idFinderRegex.Match(link).Groups[1].Value);
                                }
                            }
                        }
                    }

                    return result;
                }
            }
        }

        protected async Task<string> Post(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        {
            string url = "https://" + this.Instance + route;

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                this.AddHttpHeader(request);
                request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
                using (var response = await this.client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        protected async Task<string> PostMedia(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
        {
            string url = "https://" + this.Instance + route;

            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            this.AddHttpHeader(request);
            var content = new MultipartFormDataContent();

            if (media != null)
            {
                foreach (var m in media)
                {
                    content.Add(new StreamContent(m.Media), m.ParamName!, m.FileName);
                }
            }

            if (data != null)
            {
                foreach (var pair in data)
                {
                    content.Add(new StringContent(pair.Value), pair.Key);
                }
            }

            request.Content = content;

            using var response = await this.client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> Post<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
            where T : class
        {
            var content = media != null && media.Any() ? await this.PostMedia(route, data, media) : await this.Post(route, data);
            return this.TryDeserialize<T>(content);
        }

        protected async Task<string> Put(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        {
            string url = "https://" + this.Instance + route;

            using (var request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                this.AddHttpHeader(request);

                request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
                using (var response = await this.client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        protected async Task<T> Put<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        {
            return this.TryDeserialize<T>(await this.Put(route, data));
        }

        protected async Task<string> Patch(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        {
            string url = "https://" + this.Instance + route;

            using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), url))
            {
                this.AddHttpHeader(request);
                request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
                using (var response = await this.client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        protected async Task<string> PatchMedia(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
        {
            string url = "https://" + this.Instance + route;

            using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), url))
            {
                this.AddHttpHeader(request);

                var content = new MultipartFormDataContent();

                if (media != null)
                {
                    foreach (var m in media)
                    {
                        content.Add(new StreamContent(m.Media), m.ParamName!, m.FileName);
                    }
                }

                if (data != null)
                {
                    foreach (var pair in data)
                    {
                        content.Add(new StringContent(pair.Value), pair.Key);
                    }
                }

                request.Content = content;
                using (var response = await this.client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        protected async Task<T> Patch<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
            where T : class
        {
            var content = media != null && media.Any() ? await this.PatchMedia(route, data, media) : await this.Patch(route, data);
            return this.TryDeserialize<T>(content);
        }

        private T TryDeserialize<T>(string json)
        {
            if (json[0] == '{')
            {
                var error = JsonSerializer.Deserialize<Error>(json);
                if (error != null && !string.IsNullOrEmpty(error.Description))
                {
                    throw new ServerErrorException(error);
                }
            }

            return JsonSerializer.Deserialize<T>(json)!;
        }
    }
}
