namespace EmployeeArrivalTracker.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("subscription")]
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }

        [HttpGet]
        [Route("subscribe")]
        public async Task<ActionResult> Subscribe()
        {
            try
            {
                await this.subscriptionService.SubscribeAsync();
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
    }
}
