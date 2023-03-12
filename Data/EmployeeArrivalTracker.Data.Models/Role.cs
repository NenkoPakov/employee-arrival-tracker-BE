namespace EmployeeArrivalTracker.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EmployeeArrivalTracker.Data.Common.Models;

    public partial class Role : BaseModel<int>
    {
        public Role()
        {
            this.Employees = new HashSet<Employee>();
        }

        [Display(Name = "Role title")]
        [Required(ErrorMessage = "Role title is requered")]
        public string Title { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
