namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Models;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    using Microsoft.EntityFrameworkCore;

    using static EmployeeArrivalTracker.Services.Mapping.AutoMapperConfig;

    public class ArrivalService : IArrivalService
    {
        private const int PageSize = 10;
        private readonly IRepository<Arrival> arrivalRepository;

        public ArrivalService(IRepository<Arrival> arrivalRepository)
        {
            this.arrivalRepository = arrivalRepository;
        }

        public async Task AddAsync(Arrival arrival)
        {
            await this.arrivalRepository.AddAsync(arrival);
            await this.arrivalRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Arrival>> AddRangeAsync(IEnumerable<Arrival> arrivals)
        {
            await this.arrivalRepository.AddRangeAsync(arrivals);
            await this.arrivalRepository.SaveChangesAsync();

            return arrivals;
        }

        public async Task<Arrival> GetByEmployeeIdAsync(int employeeId) => await this.arrivalRepository.All().FirstOrDefaultAsync(a => a.Id == employeeId);

        public async Task<IEnumerable<EmployeeArrivalDetailsViewModel>> GetArrivalsAsync(int pageNumber, string orderByField, bool isAscending = true)
        {
            var query = this.arrivalRepository.All().AsQueryable();

            switch (orderByField)
            {
                case "firstName":
                    query = isAscending ? query.OrderBy(e => e.Employee.Person.FirstName) : query.OrderByDescending(e => e.Employee.Person.FirstName);
                    break;
                case "surName":
                    query = isAscending ? query.OrderBy(e => e.Employee.Person.SurName) : query.OrderByDescending(e => e.Employee.Person.SurName);
                    break;
                case "email":
                    query = isAscending ? query.OrderBy(e => e.Employee.Person.Email) : query.OrderByDescending(e => e.Employee.Person.Email);
                    break;
                case "age":
                    query = isAscending ? query.OrderBy(e => e.Employee.Person.Age) : query.OrderByDescending(e => e.Employee.Person.Age);
                    break;
                case "manager":
                    query = isAscending
                        ? query.OrderBy(e => e.Employee.Manager.Person.FirstName).ThenBy(e => e.Employee.Manager.Person.SurName)
                        : query.OrderByDescending(e => e.Employee.Manager.Person.FirstName).ThenByDescending(e => e.Employee.Manager.Person.SurName);
                    break;
                case "role":
                    query = isAscending ? query.OrderBy(e => e.Employee.Role.Title) : query.OrderByDescending(e => e.Employee.Role.Title);
                    break;
                case "teams":
                    query = isAscending ? query.OrderBy(e => e.Employee.EmployeeTeams.Count) : query.OrderByDescending(e => e.Employee.EmployeeTeams.Count);
                    break;
                case "arrivedAt":
                    query = isAscending ? query.OrderBy(e => e.ArrivedAt) : query.OrderByDescending(e => e.ArrivedAt);
                    break;
                default:
                    break;
            }

            return MapperInstance.Map<IEnumerable<EmployeeArrivalDetailsViewModel>>(await query.Skip((pageNumber - 1) * PageSize)
                                                                                                .Take(PageSize)
                                                                                                .ToListAsync());
        }

        public async Task<int> GetCountAsync() => await this.arrivalRepository.All().CountAsync();
    }
}
