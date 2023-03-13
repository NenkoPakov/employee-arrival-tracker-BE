namespace EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using EmployeeArrivalTracker.Data.Models;
    using EmployeeArrivalTracker.Services.Mapping;
    using Newtonsoft.Json;

    public class EmployeeArrivalDetailsViewModel : IMapFrom<Employee>, IMapFrom<Arrival>, IHaveCustomMappings
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("surName")]
        public string SurName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("manager")]
        public string Manager { get; set; }

        [JsonProperty("teams")]
        public IEnumerable<string> Teams { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("arrivedAt")]
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
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Employee.Role.Title))
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.Employee.EmployeeTeams.Select(t => t.Team.Name)))
            .ForMember(dest => dest.ArrivedAt, opt => opt.MapFrom(src => src.ArrivedAt.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}
