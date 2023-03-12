namespace EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival
{
    using System;
    using System.Globalization;

    using AutoMapper;

    using EmployeeArrivalTracker.Data.Models;
    using EmployeeArrivalTracker.Services.Mapping;

    public class EmployeeArrivalViewModel : IMapTo<Arrival>, IHaveCustomMappings
    {
        public int EmployeeId { get; set; }

        public string When { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<EmployeeArrivalViewModel, Arrival>().ForMember(
                m => m.ArrivedAt,
                opt => opt.MapFrom(x => DateTime.Parse(x.When, CultureInfo.CurrentCulture)));
        }
    }
}
