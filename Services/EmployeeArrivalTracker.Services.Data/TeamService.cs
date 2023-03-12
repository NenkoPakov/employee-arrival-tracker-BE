namespace EmployeeArrivalTracker.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> teamRepository;

        public TeamService(IRepository<Team> teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public async Task AddAsync(Team team)
        {
            await this.teamRepository.AddAsync(team);
            await this.teamRepository.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Team> teams)
        {
            await this.teamRepository.AddRangeAsync(teams);
            await this.teamRepository.SaveChangesAsync();
        }

        public async Task<Team> GetByNameAsync(string team) => await this.teamRepository.All().FirstOrDefaultAsync(t => t.Name == team);

        public async Task<IEnumerable<Team>> GetByNamesAsync(IEnumerable<string> teams) => await this.teamRepository.All().Where(team => teams.Contains(team.Name)).ToListAsync();
    }
}
