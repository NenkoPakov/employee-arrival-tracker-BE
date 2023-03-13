namespace EmployeeArrivalTracker.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Services.Data;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }

        [HttpGet]
        [Route("Subscribe")]
        public async Task Subscribe() => await this.subscriptionService.SubscribeAsync();

    }
}
