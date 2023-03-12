namespace EmployeeArrivalTracker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using EmployeeArrivalTracker.Data.Common.Models;

    public partial class EmployeeTeam : IAuditInfo, IDeletableEntity
    {
        [NotMapped]
        [Required(ErrorMessage = "The employee id is required")]
        public int EmployeeId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The team id is required")]
        public int TeamId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Team Team { get; set; }
    }
}
