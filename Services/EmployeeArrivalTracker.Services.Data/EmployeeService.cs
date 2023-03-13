namespace EmployeeArrivalTracker.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

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

        public async Task<bool> CheckIfPersonExistsAsync(int id) => await this.employeeRepository.All().AnyAsync(e => e.Id == id);

        public async Task AddAsync(AddEmployeeViewModel employee)
        {
            try
            {
                var newEmployee = new Employee();

                newEmployee.Person = MapperInstance.Map<Person>(employee);
                newEmployee.Manager = await this.GetByIdAsync(employee.ManagerId);
                newEmployee.Role = await this.roleService.GetByTitleAsync(employee.Role) ?? new Role { Title = employee.Role };

                var teams = await this.teamService.CreateIfNotExistsAwait(employee.Teams);
                newEmployee.EmployeeTeams = this.AssignEmployeeToTeams(teams, newEmployee).ToList();

                await this.employeeRepository.AddAsync(newEmployee);
                await this.employeeRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddRangeAsync(IEnumerable<AddEmployeeViewModel> employees)
        {
            try
            {
                var people = MapperInstance.Map<IEnumerable<Person>>(employees);

                var allRoles = await this.roleService.CreateIfNotExistsAwait(employees.Select(e => e.Role.Trim()).Distinct());
                var allTeams = await this.teamService.CreateIfNotExistsAwait(employees.SelectMany(e => e.Teams).Distinct());

                var newEmployees = new List<Employee>();
                foreach (var employee in employees)
                {
                    var newEmployee = new Employee();

                    newEmployee.Person = people.FirstOrDefault(p => employee.FirstName == p.FirstName &&
                                                                    employee.SurName == p.SurName &&
                                                                    employee.Age == p.Age);
                    newEmployee.Manager = await this.GetByIdAsync(employee.ManagerId);
                    newEmployee.Role = allRoles.FirstOrDefault(r => r.Title == employee.Role.Trim());
                    newEmployee.EmployeeTeams = this.AssignEmployeeToTeams(allTeams.Where(t => employee.Teams.Contains(t.Name)), newEmployee).ToList();

                    newEmployees.Add(newEmployee);
                }

                await this.employeeRepository.AddRangeAsync(newEmployees);
                await this.employeeRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddArrivalsAsync(IEnumerable<EmployeeArrivalViewModel> arrivals)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        private IEnumerable<EmployeeTeam> AssignEmployeeToTeams(IEnumerable<Team> teams, Employee employee)
        {
            return teams.Select(t => new EmployeeTeam { Employee = employee, Team = t })
                        .ToList();
        }
    }
}
