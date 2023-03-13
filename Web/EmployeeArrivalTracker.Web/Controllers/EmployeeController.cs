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
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }


        [HttpGet]
        public async Task<IEnumerable<EmployeeArrivalDetailsViewModel>> GetAll() => await this.employeeService.GetAllAsync<EmployeeArrivalDetailsViewModel>();

        [HttpPost]
        [Route("Add")]
        public async Task Add(AddEmployeeViewModel employee) => await this.employeeService.AddAsync(employee);

        [HttpPost]
        [Route("Arrival")]
        public async Task Arrivals(IEnumerable<EmployeeArrivalViewModel> arrivals) => await this.employeeService.AddArrivalsAsync(arrivals);
    }
}
