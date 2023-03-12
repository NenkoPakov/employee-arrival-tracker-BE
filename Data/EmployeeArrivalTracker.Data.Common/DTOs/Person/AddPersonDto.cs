namespace EmployeeArrivalTracker.Data.Common.DTOs.Person
{
    using EmployeeArrivalTracker.Services.Mapping;
    using EmployeeArrivalTracker.Web.ViewModels.Employee;

    public class AddPersonDto : IMapFrom<AddEmployeeArrivalDto>
    {
        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }
    }
}
