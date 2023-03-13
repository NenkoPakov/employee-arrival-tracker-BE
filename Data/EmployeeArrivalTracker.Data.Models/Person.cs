namespace EmployeeArrivalTracker.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EmployeeArrivalTracker.Data.Common.Models;

    public partial class Person : BaseDeletableModel<int>
    {
        private int age;

        public Person()
        {
            this.Employees = new HashSet<Employee>();
        }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The first name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Sur Name")]
        [Required(ErrorMessage = "The sur name is required")]
        public string SurName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The age field is required")]
        [Range(18, 65, ErrorMessage = "Value must be between 18 and 65")]
        public int Age
        {
            get => this.age;
            set
            {
                if (value >= 18 && value <= 65)
                {
                    this.age = value;
                }
                else
                {
                    throw new ArgumentException("Age must be between 18 and 65.");
                }
            }
        }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
