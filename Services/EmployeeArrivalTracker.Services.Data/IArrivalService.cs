using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeArrivalTracker.Data.Models;
using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

namespace EmployeeArrivalTracker.Services.Data
{
    public interface IArrivalService
    {
        Task AddAsync(Arrival arrival);
        Task<IEnumerable<Arrival>> AddRangeAsync(IEnumerable<Arrival> arrivals);
        Task<IEnumerable<EmployeeArrivalDetailsViewModel>> GetArrivalsAsync(int pageNumber);
        Task<Arrival> GetByEmployeeIdAsync(int employeeId);
    }
}