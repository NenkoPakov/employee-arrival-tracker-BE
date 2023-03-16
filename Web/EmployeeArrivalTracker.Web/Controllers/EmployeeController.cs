namespace EmployeeArrivalTracker.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Services.Data;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("employee")]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeArrivalDetailsViewModel>>> GetAll()
            => this.Ok(await this.employeeService.GetAllAsync<EmployeeArrivalDetailsViewModel>());

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> Add(AddEmployeeViewModel employee)
        {
            await this.employeeService.AddAsync(employee);
            return this.Ok();
        }

        [HttpPost]
        [Route("arrival")]
        [Authorize(Policy = "Token")]
        public async Task<ActionResult> Arrivals(IEnumerable<EmployeeArrivalViewModel> arrivals)
        {
            await this.employeeService.AddArrivalsAsync(arrivals);
            return this.Ok();
        }
    }
}
