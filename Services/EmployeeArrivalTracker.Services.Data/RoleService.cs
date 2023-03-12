namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> roleRepository;

        public RoleService(IRepository<Role> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task AddAsync(Role role)
        {
            await this.roleRepository.AddAsync(role);
            await this.roleRepository.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Role> roles)
        {
            await this.roleRepository.AddRangeAsync(roles);
            await this.roleRepository.SaveChangesAsync();
        }

        public async Task<Role> GetByTitleAsync(string title) => await this.roleRepository.All().FirstOrDefaultAsync(x => x.Title == title);

        public async Task<IEnumerable<Role>> GetByTitlesAsync(IEnumerable<string> titles) => await this.roleRepository.All().Where(title => titles.Contains(title.Title)).ToListAsync();
    }
}
