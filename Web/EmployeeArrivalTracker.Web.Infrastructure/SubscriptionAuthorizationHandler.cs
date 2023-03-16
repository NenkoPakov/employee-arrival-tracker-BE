namespace EmployeeArrivalTracker.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Common;
    using EmployeeArrivalTracker.Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;

    public class SubscriptionAuthorizationHandler : AuthorizationHandler<SubscriptionRequirement>
    {
        private readonly IMemoryCache memoryCache;
        private readonly HttpContext httpContext;

        public SubscriptionAuthorizationHandler(IMemoryCache memoryCache, IHttpContextAccessor httpContext)
        {
            this.memoryCache = memoryCache;
            this.httpContext = httpContext.HttpContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SubscriptionRequirement requirement)
        {
            if (!this.memoryCache.TryGetValue(GlobalConstants.InMemorySubscriptionKey, out Subscription subscription))
            {
                context.Fail();
                this.httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (!this.httpContext.Request.Headers.TryGetValue("X-Fourth-Token", out var token))
            {
                this.httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await this.httpContext.Response.WriteAsync("Missing token");
                return;
            }

            if (token != subscription?.Token)
            {
                this.httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await this.httpContext.Response.WriteAsync("Invalid token");
                return;
            }

            if (DateTime.UtcNow.Date > subscription?.Expires.Date)
            {
                this.httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await this.httpContext.Response.WriteAsync("Invalid date");
                return;
            }

            context.Succeed(requirement);
        }
    }
}
