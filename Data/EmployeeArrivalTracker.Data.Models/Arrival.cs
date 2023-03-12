namespace EmployeeArrivalTracker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using EmployeeArrivalTracker.Data.Common.Models;

    public partial class Arrival : IdentityModel<int>
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime ArrivedAt { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
