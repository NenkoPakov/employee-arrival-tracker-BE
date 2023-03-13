namespace EmployeeArrivalTracker.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EmployeeArrivalTracker.Services;
    using EmployeeArrivalTracker.Services.Data;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class ArrivalController : BaseController
    {
        private readonly IArrivalService arrivalService;

        public ArrivalController(IArrivalService arrivalService)
        {
            this.arrivalService = arrivalService;
        }

        [HttpGet]
        [Route("Page")]
        public async Task<IEnumerable<EmployeeArrivalDetailsViewModel>> Page(int pageNumber) => await this.arrivalService.GetArrivalsAsync(pageNumber);
    }
}
