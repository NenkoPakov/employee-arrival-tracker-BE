namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Models;

    public interface IPersonService
    {
        Task<Person> AddAsync(Person person);

        Task<IEnumerable<Person>> AddRangeAsync(IEnumerable<Person> people);

        Task<bool> CheckIfPersonExistsAsync(int id);

        Task<Person> GetByIdAsync(int managerId);

        Task<IEnumerable<Person>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
