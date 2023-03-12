namespace EmployeeArrivalTracker.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EmployeeArrivalTracker.Data.Common.Models;

    public partial class Employee : BaseDeletableModel<int>
    {
        public Employee()
        {
            this.Arrivals = new HashSet<Arrival>();
            this.EmployeeTeams = new HashSet<EmployeeTeam>();
            this.InverseManager = new HashSet<Employee>();
        }

        [Required(ErrorMessage = "The person id is required")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "The role id is required")]
        public int RoleId { get; set; }

        public int? ManagerId { get; set; }

        public virtual Employee Manager { get; set; }

        public virtual Person Person { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<Arrival> Arrivals { get; set; }

        public virtual ICollection<EmployeeTeam> EmployeeTeams { get; set; }

        public virtual ICollection<Employee> InverseManager { get; set; }
    }
}
