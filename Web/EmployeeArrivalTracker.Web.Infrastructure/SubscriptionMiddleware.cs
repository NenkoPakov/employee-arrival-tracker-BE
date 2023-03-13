namespace EmployeeArrivalTracker.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;

    public class SubscriptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMemoryCache memoryCache;

        public SubscriptionMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            this.next = next;
            this.memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/Employee/Arrival"))
            {
                this.memoryCache.TryGetValue("subscribe", out Subscription subscription);

                if (!context.Request.Headers.TryGetValue("X-Fourth-Token", out var token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Missing token");
                    return;
                }

                if (token != subscription?.Token)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token");
                    return;
                }

                if (DateTime.UtcNow.Date != subscription?.Expires.Date)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid date");
                    return;
                }
            }

            await this.next(context);
        }
    }
}
