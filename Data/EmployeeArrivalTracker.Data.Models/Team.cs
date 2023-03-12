namespace EmployeeArrivalTracker.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EmployeeArrivalTracker.Data.Common.Models;

    public partial class Team : BaseModel<int>
    {
        public Team()
        {
            this.EmployeeTeams = new HashSet<EmployeeTeam>();
        }

        [Display(Name = "Team")]
        [Required(ErrorMessage = "The team name is required")]
        public string Name { get; set; }

        public virtual ICollection<EmployeeTeam> EmployeeTeams { get; set; }
    }
}
