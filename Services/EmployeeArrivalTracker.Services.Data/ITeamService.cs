namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Models;

    public interface ITeamService
    {
        Task AddRangeAsync(IEnumerable<Team> teams);

        Task<Team> GetByNameAsync(string team);

        Task<IEnumerable<Team>> GetByNamesAsync(IEnumerable<string> teams);

        Task AddAsync(Team team);
        Task<IEnumerable<Team>> CreateIfNotExistsAwait(IEnumerable<string> teams);
    }
}
