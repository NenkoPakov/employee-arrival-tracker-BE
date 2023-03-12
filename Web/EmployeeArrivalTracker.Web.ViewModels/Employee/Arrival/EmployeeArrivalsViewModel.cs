namespace EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival
{
    using System.Collections.Generic;

    public class EmployeeArrivalsViewModel
    {
        public IEnumerable<EmployeeArrivalViewModel> Arrivals { get; set; }
    }
}
