namespace EmployeeArrivalTracker.Web.ViewModels.Employee
{
    using System.Collections.Generic;

    using EmployeeArrivalTracker.Services.Mapping;

    public class AddEmployeeArrivalDto
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int ManagerId { get; set; }

        public IEnumerable<string> Teams { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }
    }
}
