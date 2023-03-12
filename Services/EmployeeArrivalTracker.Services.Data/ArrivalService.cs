namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class ArrivalService : IArrivalService
    {
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

        public async Task<Arrival> GetByEmployeeIdAsync(int employeeId)
        {
            return await this.arrivalRepository.All().FirstOrDefaultAsync(a => a.Id == employeeId);
        }
    }
}
