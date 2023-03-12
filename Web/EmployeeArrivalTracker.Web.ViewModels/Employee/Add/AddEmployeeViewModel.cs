namespace EmployeeArrivalTracker.Web.ViewModels.Employee.Add
{
    using System.Collections.Generic;

    using AutoMapper;

    using EmployeeArrivalTracker.Data.Models;
    using EmployeeArrivalTracker.Services.Mapping;

    public class AddEmployeeViewModel : IMapTo<Person>, IMapTo<Employee>, IMapTo<Role>, IMapFrom<Employee>, IHaveCustomMappings
    {
        public string Role { get; set; }

        public int ManagerId { get; set; }

        public IEnumerable<string> Teams { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AddEmployeeViewModel, Role>().ForMember(
                m => m.Title,
                opt => opt.MapFrom(x => x.Role));
        }
    }
}
