namespace EmployeeArrivalTracker.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data;

    public interface ISeeder
    {
        Task SeedAsync(EmployeesContext dbContext, IServiceProvider serviceProvider);
    }
}
