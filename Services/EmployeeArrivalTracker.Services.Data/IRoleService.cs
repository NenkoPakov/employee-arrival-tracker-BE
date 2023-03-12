namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Models;

    public interface IRoleService
    {
        Task AddAsync(Role role);

        Task AddRangeAsync(IEnumerable<Role> roles);

        Task<Role> GetByTitleAsync(string title);

        Task<IEnumerable<Role>> GetByTitlesAsync(IEnumerable<string> titles);
    }
}
