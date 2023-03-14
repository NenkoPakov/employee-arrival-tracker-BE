using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeArrivalTracker.Data.Models;
namespace EmployeeArrivalTracker.Services.Data
{
using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    public interface IArrivalService
    {
        Task AddAsync(Arrival arrival);

        Task<IEnumerable<Arrival>> AddRangeAsync(IEnumerable<Arrival> arrivals);

        Task<IEnumerable<EmployeeArrivalDetailsViewModel>> GetArrivalsAsync(int pageNumber, string orderByField, bool isAscending = true);

        Task<Arrival> GetByEmployeeIdAsync(int employeeId);

        Task<int> GetCountAsync();
    }
}