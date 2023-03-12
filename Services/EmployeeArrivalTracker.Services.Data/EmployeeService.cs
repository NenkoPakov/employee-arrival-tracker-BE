namespace EmployeeArrivalTracker.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Transactions;

    using EmployeeArrivalTracker.Data.Common.Hubs;
    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Models;
    using EmployeeArrivalTracker.Services.Mapping;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    using static EmployeeArrivalTracker.Services.Mapping.AutoMapperConfig;

    public class EmployeeService : IEmployeeService
    {
        private readonly IDeletableEntityRepository<Employee> employeeRepository;
        private readonly IRepository<EmployeeTeam> employeeTeamRepository;
        private readonly IPersonService personService;
        private readonly IRoleService roleService;
        private readonly ITeamService teamService;
        private readonly IArrivalService arrivalService;
        private readonly IHubContext<EmployeesHub> hubContext;

        public EmployeeService(IDeletableEntityRepository<Employee> employeeRepository, IRepository<EmployeeTeam> employeeTeamRepository, IPersonService personService, IRoleService roleService, ITeamService teamService, IArrivalService arrivalService, IHubContext<EmployeesHub> hubContext)
        {
            this.employeeRepository = employeeRepository;
            this.employeeTeamRepository = employeeTeamRepository;
            this.personService = personService;
            this.roleService = roleService;
            this.teamService = teamService;
            this.arrivalService = arrivalService;
            this.hubContext = hubContext;
        }

        public async Task<int> GetCountAsync() => await this.employeeRepository.AllAsNoTracking().CountAsync();

        public async Task<IEnumerable<T>> GetAllAsync<T>() => await this.employeeRepository.All().To<T>().ToListAsync();

        public async Task<IEnumerable<Employee>> GetAllAsync() => await this.employeeRepository.All().ToListAsync();

        public async Task<Employee> GetByIdAsync(int id) => await this.employeeRepository.All().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<int> ids) => await this.employeeRepository.All().Where(e => ids.Contains(e.Id)).ToListAsync();

        public async Task<bool> CheckIfPersonExistsAsync(int id) => await this.GetByIdAsync(id) != null;

        public async Task AddAsync(AddEmployeeViewModel employee)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var person = MapperInstance.Map<Person>(employee);
                    await this.personService.AddAsync(person);

                    var role = await this.roleService.GetByTitleAsync(employee.Role);
                    if (role == null)
                    {
                        role = new Role { Title = employee.Role };
                        await this.roleService.AddAsync(role);
                    }

                    var teams = new List<Team>();
                    foreach (var employeeTeam in employee.Teams)
                    {
                        var team = await this.teamService.GetByNameAsync(employeeTeam);
                        if (team == null)
                        {
                            team = new Team { Name = employeeTeam };
                            await this.teamService.AddAsync(team);
                        }

                        teams.Add(team);
                    }

                    await this.teamService.AddRangeAsync(teams);

                    int? managerId = employee.ManagerId != 0
                        ? await this.personService.CheckIfPersonExistsAsync(employee.ManagerId)
                            ? employee.ManagerId
                            : null :
                        null;

                    var newEmployee = new Employee()
                    {
                        PersonId = person.Id,
                        RoleId = role.Id,
                        ManagerId = managerId,
                    };

                    await this.employeeRepository.AddAsync(newEmployee);
                    await this.employeeRepository.SaveChangesAsync();

                    await this.employeeTeamRepository.AddRangeAsync(teams.Select(t => new EmployeeTeam { EmployeeId = newEmployee.Id, TeamId = t.Id }));
                    await this.employeeTeamRepository.SaveChangesAsync();

                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public async Task AddRangeAsync(IEnumerable<AddEmployeeViewModel> employees)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var people = MapperInstance.Map<IEnumerable<Person>>(employees);
                    await this.personService.AddRangeAsync(people);

                    var existingRoles = await this.roleService.GetByTitlesAsync(employees.Select(e => e.Role));
                    var missingRoles = MapperInstance.Map<IEnumerable<Role>>(employees.Where(e => !existingRoles.Select(r => r.Title).Contains(e.Role)));
                    await this.roleService.AddRangeAsync(missingRoles);

                    var allRoles = existingRoles.Union(missingRoles);

                    var allTeamsNames = employees.SelectMany(e => e.Teams).Distinct();
                    var existingTeams = await this.teamService.GetByNamesAsync(allTeamsNames);
                    var existingTeamsNames = existingTeams.Select(x => x.Name);
                    var missingTeams = allTeamsNames.Where(team => !existingTeamsNames.Contains(team)).Select(mt => new Team { Name = mt }).ToList();
                    await this.teamService.AddRangeAsync(missingTeams);

                    var allTeams = existingTeams.Union(missingTeams);

                    var newEmployees = new List<Employee>();
                    foreach (var employee in employees)
                    {
                        int? managerId = employee.ManagerId != 0
                        ? await this.personService.CheckIfPersonExistsAsync(employee.ManagerId)
                            ? employee.ManagerId
                            : null :
                        null;

                        var person = people.FirstOrDefault(p => employee.FirstName == p.FirstName &&
                                                                employee.SurName == p.SurName &&
                                                                employee.Age == p.Age);

                        var role = allRoles.FirstOrDefault(r => r.Title == employee.Role.Trim());

                        var newEmployee = new Employee()
                        {
                            PersonId = person.Id,
                            RoleId = role.Id,
                            ManagerId = managerId,
                        };

                        var teams = allTeams.Where(r => employee.Teams.Contains(r.Name));
                        var employeeTeams = teams.Select(t => new EmployeeTeam { EmployeeId = person.Id, TeamId = t.Id });
                        newEmployee.EmployeeTeams = employeeTeams.ToList();

                        newEmployees.Add(newEmployee);
                    }

                    await this.employeeRepository.AddRangeAsync(newEmployees);
                    await this.employeeRepository.SaveChangesAsync();

                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public async Task AddArrivalsAsync(IEnumerable<EmployeeArrivalViewModel> arrivals)
        {
            var existingEmployees = await this.GetByIdsAsync(arrivals.Select(a => a.EmployeeId));
            var existingEmployeesId = existingEmployees.Select(e => e.Id);
            var validArrivals = arrivals.Where(a => existingEmployeesId.Contains(a.EmployeeId));

            var mappedArrivals = MapperInstance.Map<IEnumerable<Arrival>>(validArrivals);
            await this.arrivalService.AddRangeAsync(mappedArrivals);

            var mappedArrivalsDetails = MapperInstance.Map<IEnumerable<EmployeeArrivalDetailsViewModel>>(mappedArrivals);
            string serializedArrivals = JsonSerializer.Serialize(mappedArrivalsDetails);
            await this.hubContext.Clients.All.SendAsync(EmployeesHub.MethodName, serializedArrivals);
        }
    }
}
