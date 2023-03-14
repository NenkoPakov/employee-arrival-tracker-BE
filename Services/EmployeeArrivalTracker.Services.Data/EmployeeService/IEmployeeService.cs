namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Models;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    public interface IEmployeeService
    {
        Task AddArrivalsAsync(IEnumerable<EmployeeArrivalViewModel> arrivals);

        Task AddAsync(AddEmployeeViewModel employee);

        Task<bool> CheckIfPersonExistsAsync(int id);

        Task<IEnumerable<Employee>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<Employee> GetByIdAsync(int id);

        Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<int> ids);

        Task<int> GetCountAsync();

        Task AddRangeAsync(IEnumerable<AddEmployeeViewModel> employees);
    }
}
