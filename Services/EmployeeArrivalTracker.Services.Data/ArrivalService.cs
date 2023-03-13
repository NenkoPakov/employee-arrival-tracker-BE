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

        public async Task<IEnumerable<EmployeeArrivalDetailsViewModel>> GetArrivalsAsync(int pageNumber) =>
            MapperInstance.Map<IEnumerable<EmployeeArrivalDetailsViewModel>>(await this.arrivalRepository.All()
                          .AsQueryable()
                          .Skip((pageNumber - 1) * PageSize)
                          .Take(PageSize)
                          .ToListAsync());
    }
}
