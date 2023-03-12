namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Models;

    public interface IArrivalService
    {
        Task AddAsync(Arrival arrival);
        Task<IEnumerable<Arrival>> AddRangeAsync(IEnumerable<Arrival> arrivals);
        Task<Arrival> GetByEmployeeIdAsync(int employeeId);
    }
}