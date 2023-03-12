namespace EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using EmployeeArrivalTracker.Data.Models;
    using EmployeeArrivalTracker.Services.Mapping;

    public class EmployeeArrivalDetailsViewModel : IMapFrom<Employee>, IMapFrom<Arrival>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string Manager { get; set; }

        public IEnumerable<string> Teams { get; set; }

        public string RoleTitle { get; set; }

        public string ArrivedAt { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Employee, EmployeeArrivalDetailsViewModel>().ForMember(
                m => m.Manager,
                opt => opt.MapFrom(x => $"{x.Manager.Person.FirstName} {x.Manager.Person.SurName}"));

            configuration.CreateMap<Arrival, EmployeeArrivalDetailsViewModel>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.Person.FirstName))
            .ForMember(dest => dest.SurName, opt => opt.MapFrom(src => src.Employee.Person.SurName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Employee.Person.Email))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Employee.Person.Age))
            .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => $"{src.Employee.Manager.Person.FirstName} {src.Employee.Manager.Person.SurName}"))
            .ForMember(dest => dest.RoleTitle, opt => opt.MapFrom(src => src.Employee.Role.Title))
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.Employee.EmployeeTeams.Select(t => t.Team.Name)))
            .ForMember(dest => dest.ArrivedAt, opt => opt.MapFrom(src => src.ArrivedAt.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}
