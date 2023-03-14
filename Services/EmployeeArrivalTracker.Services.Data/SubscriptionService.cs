namespace EmployeeArrivalTracker.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Common;
    using EmployeeArrivalTracker.Data.Models;

    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;

    public class SubscriptionService : IDisposable, ISubscriptionService
    {
        private readonly HttpClient httpClient;
        private readonly IMemoryCache memoryCache;
        private readonly string subscriptionUrl;
        private readonly string subscriptionDateFormat;
        private readonly string apiUrl;
        private bool disposed = false;


        public SubscriptionService(HttpClient httpClient, IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.httpClient = httpClient;
            this.memoryCache = memoryCache;
            this.subscriptionUrl = configuration.GetValue<string>(GlobalConstants.SubscriptionUrlKey);
            this.subscriptionDateFormat = configuration.GetValue<string>(GlobalConstants.SubscriptionDateFormatKey);
            this.apiUrl = configuration.GetValue<string>(GlobalConstants.UrlKey);
        }

        public async Task SubscribeAsync()
        {
            string uri = this.BuildUri();
            Subscription subscriptionResponse = await this.GetResponse(uri);

            this.SetInMemoryCache(subscriptionResponse);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.httpClient.Dispose();
                }

                this.disposed = true;
            }
        }

        private string BuildUri()
        {
            this.httpClient.DefaultRequestHeaders.Add("Accept-Client", GlobalConstants.AcceptClientHeaderValue);

            var queryString = new Dictionary<string, string>
            {
                { "date", DateTime.UtcNow.ToString(this.subscriptionDateFormat) },
                { "callback", $"{this.apiUrl}/employee/arrival" },
            };

            var uri = QueryHelpers.AddQueryString(this.subscriptionUrl, queryString);
            return uri;
        }

        private async Task<Subscription> GetResponse(string uri)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception($"Failed to subscribe to service: {response.StatusCode}");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            Subscription subscriptionResponse = Subscription.FromJson(responseBody);
            return subscriptionResponse;
        }

        private void SetInMemoryCache(Subscription subscriptionResponse)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(subscriptionResponse.Expires.Subtract(DateTime.UtcNow));

            this.memoryCache.Set(GlobalConstants.InMemorySubscriptionKey, subscriptionResponse, cacheEntryOptions);
        }
    }
}
