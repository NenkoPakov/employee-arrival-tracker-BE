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
        public async Task<IEnumerable<EmployeeArrivalDetailsViewModel>> Page(string orderByField, int pageNumber = 1, bool isAscending = true) => await this.arrivalService.GetArrivalsAsync(pageNumber, orderByField, isAscending);

        [HttpGet]
        [Route("count")]
        public async Task<int> Count() => await this.arrivalService.GetCountAsync();
    }
}
