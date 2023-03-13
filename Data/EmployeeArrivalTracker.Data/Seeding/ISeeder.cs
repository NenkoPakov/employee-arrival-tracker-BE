namespace EmployeeArrivalTracker.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data;
    using Microsoft.Extensions.Configuration;

    public interface ISeeder
    {
        Task SeedAsync(EmployeesContext dbContext, IServiceProvider serviceProvider);
    }
}
