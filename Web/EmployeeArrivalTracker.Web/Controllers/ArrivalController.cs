namespace EmployeeArrivalTracker.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Services.Data;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("arrival")]
    public class ArrivalController : BaseController
    {
        private readonly IArrivalService arrivalService;

        public ArrivalController(IArrivalService arrivalService)
        {
            this.arrivalService = arrivalService;
        }

        [HttpGet]
        [Route("page/{pageNumber:int?}")]
        public async Task<ActionResult<IEnumerable<EmployeeArrivalDetailsViewModel>>> Page(string orderByField, int pageNumber = 1, bool isAscending = true)
            => this.Ok(await this.arrivalService.GetArrivalsAsync(pageNumber, orderByField, isAscending));

        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<int>> Count() => this.Ok(await this.arrivalService.GetCountAsync());
    }
}
